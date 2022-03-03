using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private EnemyController enemyCont;
    [SerializeField] private int zoneID;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            enemyCont.StartSpawning(zoneID);
        }
    }
}
