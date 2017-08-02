using UnityEngine;

namespace Assets.Scripts
{
	public class FPShooting : MonoBehaviour
	{
		// General variables do define behavior
		[SerializeField]
		private bool isSingleFire;
		[SerializeField]
		private float fireRate;

		private Target tempComponent;

		private delegate void shootMethod();
		private shootMethod shoot;

		// Variables needed by projectile munitions
		[Header("Parameters needed to shoot a GameObject")]
		[SerializeField]
		private GameObject bullet;
		[SerializeField]
		private float initialVelocity;
		[SerializeField]
		private bool isProjectile;
		private GameObject launchedBullet;
		[SerializeField]
		private float lifeTime;

		// Variables needed by ray cast munitions
		[SerializeField]
		private bool isRayCast;
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

			if (isProjectile && !isRayCast)
			{
				 shoot = ShootProjectile;
			}

			if (isRayCast)
			{
				shoot = ShootRayCast;
			}

		}
	
		// Update is called once per frame
		void Update ()
		{

			if (Input.GetButtonDown("Fire1") && isSingleFire)
			{
				shoot();				
			}

			else
			{
				if (Input.GetButtonDown("Fire1"))
				{
					InvokeRepeating(shoot.Method.Name, 0f, 1f / fireRate);
				}
				else if (Input.GetButtonUp("Fire1"))
				{
					CancelInvoke(shoot.Method.Name);
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
			// Todo: Convert the different types of shooters into delegates that are selected at startup
			// Todo: Move I have been hit logic to the target - make it his responsibility
			// Todo: Add logic to limit ammunition.
			// Todo: Add logic to reload.



			launchedBullet = (GameObject)Instantiate(bullet, playerCamera.transform.position + playerCamera.transform.forward * 1, playerCamera.transform.rotation);
			launchedBullet.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * initialVelocity, ForceMode.Impulse);
			// The munition missed everything so simply destroy it at the end of its life time
			Destroy(launchedBullet, lifeTime);
			// The target senses via OnCollisionEnter And takes action
		}

		public void ShootRayCast()
		{
			ray.origin = playerCamera.transform.position;
			ray.direction = playerCamera.transform.forward;

			if(Physics.Raycast(ray, out hitInfo, range))
			{
				// Send a call to the target that was hit so it can take appropriate action
				tempComponent = hitInfo.transform.gameObject.GetComponent<Target>();
				if (tempComponent != null)
				{
					// We will have to send data on the munition that hit it so the correct effect can be implemented
					tempComponent.OnShot(hitInfo, isRayCast);
				}
			}
		}
	}
}
