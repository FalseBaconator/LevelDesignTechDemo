using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsAddon : MonoBehaviour
{

    public float crouchHeight;
    private float fullHeight;

    // Start is called before the first frame update
    void Start()
    {
        fullHeight = gameObject.transform.localScale.y;
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
    }
}
