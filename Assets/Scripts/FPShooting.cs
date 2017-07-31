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
		[SerializeField]
		private float fireRate;
		[SerializeField]
		private float range;
		private RaycastHit hitInfo;
		private Ray ray ;
		private Vector3 hitPoint;

		// All munitions need to know where the camera is
		private GameObject playerCamera;


		// Use this for initialization
		void Start () {

			playerCamera = GameObject.FindGameObjectWithTag("FPCamera");
			ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

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

				if (isProjectile && !isRayCast)
				{
					ShootRayCast();
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

		public void ShootRayCast()
		{

			// Todo: Add logic to limit the rate of fire.
			// Todo: Add logic to limit ammunition.
			// Todo: Add logic to reload.
//			Ray ray = new Ray(playerCamera.transform.position,playerCamera.transform.forward);
			ray.origin = playerCamera.transform.position;
			ray.direction = playerCamera.transform.forward;


			if(Physics.Raycast(ray, out hitInfo, range))
			{
				hitPoint = hitInfo.point;
				// Send a call to the target that was hit so it shows that it was hit.
			}



		}
	}
}
