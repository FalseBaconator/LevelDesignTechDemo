using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPoint : MonoBehaviour
{

    public GameObject collectible;
    public float timer;
    private float maxTime;
    public AudioClip clip;
    public AudioSource source;


    // Start is called before the first frame update
    void Start()
    {
        maxTime = timer;
        GameObject.Instantiate(collectible, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0 && gameObject.transform.childCount == 0)
        {
            timer = 0;
            GameObject.Instantiate(collectible, gameObject.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.transform.childCount > 0)
        {
            GameObject.Destroy(gameObject.transform.GetChild(0).gameObject);
            timer = maxTime;
            if (other.GetComponent<PlayerControlsAddon>().canThrow == false)
            {
                other.GetComponent<PlayerControlsAddon>().canThrow = true;
                other.GetComponent<PlayerControlsAddon>().canThrowText.text = "can throw";
                source.clip = clip;
                source.Play();
            }
        }
    }

}
