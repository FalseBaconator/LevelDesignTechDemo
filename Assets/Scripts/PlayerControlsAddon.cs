using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsAddon : MonoBehaviour
{

    public float crouchHeight;
    private float fullHeight;
    public GameObject throwable;
    public Transform throwFrom;
    public float throwForce;
    public Vector3 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        fullHeight = gameObject.transform.localScale.y;
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            gameObject.transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            gameObject.transform.localScale = new Vector3(transform.localScale.x, fullHeight, transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject thrown = GameObject.Instantiate(throwable, throwFrom);
            thrown.transform.parent = null;
            thrown.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * throwForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Kill();
        }

    }

    public void SetRespawn(Vector3 point)
    {
        respawnPoint = point;
    }

    public void Teleport(Vector3 point)
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = point;
        gameObject.GetComponent<CharacterController>().enabled = true; 
    }

    public void Kill()
    {
        Teleport(respawnPoint);
    }

}
