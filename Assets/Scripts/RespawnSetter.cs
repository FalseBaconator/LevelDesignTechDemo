using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControlsAddon>().SetRespawn(transform.position);
        }
    }
}
