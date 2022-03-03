using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killbox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
