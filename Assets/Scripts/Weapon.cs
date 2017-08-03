using UnityEngine;

namespace Assets.Scripts
{
    public class Weapon : MonoBehaviour
    {

        // General variables do define behavior and stay with the weapon
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
        private GameObject firePoint;
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
        //[SerializeField]
        //private float range;

        [SerializeField] private Munitions munition;

        private RaycastHit hitInfo;
        private Ray ray;
        //		private Vector3 hitPoint;

        // All weapons need to know where the camera is
        private GameObject playerCamera;


        // Use this for initialization
        void Start()
        {

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
        void Update()
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
        public void ShootProjectile()
        {
            // The target senses via OnCollisionEnter And takes action
            launchedBullet = (GameObject)Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
            launchedBullet.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * initialVelocity, ForceMode.Impulse);

            // The munition missed everything so simply destroy it at the end of its life time
            Destroy(launchedBullet, lifeTime);
        }

        public void ShootRayCast()
        {
            ray.origin = firePoint.transform.position;
            ray.direction = firePoint.transform.forward;

            if (Physics.Raycast(ray, out hitInfo, munition.Range))
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
