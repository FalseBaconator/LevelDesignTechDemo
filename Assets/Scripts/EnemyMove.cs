using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{

    public bool toDebug;

    public enum StateType
    {
        Patrolling,
        Chasing,
        StartSearching,
        Searching,
        Distracted,
        Retreating
    }

    enum RotateDir
    {
        Right,
        Left
    }

    StateType state = StateType.Patrolling;

    RotateDir rDir = RotateDir.Left;

    public NavMeshAgent agent;

    GameObject player;

    public GameObject targetParent;

    public GameObject[] targets;

    int targetIndex = 0;

    public float distFromTarget;

    public float searchAngle;

    public float startAngle;

    public float rotateSpeed;

    public float distractedTime;

    public float searchTime;

    public float timer;

    public float visionDist;

    public float visionAngle;

    public Vector3 lastPatrolSpot;

    public Light light;

    public StateType State{ get => state; set {
            if(state == StateType.Patrolling)
            {
                lastPatrolSpot = transform.position;
            }
            if(value == StateType.Chasing)
            {
                light.color = Color.red;
            }
            if(value == StateType.Distracted)
            {
                timer = distractedTime;
                light.color = Color.white;
            }
            if(value == StateType.Searching)
            {
                startAngle = transform.rotation.eulerAngles.y;
                timer = searchTime;
                light.color = Color.red;
            }
            if(value == StateType.Patrolling)
            {
                agent.SetDestination(targets[targetIndex].transform.position);
                light.color = Color.white;
            }
            if(value == StateType.Retreating)
            {
                agent.SetDestination(lastPatrolSpot);
                light.color = Color.white;
            }
            state = value;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(targets[targetIndex].transform.position);
        targetParent.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        //if (toDebug) Debug.Log(Vector3.Angle(player.transform.position - transform.position, transform.forward));

        //if (toDebug) Debug.Log(State);

        if (State == StateType.Patrolling)
        {
            if (Vector3.Distance(transform.position, targets[targetIndex].transform.position) <= distFromTarget)
            {
                targetIndex++;
                if (targetIndex >= targets.Length)
                {
                    targetIndex = 0;
                }
                agent.SetDestination(targets[targetIndex].transform.position);
            }
            if (CheckVision())
            {
                State = StateType.Chasing;
            }
        }
        else if (State == StateType.Chasing)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.GetComponent<PlayerControlsAddon>().Head.position - transform.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    State = StateType.StartSearching;
                }
            }            
        }
        else if (State == StateType.StartSearching)
        {
            if (Vector3.Distance(transform.position, agent.destination) <= distFromTarget)
                State = StateType.Searching;
            if (CheckVision())
            {
                State = StateType.Chasing;
            }
        }
        else if (State == StateType.Searching)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                State = StateType.Retreating;
            }
            if (rDir == RotateDir.Left)
            {
                transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
                if (360 - transform.rotation.eulerAngles.y - startAngle >= searchAngle)
                {
                    rDir = RotateDir.Right;
                }
            }
            else if (rDir == RotateDir.Right)
            {
                transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
                if (transform.rotation.eulerAngles.y - startAngle >= searchAngle)
                {
                    rDir = RotateDir.Left;
                }
            }
            if (CheckVision())
            {
                State = StateType.Chasing;
            }
        }
        else if (State == StateType.Distracted)
        {
            timer -= Time.deltaTime;
            agent.ResetPath();
            if (timer <= 0)
            {
                State = StateType.Retreating;
            }
        }
        else if (State == StateType.Retreating)
        {
            if (Vector3.Distance(transform.position, agent.destination) <= distFromTarget)
            {
                State = StateType.Patrolling;
            }
            if (CheckVision())
            {
                State = StateType.Chasing;
            }
        }
    }


    bool CheckVision()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= visionDist)
        {
            if(Mathf.Abs(Vector3.Angle(player.transform.position - transform.position, transform.forward)) <= visionAngle)
            {
                
                RaycastHit hit;
                if (Physics.Raycast(transform.position, player.GetComponent<PlayerControlsAddon>().Head.position - transform.position, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


}
