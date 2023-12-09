using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using Unity.AI.Navigation;
using EnemyStuff;

namespace ModuleSnapping
{
    public class Generator : MonoBehaviorSingleton<Generator>
    {
        [SerializeField] private int seed;
        [SerializeField] private int lastPlayerCount = 1;
        //[SerializeField] private List<PlayerHandler> playerList2 = new List<PlayerHandler>();
        [SerializeField] private PlayerMovement[] playerList2;

        [SerializeField] private Tileset tileset; //Stores all modules that can be used in X area
        [SerializeField] private uint maxIterations; //Stores how many times the process can repeat, dictates map size1
        [SerializeField] private uint maxAttempts; //Stores max amount of times a module can try to be connected to another module

        [Header("Debugging")]
        [SerializeField] bool runOnStart = false;
        [SerializeField] bool useSlowGen = false;
        [SerializeField] private bool inProgress = false; //Stores if we are already generating a map, will switch to false when we are finished
        [SerializeField] private bool lastRoomGenerated = false; //Stores if we have generated the last room
        [SerializeField] private GameObject block;
        private Photon.Pun.PhotonView view;

        private List<Connection> allPendingConnections;

        private bool clearingInProgress = false;
        private bool monsterSpawner = false;

        private String lastModule = "";

        //[SerializeField] private String startPointName;
        [SerializeField] private String endPointName;

        [SerializeField] List<Connection> pendingConnections = new List<Connection>();
        [SerializeField] List<Connection> allConnections = new List<Connection>();

        private void Start()
        {
            view = GetComponent<PhotonView>();

            if (runOnStart && Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                seed = UnityEngine.Random.Range(0, int.MaxValue);
                UnityEngine.Random.InitState(seed);
                GenerateModules();
            }

        }

        private void Update()
        {
            if(FindObjectsOfType<EnemySpawner>().Length > 0)
            {
                monsterSpawner = true;
            }
            else
            {
                monsterSpawner = false;
            }
        }

        /*public void Update()
        {
            playerList2 = FindObjectsOfType<PlayerMovement>();

            if(Photon.Pun.PhotonNetwork.PlayerList.Length > lastPlayerCount)
            {
                lastPlayerCount++;



                foreach (PlayerMovement player in playerList2)
                {
                    if (player.GetComponent<PhotonView>().IsMine)
                    {
                        OnPhotonPlayerConnected(player.GetComponent<PhotonView>().Owner);
                    }
                }
            }
        }*/

        /*public void OnConnectedToMaster(Photon.Realtime.Player player)
        {
            if(Photon.Pun.PhotonNetwork.PlayerList.Length > 1)
            {
                Debug.Log("bruh");
                if (Photon.Pun.PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("poop");
                    view.RPC("SetSeed", player, seed);
                }
            }
        }

        [PunRPC]
        public void SetSeed(int Seed)
        {
            seed = Seed;
            UnityEngine.Random.InitState(seed);
            GenerateModules();
        }*/

        public void GenerateModules()
        {
            if (inProgress) //we're already generating a map, so don't make another one
                return;

            lastRoomGenerated = false;

            //ensure there is no other maps generated already
            if (transform.childCount == 0)
            {
                //No dungeon already exists, so generate a dungeon and return out of this method
                StartCoroutine(GenerateEnvironment());
                return;
            }
            else if (clearingInProgress == false)
            {
                clearingInProgress = true;
                ClearModules();
                //GenerateModules();
            }
        }

