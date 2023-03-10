using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControlsAddon : MonoBehaviour
{

    public float crouchHeight;
    private float fullHeight;
    public GameObject throwable;
    public Transform throwFrom;
    public float throwForce;
    public Vector3 respawnPoint;
    public Transform Head;
    public TextMeshProUGUI canThrowText;

    public List<GameObject> thrownCans;
    public bool canThrow = false;
    // Start is called before the first frame update
    void Start()
    {
        fullHeight = gameObject.transform.localScale.y;
        respawnPoint = transform.position;
        canThrowText.text = "cannot throw";
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && canThrow)
        {
            canThrow = false;
            canThrowText.text = "cannot throw";
            GameObject thrown = GameObject.Instantiate(throwable, throwFrom);
            thrown.transform.parent = null;
            thrown.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * throwForce, ForceMode.Impulse);
            thrownCans.Add(thrown);
            if(thrownCans.Count > 10)
            {
                GameObject toDelete = thrownCans[0];
                thrownCans.RemoveAt(0);
                GameObject.Destroy(toDelete);
            }
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
