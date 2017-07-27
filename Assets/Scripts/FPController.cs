using Boo.Lang;
using UnityEngine;

namespace Assets.Scripts
{

	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(FPController))]
	public class FPController : MonoBehaviour
	{

		private float movFwd, movHor, movRot, lookRot;
		private Vector3 currentLookRot = Vector3.zero;
		private CharacterController controller;
		private Vector3 movePlayer = Vector3.zero;

		[Header("Player Speed Settings")]
		[SerializeField]
		private float fwdSpeed = 0f;
		[SerializeField]
		private float horSpeed = 0f;
		[SerializeField]
		private float jumpSpeed = 5f;

		[Space(2)]
		[Header("Mouse Sensitivity Settings")]
		[SerializeField]
		private float mouseSensitivityX = 10f;
		[SerializeField]
		private float mouseSensitivityY = 10f;

		[Space(2)]
		[Header("Vertical look Settings")]
		[SerializeField]
		private float maxLookUP = 65f;
		[SerializeField]
		private float maxLookDown = -65f;
		[Space(25)]

		private GameObject playerCamera;

		// Use this for initialization
		void Start ()
		{
			Cursor.lockState = CursorLockMode.Locked;
			controller = GetComponent<CharacterController>();
			playerCamera = GameObject.FindGameObjectWithTag("FPCamera");
		}
	
		// Update is called once per frame
		void Update ()
		{

			#region Move the players head up and down

			// Camera Rotation - we move the camera up and down, to allow the player to look up or down.
			// Remember this is the incremental rotation and not the final position of the camera
			// Note that the camera starts the scene looking forward and level

			lookRot = Input.GetAxis("Mouse Y") * mouseSensitivityY;
			currentLookRot.x = currentLookRot.x - lookRot;
			currentLookRot.x = Mathf.Clamp(currentLookRot.x, maxLookDown, maxLookUP);

			playerCamera.transform.localRotation = Quaternion.Euler(currentLookRot.x, 0f, 0f);

		   #endregion

		 #region Using the game players input Move the Player in the game

			//Player Rotation - we rotate  the transform of the player
			movRot = Input.GetAxis("Mouse X") * mouseSensitivityX;
			transform.Rotate(0, movRot, 0);

			// Move the player forward or backward
			movFwd = Input.GetAxis("Vertical") * fwdSpeed ;
			movePlayer.z = movFwd;

			// Move the player left or right
			movHor = Input.GetAxis("Horizontal") * horSpeed;
			movePlayer.x = movHor;

			// The Move method does not account for acceleration of gravity (9.81 M/Sec^2)so we must calculate current velocity given
			// the time delta
			// V = a*t + V0
			movePlayer.y = Physics.gravity.y * Time.deltaTime + movePlayer.y;

			if (controller.isGrounded && Input.GetButtonDown("Jump"))
			{
				movePlayer.y = jumpSpeed;
			}

			// The player has rotated so we must add this rotation to our planned movement.
			movePlayer = transform.rotation * movePlayer;

			// Move the player to his new location	   
			controller.Move(movePlayer*Time.deltaTime);

			#endregion

		}
	}
}
// Movement
/*
float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

verticalVelocity += Physics.gravity.y* Time.deltaTime;
		
if( characterController.isGrounded && Input.GetButton("Jump") ) {
verticalVelocity = jumpSpeed;
}
		
Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

speed = transform.rotation* speed;


characterController.Move( speed* Time.deltaTime );
*/