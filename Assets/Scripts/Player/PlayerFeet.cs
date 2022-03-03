using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    List<GameObject> groundObjects;

    public bool IsGrounded => groundObjects.Count > 0;

    private void Awake()
    {
        groundObjects = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Tree"))
        {
            groundObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Tree"))
        {
            groundObjects.Remove(other.gameObject);
        }
    }
}
