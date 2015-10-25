using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GameObject target;
	
	void LateUpdate () 
	{
		if (target != null)
		{
			MoveCamera ();
		}
	}

	void MoveCamera ()
	{
		var height = 20;
		var distance = 30;
		
		var rotationDamping = 5;
		var heightDamping = 1;
		var wantedRotationAngle = target.transform.eulerAngles.y - 180;
		var wantedHeight = target.transform.position.y + height;
		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;

		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		transform.position = target.transform.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);
		transform.LookAt (target.transform);
	}
}
