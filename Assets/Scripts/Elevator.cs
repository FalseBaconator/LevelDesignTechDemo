using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public GameObject DoorTrigger;
    public GameObject ElevatorPhysical;
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
        if (other.CompareTag(PlayerTag))
        {

            DoorTrigger.GetComponent<AutomaticDoor>().toOpen = false;
            DoorTrigger.GetComponent<AutomaticDoor>().toClose = true;

            other.transform.parent = gameObject.transform;

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
        if(other.transform.parent == gameObject.transform)
        {
            other.transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(toEnd && DoorTrigger.GetComponent<AutomaticDoor>().toOpen == false && DoorTrigger.GetComponent<AutomaticDoor>().toClose == false)
        {
            Debug.Log("Test");
            
            ElevatorPhysical.transform.Translate((endPos.position - ElevatorPhysical.transform.position).normalized * speed * Time.deltaTime);
            if (ElevatorPhysical.transform.position == endPos.position)
            {
                toEnd = false;
                atEnd = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toOpen = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toClose = false;
                DoorTrigger.GetComponent<AutomaticDoor>().resetPos();
            }
        }
        else if (toStart && DoorTrigger.GetComponent<AutomaticDoor>().toOpen == false && DoorTrigger.GetComponent<AutomaticDoor>().toClose == false)
        {
            Debug.Log("Test2");
            
            ElevatorPhysical.transform.Translate((startPos.position - ElevatorPhysical.transform.position).normalized * speed * Time.deltaTime);
            if (ElevatorPhysical.transform.position == startPos.position)
            {
                toStart = false;
                atStart = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toOpen = true;
                DoorTrigger.GetComponent<AutomaticDoor>().toClose = false;
                DoorTrigger.GetComponent<AutomaticDoor>().resetPos();
            }
        }
    }
}
