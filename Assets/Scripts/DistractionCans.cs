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
        enemies = GameObject.FindGameObjectsWithTag("Enemy");       //Can't turn an array into a list but need to use lists for can removal. Suggest using canManager GameObject.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
