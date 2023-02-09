using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionCans : MonoBehaviour
{

    private GameObject[] enemies;
    private List<GameObject> cans;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") == false && collision.gameObject.CompareTag("Enemy") == false)
        {
            foreach (GameObject enemy in enemies)
            {
                if (Vector3.Distance(enemy.transform.position, gameObject.transform.position) <= range)
                {
                    enemy.transform.LookAt(gameObject.transform.position, Vector3.up);
                }
            }
        }else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
    }
}
