using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionCans : MonoBehaviour
{

    private GameObject[] enemies;
    private List<GameObject> cans;
    public float range;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") == false && collision.gameObject.CompareTag("Enemy") == false)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyMove>().DistractAtLocation(gameObject.transform.position);
            }
        }else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.LookAt(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlsAddon>().Head.position);
        }
    }
}