        private IEnumerator GenerateEnvironment()
        {
            inProgress = true;


            //create a list to store all modules we load, and choose a random starting module
            List<Module> loadedModules = new List<Module>();
            int startingModuleRNG = UnityEngine.Random.Range(0, tileset.startingModules.Length);

            //Spawn the starting module as a prefab and make it a child of the generator
            GameObject startingModulePrefab = Photon.Pun.PhotonNetwork.Instantiate(tileset.startingModules[startingModuleRNG].name, Vector3.zero, Quaternion.identity);
            startingModulePrefab.transform.parent = transform;

            //Add the module component of the starting module to the loaded modules list
            Module startingModule = startingModulePrefab.GetComponent<Module>();
            loadedModules.Add(startingModule);

            //List<Connection> pendingConnections = new List<Connection>(startingModule.GetConnections);
            pendingConnections = new List<Connection>(startingModule.GetConnections);



            // set our iterations index to 0 for the first iteration
            int iteration = 0;

            while (iteration < maxIterations) //this code will loop until we hit the max iterations number
            {
                //Create a new list to store all new connections
                List<Connection> newConnections = new List<Connection>();

                //Loop through all the connections that have been confirmed and are in the pendingConnections list
                for (int connectionIndex = 0; connectionIndex < pendingConnections.Count; connectionIndex++)
                {
                    if (lastRoomGenerated)
                        break;

                    //Check if there is a connection at this index and if it is active 
                    if (pendingConnections[connectionIndex] == null || pendingConnections[connectionIndex].isActiveAndEnabled == false)
                    {
                        //If we end up here, there is either no connection at this index OR the connection is not active
                        continue;
                    }

                    //Loops until there is a successful module created and connected, OR we reach our maximum attempts
                    for (int curAttempt = 0; curAttempt < maxAttempts; curAttempt++)
                    {
                        //This spawns a random module of a valid type
                        //Put into position with correct rotation
                        //If it fits, it sits
                        //If it doesn't fit, try again, that is one attempt.

                        //Get a random valid connection type from the given connection
                        ModuleType validType = GetRandom(pendingConnections[connectionIndex].GetValidConnections);

                        //create a variable to store the prefab, and get a module of the given type
                        GameObject newModulePrefab = GetModulePrefabOfType(validType, iteration, lastModule);

                        lastModule = newModulePrefab.ToString();
                        //Debug.Log(newModulePrefab.ToString());

                        GameObject prefab = Photon.Pun.PhotonNetwork.Instantiate(newModulePrefab.name, Vector3.zero, Quaternion.identity);
                        prefab.transform.parent = transform;
                        Module newModule = prefab.GetComponent<Module>();
                        Connection[] newModuleConnections = newModule.GetConnections;
                        Connection exitToMatch = newModuleConnections.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleConnections);
                        MatchExits(pendingConnections[connectionIndex], exitToMatch);
                        newConnections.AddRange(newModuleConnections.Where(e => e != exitToMatch));

                        //Will store if a collision is found
                        bool collisionFound = false;

                        //Wait a frame to ensure module is in position


                        if (useSlowGen)
                            yield return new WaitForSeconds(0.5f);          // SLOW GEN
                        else
                            yield return null;


                        //Check if there has been a collision
                        //If so, remove the new module and try again
                        if (newModule.GetIgnoreCollision == false)
                        {
                            collisionFound = newModule.CollisionCheck();
                            if (collisionFound == true)
                            {
                                //Debug.Log("Collision found.");
                                //Debug.Log(newModule.gameObject.ToString());
                                Photon.Pun.PhotonNetwork.Destroy(newModule.gameObject);     //  !!!!
                                break;
                            }
                        }

                        //Sanity check, should not be needed, but makes sure that no collision took place
                        if (collisionFound == true)
                        {
                            //The element did not fit
                            Debug.Log("Removed odd element.");
                            Destroy(newModule.gameObject);
                        }
                        else
                        {
                            if (iteration == maxIterations - 1)
                            {
                                lastRoomGenerated = true;
                                GetComponent<NavMeshSurface>().BuildNavMesh();
                            }

                            if(!monsterSpawner)
                            {
                                ClearModules();
                            }

                            //the new module fits with no issues, so turn off the connections and add the new module to the loadedModules list
                            pendingConnections[connectionIndex].gameObject.SetActive(false);
                            exitToMatch.gameObject.SetActive(false);
                            loadedModules.Add(newModule);

                            //Debug.Log(loadedModules[loadedModules.Count - 1].ToString());
                            break;
                        }
                    }
                }

                //Add new connections to the pendingConnections list and uptick the iteration index
                allConnections.AddRange(pendingConnections);
                pendingConnections = newConnections;

                iteration++;

            }



