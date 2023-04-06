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

    public enum Actor
    {
        RWohler,
        RMartel
    }

    public Actor actor;

    public AudioClip[] RWDistracts; //Wohler Lines
    public AudioClip[] RWSearches;
    public AudioClip[] RWSpots;
    public AudioClip[] RWRetreats;
    public AudioClip[] RMDistracts; //Martel Lines
    public AudioClip[] RMSearches;
    public AudioClip[] RMSpots;
    public AudioClip[] RMRetreats;

    public AudioSource speaker;

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

    //Code that runs whenever the state changes | Changes flashlight color to match state
    public StateType State{ get => state; set {
            if (state == StateType.Patrolling)
            {
                lastPatrolSpot = transform.position;//Saves where the enemy stopped patrolling
            }
            switch (value)
            {
                case StateType.Chasing:
                    if (actor == EnemyMove.Actor.RWohler) speaker.clip = RWSpots[Random.Range(0, RWSpots.Length)];
                    else if (actor == EnemyMove.Actor.RMartel) speaker.clip = RMSpots[Random.Range(0, RMSpots.Length)];
                    speaker.Play();
                    flashlight.color = Color.red;
                    break;
                case StateType.Distracted:
                    //Stops moving and starts timer
                    if (actor == EnemyMove.Actor.RWohler) speaker.clip = RWDistracts[Random.Range(0, RWDistracts.Length)];
                    else if (actor == EnemyMove.Actor.RMartel) speaker.clip = RMDistracts[Random.Range(0, RMDistracts.Length)];
                    speaker.Play();
                    flashlight.color = Color.green;
                    agent.ResetPath();
                    timer = distractedTime;
                    break;
                case StateType.Searching:
                    //Saves the angle they started at and starts timer
                    if (actor == EnemyMove.Actor.RWohler) speaker.clip = RWSearches[Random.Range(0, RWSearches.Length)];
                    else if (actor == EnemyMove.Actor.RMartel) speaker.clip = RMSearches[Random.Range(0, RMSearches.Length)];
                    speaker.Play();
                    startAngle = transform.rotation.eulerAngles.y;
                    timer = searchTime;
                    flashlight.color = Color.yellow;
                    break;
                case StateType.Patrolling:
                    //Sets the destination they were following when they stopped
                    agent.SetDestination(targets[targetIndex].transform.position);
                    flashlight.color = Color.white;
                    break;
                case StateType.Retreating:
                    //Go back to where they stopped patrolling
                    if (actor == EnemyMove.Actor.RWohler) speaker.clip = RWRetreats[Random.Range(0, RWRetreats.Length)];
                    else if (actor == EnemyMove.Actor.RMartel) speaker.clip = RMRetreats[Random.Range(0, RMRetreats.Length)];
                    speaker.Play();
                    agent.SetDestination(lastPatrolSpot);
                    flashlight.color = Color.cyan;
                    break;
            }
            state = value;
        }
    }

    private void Start()
    {
        //Setup
        player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(targets[targetIndex].transform.position);
        targetParent.transform.parent = null;   //Prevents the patrol points from moving with the enemy
    }

    // Update is called once per frame
    void Update()
    {
        switch (State) 
        {
            case StateType.Patrolling:
                if (Vector3.Distance(transform.position, targets[targetIndex].transform.position) <= distFromTarget)
                {
                    //If the enemy reaches their target point, go to next target. Go back to first target if at last target
                    targetIndex++;
                    if (targetIndex >= targets.Length)
                        targetIndex = 0;
                    agent.SetDestination(targets[targetIndex].transform.position);
                }
                //Change state if applicable
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Chasing:
                RaycastHit hit;
                if (Physics.Raycast(flashlight.transform.position, player.GetComponent<PlayerControlsAddon>().Head.position - transform.position, out hit))
                {
                    //Chases after player until line of sight with player is lost
                    if (hit.collider.CompareTag("PlayerBody"))
                        agent.SetDestination(player.transform.position);
                    else
                        State = StateType.StartSearching;
                }
                break;
            case StateType.StartSearching:
                //The target stays the same as from the chasing state
                if (Vector3.Distance(transform.position, agent.destination) <= distFromTarget)
                    //start looking around if reaching where the player was last seen
                    State = StateType.Searching;
                //change states if appropriate
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Searching:
                timer -= Time.deltaTime;
                float num = transform.rotation.eulerAngles.y - startAngle;  //current angle from start angle
                
                //retreat if they lose patience
                if (timer <= 0)
                {
                    timer = 0;
                    State = StateType.Retreating;
                }

                //Rotate in the appropriate direction
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

                //change states if appropriate
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Distracted:
                timer -= Time.deltaTime;
                //Look at the direction the "noise" came from
                transform.LookAt(distractSpot);
                transform.Rotate(-transform.rotation.eulerAngles.x, 0, 0); //Keeps upright
                //Change states when appropriate
                if (timer <= 0)
                    State = StateType.Retreating;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
            case StateType.Retreating:
                //Go back to where they stopped patrolling, and change state when appropriate
                if (Vector3.Distance(transform.position, agent.destination) <= distFromTarget)
                    State = StateType.Patrolling;
                if (toDistract)
                    State = StateType.Distracted;
                if (CheckVision())
                    State = StateType.Chasing;
                break;
        }

        //toDistract is set to false so that the state only occurs once
        if(toDistract)
            toDistract = false;
    }

    //Gets called by DistractionCans
    public void DistractAtLocation(Vector3 loc)
    {
        //Saves location of impact. Sets toDistract to true if the location is close enough
        distractSpot = loc;
        if (Vector3.Distance(distractSpot, transform.position) <= hearingDist)
            toDistract = true;
    }

    bool CheckVision()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= visionDist)   //Checks if player is within distance
        {
            if(Mathf.Abs(Vector3.Angle(player.transform.position - transform.position, transform.forward)) <= visionAngle)  //Checks if player is within angle
            {
                RaycastHit hit;
                if (Physics.Raycast(flashlight.transform.position, player.GetComponent<PlayerControlsAddon>().Head.position - transform.position, out hit)) //Checks if player is blocked from view
                {
                    if (hit.collider.CompareTag("PlayerBody"))
                    {
                        return true;    //Player is visible
                    }
                }
            }
        }

        return false;   //Player is not visible
    }


}
