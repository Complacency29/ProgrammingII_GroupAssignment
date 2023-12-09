using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuleSnapping;
using UnityEngine.AI;
using Photon.Pun;

namespace EnemyStuff
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public float maxHealth = 100;
        private float curHealth;
        private Rigidbody RB;

        public EnemyFSM stateMachine { get; set; }
        public StateWander wanderState { get; set; }
        public StateSearch searchState { get; set; }
        public StateChase chaseState { get; set; }
        public StateAttack attackState { get; set; }

        private PhotonView view;

        //private List<Module> moduleList;
        private Module[] moduleList;

        private void Awake()
        {
            attack = GetComponentInChildren<SphereCollider>();
            attack.gameObject.SetActive(false);

            stateMachine = new EnemyFSM();

            wanderState = new StateWander(this, stateMachine);
            searchState = new StateSearch(this, stateMachine);
            chaseState = new StateChase(this, stateMachine);
            attackState = new StateAttack(this, stateMachine);

            view = GetComponent<PhotonView>();
            curHealth = maxHealth;
        }

        float time;

        private void FixedUpdate()
        {
            time += Time.deltaTime;

            if(time >= 10f && stateMachine.CurEnemyState == wanderState)
            {
                time = 0;
                hasPath = false;
            }
        }

        void Start()
        {
            curHealth = maxHealth;
            RB = GetComponent<Rigidbody>();

            stateMachine.Initialize(wanderState);

            moduleList = FindObjectsOfType<Module>();

            /*foreach(Module mod in moduleList)
            {
                if(mod.GetModuleTypes[0] == ModuleType.GroundProp)
                {
                    if(mod != null)
                    {
                       
                    }
                }
            }*/
        }

        public void Damage(int _amount)
        {
            view.RPC("DamageRPC", RpcTarget.All, _amount);
        }

        [PunRPC]
        public void DamageRPC(int _amount)
        {
            curHealth -= _amount;
        }

        public void Die()
        {
            Photon.Pun.PhotonNetwork.Destroy(gameObject);
        }

        [SerializeField] Vector3 newPosition;
        [SerializeField] Vector3 lastKnownPos;
        [SerializeField] GameObject targetPlayer;
        bool hasPath = false;
        Collider attack;

        public void Update()
        {
            if(PhotonNetwork.IsMasterClient == false)
            {
                return;
            }

            if(curHealth <= 0)
            {
                Die();
            }

            if(stateMachine.CurEnemyState == wanderState)
            {
                if(hasPath == false)
                {
                    hasPath = true;

                    // Get all the modules
                    //Module[] moduleList = FindObjectsOfType<Module>();

                    // Generate a random number within the array length
                    int random = Random.Range(0, moduleList.Length);
                    Debug.Log($"module to go to: {random}");

                    // Set the new position to the random module's position
                    if(moduleList[random].GetModuleTypes[0] != ModuleType.GroundProp)
                    {
                        newPosition = moduleList[random].gameObject.transform.position;
                    }

                    // Tell agent to go
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    
                    if(agent.isOnNavMesh)
                    {
                        agent.SetDestination(newPosition);
                    }
                }
                else if(Vector3.Distance(gameObject.transform.position, newPosition) <= 3 && hasPath == true)
                {
                    hasPath = false;
                }
            }

            if(stateMachine.CurEnemyState == chaseState)
            {
                if(lastKnownPos != null)
                {
                    hasPath = true;
                    GetComponent<NavMeshAgent>().SetDestination(lastKnownPos);
                }

                if (Vector3.Distance(transform.position, lastKnownPos) < 2f)
                {
                    hasPath = false;

                    if (Vector3.Distance(transform.position, targetPlayer.transform.position) < 2f)
                    {
                        stateMachine.ChangeState(attackState);

                        GetComponent<NavMeshAgent>().isStopped = true;

                        Attack();
                    }
                    else
                    {
                        CheckPlayerWithRay();
                    }
                }
            }
        }

        private void Attack()
        {
            view.RPC("RPCAttack", RpcTarget.All);
        }

        [PunRPC]
        private void RPCAttack()
        {
            StartCoroutine(RPCAttackCO());
        }

        private IEnumerator RPCAttackCO()
        {
            Debug.Log("Attacking for real");

            yield return new WaitForSecondsRealtime(0.5f);

            attack.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(1f);

            attack.gameObject.SetActive(false);
            stateMachine.ChangeState(chaseState);
            GetComponent<NavMeshAgent>().isStopped = false;

            yield break;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (view.IsMine == false)
            {
                return;
            }

            if (other.tag == "Player")
            {
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(Vector3.Normalize(other.transform.position - transform.position).x, Vector3.Normalize(other.transform.position - transform.position).y, Vector3.Normalize(other.transform.position - transform.position).z), Color.red);
                Debug.Log("ray test");

                targetPlayer = other.gameObject;

                CheckPlayerWithRay();
            }
        }

        private void CheckPlayerWithRay()
        {
            if (PhotonNetwork.IsMasterClient == false)
            {
                return;
            }

            if (targetPlayer == null)
            {
                Debug.Log("target player null");
                return;
            }

            RaycastHit hit;

            Vector3 targetVector = Vector3.Normalize(new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 1, targetPlayer.transform.position.z) - transform.position);

            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetVector, out hit))
            {
                Debug.Log($"hit tag: {hit.transform.gameObject.tag}, object: {hit.transform.gameObject.name}");

                if (hit.transform.gameObject.tag == "Player")
                {
                    Debug.Log("ray success");

                    targetPlayer = hit.transform.gameObject;
                    lastKnownPos = targetPlayer.transform.position;

                    if (stateMachine.CurEnemyState != chaseState && stateMachine.CurEnemyState != attackState)
                    {
                        stateMachine.ChangeState(chaseState);
                    }
                }
                else
                {
                    hasPath = false;
                    stateMachine.ChangeState(wanderState);
                }
            }
            else
            {
                hasPath = false;
                stateMachine.ChangeState(wanderState);
            }
        }
    }
}

