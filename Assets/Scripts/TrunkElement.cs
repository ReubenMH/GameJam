using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkElement : MonoBehaviour
{
    public Transform offsetTrunk;
    public BoxCollider col;

    public void SetActive(bool b)
    {
        col.enabled = b;
    }
}
