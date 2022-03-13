using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

	float timer;
	public float maxTime = 5f;
	
    void Update()
    {
		timer += Time.deltaTime;
		if(timer > maxTime) {
			Destroy(gameObject);
		}
    }
}
