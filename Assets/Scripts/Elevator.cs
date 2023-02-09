using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public GameObject DoorTrigger;
    public GameObject ElevatorPhysical;
    public GameObject Player;
    public Transform startPos;
    public Transform endPos;
    public string PlayerTag;
    public float speed;
    public bool atStart;
    public bool atEnd;
    public bool toStart;
    public bool toEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {

            DoorTrigger.GetComponent<AutomaticDoor>().toOpen = false;
            DoorTrigger.GetComponent<AutomaticDoor>().toClose = true;

            Player.transform.parent = gameObject.transform.parent;

            if (atStart)
            {
                atStart = false;
                atEnd = false;
                toStart = false;
                toEnd = true;
            }else if (atEnd)
            {
                atStart = false;
                atEnd = false;
                toStart = true;
                toEnd = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.parent != null)
        {
            Player.transform.parent = null;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(toEnd && DoorTrigger.GetComponent<AutomaticDoor>().toOpen == false && DoorTrigger.GetComponent<AutomaticDoor>().toClose == false)
        {
            //Player.transform.Translate((endPos.position - ElevatorPhysical.transform.position).normalized * speed * Time.fixedDeltaTime);
            ElevatorPhysical.transform.Translate((endPos.position - ElevatorPhysical.transform.position).normalized * speed * Time.fixedDeltaTime);
            //if (ElevatorPhysical.transform.position == endPos.position)
            if(Vector3.Distance(ElevatorPhysical.transform.position, endPos.position) <= 1)
            {
                ElevatorPhysical.transform.position = endPos.position;
                toEnd = false;
                atEnd = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toOpen = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toClose = false;
                DoorTrigger.GetComponent<AutomaticDoor>().resetPos();
            }
        }
        else if (toStart && DoorTrigger.GetComponent<AutomaticDoor>().toOpen == false && DoorTrigger.GetComponent<AutomaticDoor>().toClose == false)
        {
            //Player.transform.Translate((startPos.position - ElevatorPhysical.transform.position).normalized * speed * Time.fixedDeltaTime);
            ElevatorPhysical.transform.Translate((startPos.position - ElevatorPhysical.transform.position).normalized * speed * Time.fixedDeltaTime);
            if (Vector3.Distance(ElevatorPhysical.transform.position, startPos.position) <= 1)
            {
                ElevatorPhysical.transform.position = startPos.position;
                toStart = false;
                atStart = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toOpen = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toClose = false;
                DoorTrigger.GetComponent<AutomaticDoor>().resetPos();
            }
        }
    }
}
