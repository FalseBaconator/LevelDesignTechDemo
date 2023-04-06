using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionCans : MonoBehaviour
{

    private GameObject[] enemies;
    //private List<GameObject> cans;
    public float range;
    public AudioClip[] noises;
    public AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.clip = noises[Random.Range(0, noises.Length)];
        source.Play();
        if (collision.gameObject.CompareTag("Player") == false && collision.gameObject.CompareTag("Enemy") == false)    //If it hits a wall or furniture
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyMove>().DistractAtLocation(gameObject.transform.position);  //Ask all enemies to check if they heard the noise
            }
        }else if (collision.gameObject.CompareTag("Enemy")) //If it hits an enemy
        {
            collision.gameObject.transform.LookAt(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlsAddon>().Head.position);    //Enemy turns to look at player
        }
    }
}
