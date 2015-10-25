using UnityEngine;
using System.Collections;

public class CarController : Photon.MonoBehaviour 
{
	public int speed = 50;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	private Quaternion syncEndRotation;
	private Quaternion syncStartRotation;

	public void Start()
	{
		if (photonView.isMine)
		{
			SetupCamera ();
		}
	}

	void SetupCamera ()
	{
		GameObject camera = GameObject.FindWithTag ("MainCamera");
		if (camera != null) 
		{
			CameraController followScript = camera.GetComponent ("CameraController") as CameraController;
			if (followScript != null) 	
			{
				followScript.target = gameObject;
			}
		}
	}

	void Update () 
	{
		if (photonView.isMine)
		{
			GetInputForPlayer ();
		}
		else
		{
			SyncPlayerTransform ();
		}
	}

	void SyncPlayerTransform ()
	{
		syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime / syncDelay);
		transform.rotation = Quaternion.Slerp (syncStartRotation, syncEndRotation, syncTime / syncDelay);
	}

	void GetInputForPlayer ()
	{
		if (Input.GetKey ("s")) 
		{
			MoveBackward();
		}
		if (Input.GetKey ("w")) 
		{
			MoveForward();
		}

		if (Input.GetKey ("d")) 
		{
			MoveRight();
		}
		if (Input.GetKey ("a")) 
		{
			MoveLeft();
		}
	}

	public void MoveForward()
	{
		transform.Translate (new Vector3 (0, 0, -3) * speed * Time.deltaTime);
	}

	public void MoveBackward()
	{
		transform.Translate (new Vector3 (0, 0, 3) * speed * Time.deltaTime);
	}

	public void MoveLeft()
	{
		transform.Rotate(0, -60 * Time.deltaTime, 0, Space.World);
	}

	public void MoveRight()
	{
		transform.Rotate(0, 60 * Time.deltaTime, 0, Space.World);
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			syncEndPosition = (Vector3)stream.ReceiveNext();
			syncStartPosition = transform.position;
			
			syncEndRotation = (Quaternion)stream.ReceiveNext();
			syncStartRotation = transform.rotation;
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
		}
	}
}
