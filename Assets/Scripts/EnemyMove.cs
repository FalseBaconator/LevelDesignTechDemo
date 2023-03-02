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

    public float hearingDist;

    public Vector3 lastPatrolSpot;

    public Light flashlight;

    private Vector3 distractSpot;

    private bool toDistract;

    public StateType State{ get => state; set {
            if(state == StateType.Patrolling)
            {
                lastPatrolSpot = transform.position;
            }
            if(value == StateType.Chasing)
            {
                flashlight.color = Color.red;
            }
            if(value == StateType.Distracted)
            {
                transform.LookAt(distractSpot);
                transform.Rotate(-transform.rotation.eulerAngles.x, 0, 0);
                agent.ResetPath();
                timer = distractedTime;
            }
            if(value == StateType.Searching)
            {
                startAngle = transform.rotation.eulerAngles.y;
                timer = searchTime;
                flashlight.color = Color.red;
            }
            if(value == StateType.Patrolling)
            {
                agent.SetDestination(targets[targetIndex].transform.position);
                flashlight.color = Color.white;
            }
            if(value == StateType.Retreating)
            {
                agent.SetDestination(lastPatrolSpot);
                flashlight.color = Color.white;
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
        switch (State) 
        {
            case StateType.Patrolling:
                if (Vector3.Distance(transform.position, targets[targetIndex].transform.position) <= distFromTarget)
                {
                    targetIndex++;
                    if (targetIndex >= targets.Length)
                        targetIndex = 0;
                    agent.SetDestination(targets[targetIndex].transform.position);
                }
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Chasing:
                RaycastHit hit;
                if (Physics.Raycast(flashlight.transform.position, player.GetComponent<PlayerControlsAddon>().Head.position - transform.position, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                        agent.SetDestination(player.transform.position);
                    else
                        State = StateType.StartSearching;
                }
                break;
            case StateType.StartSearching:
                if (Vector3.Distance(transform.position, agent.destination) <= distFromTarget)
                    State = StateType.Searching;
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Searching:
                timer -= Time.deltaTime;
                float num = transform.rotation.eulerAngles.y - startAngle;
                if (timer <= 0)
                {
                    timer = 0;
                    State = StateType.Retreating;
                }
                if (rDir == RotateDir.Left)
                {
                    transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
                    
                    if (num > searchAngle * 2)
                        num -= 360;
                    if (num < -searchAngle * 2)
                        num += 360;
                    if (toDebug)
                        Debug.Log(num + " Left");
                    if (num <= -searchAngle)
                        rDir = RotateDir.Right;
                }
                else if (rDir == RotateDir.Right)
                {
                    transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
                    if (num > searchAngle * 2)
                        num -= 360;
                    if (num < -searchAngle * 2)
                        num += 360;
                    if (toDebug)
                        Debug.Log(num + " Right");
                    if (num >= searchAngle)
                        rDir = RotateDir.Left;
                }
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Distracted:
                timer -= Time.deltaTime;
                if (timer <= 0)
                    State = StateType.Retreating;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Retreating:
                if (Vector3.Distance(transform.position, agent.destination) <= distFromTarget)
                    State = StateType.Patrolling;
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
        }

        if(toDistract)
            toDistract = false;
    }

    public void DistractAtLocation(Vector3 loc)
    {
        distractSpot = loc;
        if (Vector3.Distance(distractSpot, transform.position) <= hearingDist)
            toDistract = true;
    }

    bool CheckVision()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= visionDist)
        {
            if(Mathf.Abs(Vector3.Angle(player.transform.position - transform.position, transform.forward)) <= visionAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(flashlight.transform.position, player.GetComponent<PlayerControlsAddon>().Head.position - transform.position, out hit))
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
