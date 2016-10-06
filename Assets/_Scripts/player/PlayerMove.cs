using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {

	public float movementSpeed = 1.0f;
	private Camera playerCamera;
	private Rigidbody playerRigidBody;

	void Start()
	{
		playerCamera = this.GetComponentInChildren<Camera> ();
		playerRigidBody = this.GetComponent<Rigidbody> ();
	}

	void Update()
	{
		if (GameState.CAN_PLAYER_MOVE) {

			float zMove = Input.GetAxis ("Vertical");
//			Vector3 moveForward = new Vector3(0.0f, 0.0f, zMove);
			Vector3 cameraForward = playerCamera.transform.forward;
			cameraForward.y = 0;
			this.transform.position += cameraForward * zMove * movementSpeed;
		}
	}
}