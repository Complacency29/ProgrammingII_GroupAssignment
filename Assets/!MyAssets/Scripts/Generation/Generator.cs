using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace ModuleSnapping
{
    public class Generator : SingletonMonoBehavior<Generator>
    {
        [SerializeField] private Tileset tileset; //Stores all modules that can be used in X area
        [SerializeField] private uint maxIterations; //Stores how many times the process can repeat, dictates map size1
        [SerializeField] private uint maxAttempts; //Stores max amount of times a module can try to be connected to another module
        [SerializeField] private bool inProgress = false; //Stores if we are already generating a map, will switch to false when we are finished
        [SerializeField] private bool lastRoomGenerated = false; //Stores if we have generated the last room
        [SerializeField] private GameObject block;

        [SerializeField] private bool clearingInProgress = false;

        private String lastModule = "";

        [SerializeField] private String startPointName;
        [SerializeField] private String endPointName;

        [SerializeField] List<Connection> pendingConnections = new List<Connection>();
        [SerializeField] List<Connection> allConnections = new List<Connection>();

        private void Start()
        {
            GenerateModules();
        }

        public void GenerateModules()
        {
            lastRoomGenerated = false;

            if (inProgress) //we're already generating a map, so don't make another one
                return;

            //if (clearingInProgress)
                //StartCoroutine(GenerateEnvironment());

            //ensure there is no other maps generated already
            if (transform.childCount == 0)
            {
                StartCoroutine(GenerateEnvironment());
            }
            else if(clearingInProgress == false)
            {
                clearingInProgress = true;
                ClearModules();
                //GenerateModules();
            }
        }

        private IEnumerator GenerateEnvironment()
        {
            inProgress = true; //set this to true first to prevent the program tripping over itself

            List<Module> loadedModules = new List<Module>(); //stores all modules that have been made and confirmed
            int startingModuleRNG = UnityEngine.Random.Range(0, tileset.startingModules.Length);

            //Spawn a new module of the randomly selected type
            //Then set it as a child of this transform
            GameObject startingModulePrefab = Instantiate(tileset.startingModules[startingModuleRNG]);
            startingModulePrefab.transform.parent = transform;

            //Get the starting module prefab, add it to the loadedModules list, get all connections assigned to that module
            //Save the list of pending connections so we can add other connections to this list later
            Module startingModule = startingModulePrefab.GetComponent<Module>();
            loadedModules.Add(startingModule);
            //List<Connection> pendingConnections = new List<Connection>(startingModule.GetConnections);
            pendingConnections = new List<Connection>(startingModule.GetConnections);


            // set our iterations index to 0 for the start
            int iteration = 0;

            while(iteration < maxIterations && lastRoomGenerated == false) //this code will loop until we hit the max iterations number
            {
                //This is where we will store the connections for the new modules that have been made
                List<Connection> newConnections = new List<Connection>();

                //Loop through all the connections that have been confirmed and are in the pendingConnections list
                for (int exitIndex = 0; exitIndex < pendingConnections.Count; exitIndex++)
                {
                    //Sanity check to make sure we are not using an empty value
                    if (pendingConnections[exitIndex] == null || pendingConnections[exitIndex].isActiveAndEnabled == false)
                    {
                        continue;
                    }
                    //Loops until there is a successful module created and connected, OR we reach our maximum attempts
                    for (int curAttempt = 0; curAttempt < maxAttempts; curAttempt++)
                    {
                        //This spawns a random module of a valid type
                        //Put into position with correct rotation
                        //If it fits, it sits
                        //If it doesn't fit, try again, that is one attempt.
                        ModuleType validType = GetRandom(pendingConnections[exitIndex].GetValidConnections);
                        GameObject newModulePrefab = GetModulePrefabOfType(validType, iteration, lastModule);

                        lastModule = newModulePrefab.ToString();
                        //Debug.Log(newModulePrefab.ToString());

                        GameObject prefab = Instantiate(newModulePrefab);
                        prefab.transform.parent = transform;
                        Module newModule = prefab.GetComponent<Module>();
                        Connection[] newModuleConnections = newModule.GetConnections;
                        Connection exitToMatch = newModuleConnections.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleConnections);
                        MatchExits(pendingConnections[exitIndex], exitToMatch);
                        newConnections.AddRange(newModuleConnections.Where(e => e != exitToMatch));

                        //Will store if a collision is found
                        bool collisionFound = false;

                        //Wait a frame to ensure module is in position
                        
                        yield return new WaitForSeconds(0.1f);          // SLOW GEN
                        //yield return null;                              // FAST GEN
                        
                        //Check if there has been a collision
                        //If so, remove the new module and try again
                        if(newModule.GetIgnoreCollision == false)
                        {
                            collisionFound = newModule.CollisionCheck();
                            if(collisionFound == true)
                            {
                                Destroy(newModule.gameObject);
                                break;
                            }
                        }

                        //Sanity check, should not be needed, but makes sure that no collision took place
                        if (collisionFound == true)
                        {
                            Debug.Log("Removed odd element.");
                            Destroy(newModule.gameObject);
                        }
                        else
                        {
                            //the new module fits with no issues, so turn off the connections and add the new module to the loadedModules list
                            pendingConnections[exitIndex].gameObject.SetActive(false); //turns off the connection point so it cannot be used again
                            exitToMatch.gameObject.SetActive(false);
                            loadedModules.Add(newModule);

                            //Debug.Log(loadedModules[loadedModules.Count - 1].ToString());

                            if (loadedModules[loadedModules.Count - 1].ToString() == $"{endPointName}(Clone) (ModuleSnapping.Module)")
                            {
                                lastRoomGenerated = true;

                            }

                            /*if (loadedModules[loadedModules.Count - 1].ToString() == "Module Room 2(Clone) (ModuleSnapping.Module)")
                            {
                                lastRoomGenerated = true;
                            }*/

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
                    Instantiate(block, con.transform);
                    //Debug.Log($"Block generated at ({con.transform.position.x}, {con.transform.position.y}, {con.transform.position.z})");
                }
            }

            inProgress = false;

            //yield return new WaitForSeconds(0.1f);

            if(lastRoomGenerated == false)
            {
                //ClearModules();
                ClearModules();
                
            }
        }

        /// <summary>
        /// Get both connection points and put them at the same position
        /// Rotate the new connection to be the mirror of the old connection
        /// </summary>
        /// <param name="_oldExit">The original exit that we will not rotate</param>
        /// <param name="_newExit">The new exit which will be rotated</param>
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

            List<GameObject> validChoices = new List<GameObject>();

            for (int i = 0; i < tileset.allModules.Length; i++)
            {
                GameObject curModuleObject = tileset.allModules[i];
                Module curModule = curModuleObject.GetComponent<Module>();

                for (int x = 0; x < curModule.GetModuleTypes.Length; x++)
                {
                    if (curModule.GetModuleTypes[x] == validType && curModule.ToString() != endPointName)
                    {
                        validChoices.Add(curModuleObject);
                    }
                }
            }

            if((iteration == maxIterations - 1) && lastModule == "Hallway (UnityEngine.GameObject)")
            {
                //lastRoomGenerated = true;

                return tileset.endModules[UnityEngine.Random.Range(0, tileset.endModules.Length - 1)];
                //return tileset.allModules[tileset.allModules.Length - 1];
                
                //return validChoices[validChoices.Count - 1];
            }
            else
            {
                //Debug.Log(lastModule.ToString());
                return validChoices[UnityEngine.Random.Range(0, validChoices.Count - 1)];
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
            //if in progress or there is already no children
            if(inProgress || transform.childCount == 0)
            {
                GenerateModules();
                return;
            }
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }
            clearingInProgress = false;
            StartCoroutine(GenerateEnvironment());
        }
    }
}
