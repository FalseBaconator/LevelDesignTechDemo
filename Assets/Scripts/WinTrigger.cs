using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public string playerTag;

    public int sceneIndex;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == playerTag)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

}
