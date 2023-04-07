using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EnemyMove[] enemies;
    public Elevator[] elevators;

    // Start is called before the first frame update
    void Start()
    {
        elevators = GameObject.FindObjectsOfType<Elevator>();
        enemies = GameObject.FindObjectsOfType<EnemyMove>();
    }
    
    public void Restart()
    {
        foreach(Elevator elevator in elevators)
        {
            elevator.Restart();
        }
        foreach (EnemyMove enemy in enemies)
        {
            enemy.Restart();
        }
    }

}
