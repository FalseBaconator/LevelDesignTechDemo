using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public GameObject Enemy;

    public float viewRange;

    public Material red;
    public Material green;

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
            if(Physics.Raycast(Enemy.transform.position, other.GetComponent<PlayerControlsAddon>().Head.position - Enemy.transform.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    rend.material = red;
                    Enemy.GetComponent<EnemyMove>().State = EnemyMove.StateType.Chasing;
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
