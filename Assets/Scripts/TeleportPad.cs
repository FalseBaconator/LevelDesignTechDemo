using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    public GameObject targetObj;

    public static bool canTeleport = true;

    Vector3 targetPos;

    public AudioClip clip;
    public AudioSource source;

    private void Start()
    {
        targetPos = targetObj.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport == true)
        {
            other.GetComponent<PlayerControlsAddon>().Teleport(targetPos);
            canTeleport = false;
            source.clip = clip;
            source.Play();
            targetObj.GetComponent<AudioSource>().clip = clip;
            targetObj.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport == false)
        {
            canTeleport = true;
        }
    }

}
