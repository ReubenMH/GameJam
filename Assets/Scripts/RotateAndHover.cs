using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndHover : MonoBehaviour
{
	float timer = 0;
	Vector3 localStartPosition;

    // Start is called before the first frame update
    void Start()
    {
		localStartPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

		timer += Time.deltaTime;

		Vector3 eulerAngles = transform.localRotation.eulerAngles;
		eulerAngles.y += 90f * Time.deltaTime;

		transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);

		transform.localPosition = localStartPosition + Vector3.up * Mathf.Sin(timer * 5f) * 0.25f;

    }
}
