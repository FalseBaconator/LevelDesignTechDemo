using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public GameObject Enemy;

    public float viewRange;

    public Material red;
    public Material green;

    //public LayerMask ignoreLayer;

    MeshRenderer rend;

    private void Start()
    {
        Enemy = transform.parent.gameObject;
        rend = GetComponent<MeshRenderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaycastHit hit;
            //Debug.DrawRay(Enemy.transform.position, other.transform.position - Enemy.transform.position);
            if(Physics.Raycast(Enemy.transform.position, other.GetComponent<PlayerControlsAddon>().Head.position - Enemy.transform.position, out hit))
            {
                //Debug.Log(hit.collider.gameObject);
                if (hit.collider.CompareTag("Player"))
                {
                    rend.material = red;
                }
                else
                {
                    rend.material = green;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<Renderer>().material = green;
        }
    }
}