            yield return null;

            allConnections.AddRange(pendingConnections);

            foreach (Connection con in allConnections)
            {
                if (con != null && con.isActiveAndEnabled == true)
                {
                    GameObject go = Photon.Pun.PhotonNetwork.Instantiate(block.name, con.transform.position, Quaternion.identity);
                    go.transform.parent = transform;
                    //Debug.Log($"Block generated at ({con.transform.position.x}, {con.transform.position.y}, {con.transform.position.z})");
                }
            }

            inProgress = false;


            //we've completed our iterations, now we can try to place the last room
            //go through all pending connections and try to place the last room.
            //attempt to place a room module from the


            inProgress = false;
            if (lastRoomGenerated == false)
            {
                Debug.Log("The last room was not generated.");
                yield return new WaitForSeconds(.1f);
                ClearModules();
            }
            else
            {
                Debug.Log("Capping unused connections");
                //we generated the last room, so cap the unused connections
                //CapUnusedConnections(pendingConnections);
                for (int i = 0; i < pendingConnections.Count; i++)
                {
                    if (pendingConnections[i] == null)
                    {
                        continue;
                    }

                    if (pendingConnections[i].CapIfUnused == false || pendingConnections[i] != null)
                    {
                        //we don't want to cap this connection
                        Debug.Log("This connection should not be capped: " + pendingConnections[i].name);
                    }
                    else
                    {
                        //generate a random number
                        int rng = UnityEngine.Random.Range(0, tileset.capModules.Length);

                        //spawn a cap module of index rng
                        GameObject capPrefab = Photon.Pun.PhotonNetwork.Instantiate(tileset.capModules[rng].name, Vector3.zero, Quaternion.identity);
                        capPrefab.transform.parent = transform;
                        Module newModule = capPrefab.GetComponent<Module>();
                        Connection[] newModuleConnections = newModule.GetConnections;
                        Connection exitToMatch = newModuleConnections[0];
                        //view.RPC("MatchExits", RpcTarget.All, pendingConnections[i], exitToMatch);
                        MatchExits(pendingConnections[i], exitToMatch);
                    }
                }
            }
        }

        /// <summary>
        /// Get both connection points and put them at the same position
        /// Rotate the new connection to be the mirror of the old connection
        /// </summary>
        /// <param name="_oldExit">The original exit that we will not rotate</param>
        /// <param name="_newExit">The new exit which will be rotated</param>
        ///[PunRPC]
        private void MatchExits(Connection _oldExit, Connection _newExit)
        {
            Transform newModuleObject = _newExit.transform.parent;
            Vector3 matchingVector = -_oldExit.transform.forward;
            float correctiveRotation = CorrectiveRotation(matchingVector) - CorrectiveRotation(_newExit.transform.forward);
            newModuleObject.RotateAround(_newExit.transform.position, Vector3.up, correctiveRotation);
            Vector3 correctiveTranslation = _oldExit.transform.position - _newExit.transform.position;
            newModuleObject.transform.position += correctiveTranslation;
        }

