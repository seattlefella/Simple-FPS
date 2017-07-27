using UnityEngine;

namespace Assets.Scripts
{
	public class FPShooting : MonoBehaviour
	{

		// Variables needed by projectile munitions
		[Header("Parameters needed to shoot a GameObject")]
		[SerializeField]
		private GameObject bullet;
		[SerializeField]
		private float initialVelocity;
		[SerializeField]
		private bool isProjectile;
		private GameObject launchedBullet;

		// Variables needed by ray cast munitions
		[SerializeField]
		private bool isRayCast;

		// All munitions need to know where the camera is
		private GameObject playerCamera;


		// Use this for initialization
		void Start () {

			playerCamera = GameObject.FindGameObjectWithTag("FPCamera");

		}
	
		// Update is called once per frame
		void Update ()
		{

			if (Input.GetButtonDown("Fire1"))
			{

				if (isProjectile && !isRayCast)
				{
					ShootProjectile();
				}

			}
		
		}

		// this method will actually use the physics system to move a game object through space
		// Care must be taken in selecting the speed of the projectile as if the physics engine cannot keep up
		// The game object will pass through scene objects.  Also, the designer may have to change how fast physic
		// updates are done.
		public void  ShootProjectile()
		{

			// Todo: Add logic to limit the rate of fire.
			// Todo: Add logic to limit ammunition.
			// Todo: Add logic to reload.

			launchedBullet = (GameObject)Instantiate(bullet, playerCamera.transform.position + playerCamera.transform.forward * 1, playerCamera.transform.rotation);
			launchedBullet.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * initialVelocity, ForceMode.Impulse);
		}
	}
}