        /// <summary>
        /// Calculate the angle needed to rotate a vector so that it is the MIRROR of the given vector
        /// </summary>
        /// <param name="matchingVector">The vector you would like to mirror</param>
        /// <returns>The angle for the resulting mirrored vector</returns>
        private float CorrectiveRotation(Vector3 matchingVector)
        {
            return Vector3.Angle(Vector3.forward, matchingVector) * Mathf.Sign(matchingVector.x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validType"></param>
        /// <returns></returns>
        private GameObject GetModulePrefabOfType(ModuleType validType, int iteration, String lastModule)
        {
            //create a list to store all our valid choices for modules
            List<GameObject> validChoices = new List<GameObject>();

            //check if we are on the last iteration
            if (iteration >= maxIterations - 1)
            {
                //This code is for the last iteration
                for (int i = 0; i < tileset.endModules.Length; i++)
                {

                    validType = ModuleType.Room;
                    GameObject curModuleObject = tileset.endModules[i];
                    Module curModule = curModuleObject.GetComponent<Module>();

                    for (int x = 0; x < curModule.GetModuleTypes.Length; x++)
                    {
                        if (curModule.GetModuleTypes[x] == validType)
                        {
                            validChoices.Add(curModuleObject);
                        }
                    }
                }
            }
            else
            {
                //This code is for all other iterations
                for (int i = 0; i < tileset.allModules.Length; i++)
                {
                    GameObject curModuleObject = tileset.allModules[i];
                    Module curModule = curModuleObject.GetComponent<Module>();

                    for (int x = 0; x < curModule.GetModuleTypes.Length; x++)
                    {
                        if (curModule.GetModuleTypes[x] == validType)
                        {
                            validChoices.Add(curModuleObject);
                        }
                    }
                }
            }

            if (validChoices.Count > 0)
            {
                if (iteration > 6 && !monsterSpawner && validChoices.Contains(Resources.Load("MonsterRoom") as GameObject))
                {
                    return Resources.Load("MonsterRoom") as GameObject;
                }

                if(validChoices.Contains(Resources.Load("MonsterRoom") as GameObject))
                {
                    validChoices.Remove(Resources.Load("MonsterRoom") as GameObject);
                }

                return validChoices[UnityEngine.Random.Range(0, validChoices.Count)];
            }
            else
            {
                Debug.Log("No valid choices found.");
                return null;
            }
        }

        /// <summary>
        /// Pass in a generic array and get a value from the array
        /// Can be used for ANY array
        /// </summary>
        /// <typeparam name="TItem">The data type of the array</typeparam>
        /// <param name="_array">The array to select from</param>
        /// <returns>A random element from the given array</returns>
        private TItem GetRandom<TItem>(TItem[] _array) //this lets you pass in an array of any value type
        {
            return _array[UnityEngine.Random.Range(0, _array.Length)];
        }

        /// <summary>
        /// This can be improved
        /// currently uses recursive programming
        /// this was for instructional purposes only, and is not the best way here
        /// </summary>
        private void ClearModules()
        {
            StopCoroutine(GenerateEnvironment());

            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                if (inProgress || transform.childCount == 0)
                {
                    return;
                }

                Transform[] p = GetComponentsInChildren<Transform>();

                for (int i = p.Length - 1; i >= 0; i--)
                {
                    if (p[i] != transform && p[i].GetComponent<PhotonView>())
                    {
                        Photon.Pun.PhotonNetwork.Destroy(p[i].gameObject);
                    }
                }

                clearingInProgress = false;
                StartCoroutine(GenerateEnvironment());

                //if in progress or there is already no children
                if (inProgress || transform.childCount == 0)
                {
                    return;
                }
                /*
                //if in progress or there is already no children
                if (inProgress || transform.childCount == 0)
                {
                    return;
                }
                foreach (Transform item in transform)
                {
                    Photon.Pun.PhotonNetwork.Destroy(item.gameObject);
                }
                clearingInProgress = false;
                StartCoroutine(GenerateEnvironment());
                */
            }
        }

        /*void CapUnusedConnections(List<Connection> _connections)
        {
            Debug.Log("Capping unused connections");
            allPendingConnections = _connections;

            for (int i = 0; i < _connections.Count; i++)
            {
                if (_connections[i].CapIfUnused == false)
                {
                    //we don't want to cap this connection
                    Debug.Log("This connection should not be capped: " + _connections[i].name);
                    continue;
                }

                //generate a random number
                int rng = UnityEngine.Random.Range(0, tileset.capModules.Length);

                //spawn a cap module of index rng
                GameObject capPrefab = Photon.Pun.PhotonNetwork.Instantiate(tileset.capModules[rng].name, Vector3.zero, Quaternion.identity);
                capPrefab.transform.parent = transform;
                Module newModule = capPrefab.GetComponent<Module>();
                Connection[] newModuleConnections = newModule.GetConnections;
                Connection exitToMatch = newModuleConnections.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleConnections);
                MatchExits(_connections[i], exitToMatch);
            }*/
        //}
    }
}
