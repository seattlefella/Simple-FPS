using UnityEngine;

namespace Assets.Scripts
{
    public class Weapon : MonoBehaviour
    {

        // We want as many parameters as possible stored within the scriptableObject
        // and this is our reference for it.
        [SerializeField]
        private WeaponData weaponData;

        // This is all of the parametric data on the munition.
        // Todo: This will be re-factored to come from a magazine class.
        [SerializeField]
        private Munitions munition;

        // Delegates and events needed by the class
        private delegate void shootMethod();
        private shootMethod shoot;

        // The designer must use the editor to set the references
        // These cannot be stored int eh scriptableObject
        [SerializeField]
        private GameObject firePoint;
        [SerializeField]
        private AudioSource firePointAudioSource;

        // All weapons needs to know where the camera is
        private GameObject playerCamera;

        // We need to cache references to both the sound and visual so we can enable and change them
        // this comes from the munition

        private GameObject muzzleEffect;

        [SerializeField]
        private Transform muzzleEffectParent;


        // private variables needed for internal reasons by the class
        private Target tempComponent;       // ToDo: eliminate the need for this variable
        private GameObject launchedBullet;
        private RaycastHit hitInfo;
        private Ray ray;



        // Use this for initialization
        void Start()
        {
            // Get a reference to the player camera
            playerCamera = GameObject.FindGameObjectWithTag("FPCamera");
            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            // Set up the delegates so they know which shoot function to use
            if (weaponData.Type == WeaponType.Projectile)
            {
                shoot = ShootProjectile;
            }

            else if (weaponData.Type == WeaponType.Raycaster)
            {
                shoot = ShootRayCast;
            }

            else
            {
                // just in case we add a type and not a method to shoot it.
                shoot = ShootRayCast;
            }
        }

        // Update is called once per frame
        void Update()
        {

            // Capture and use the player input
            if (Input.GetButtonDown("Fire1") && weaponData.IsSingleFire)
            {
                shoot();
            }

            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    InvokeRepeating(shoot.Method.Name, 0f, 1f / weaponData.FireRate);
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
            launchedBullet = (GameObject)Instantiate(weaponData.Projectile, firePoint.transform.position, firePoint.transform.rotation);
            launchedBullet.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * weaponData.InitialVelocity, ForceMode.Impulse);
            muzzleFireEffect();
            // The munition missed everything so simply destroy it at the end of its life time
            Destroy(launchedBullet, munition.LifeTime);
        }

        public void ShootRayCast()
        {

            // Get a munition from the active magazine if out of ammo take out of ammo action

            // Check the health of the weapon - one cannot fire if is over heated

            // Fire the shot
            ray.origin = firePoint.transform.position;
            ray.direction = firePoint.transform.forward;

            muzzleFireEffect();


            // Notify what ever was hit that it was hit and by what
            if (Physics.Raycast(ray, out hitInfo, munition.Range))
            {
                // Send a call to the target that was hit so it can take appropriate action
                // ToDo: eliminate the need for temp component and move to an event system
                tempComponent = hitInfo.transform.gameObject.GetComponent<Target>();
                if (tempComponent != null)
                {
                    // ToDo: Send data on the munition that was used so the correct effect can be implemented
                    tempComponent.OnShot(hitInfo, weaponData.Type);
                }
             }
         }

        private void muzzleFireEffect()
        {
            // Visual at the location of the FirePoint GameObject
            muzzleEffect = (GameObject)Instantiate(munition.MuzzleEffect, Vector3.zero, Quaternion.identity);
            muzzleEffect.transform.parent = muzzleEffectParent;
            muzzleEffect.transform.localPosition = Vector3.zero;
            muzzleEffect.transform.localRotation = Quaternion.identity;
            muzzleEffect.transform.localScale = Vector3.one;
            muzzleEffect.SetActive(true);

            // Audio at the location of the FirePoint GameObject
            firePointAudioSource.clip = munition.FireSound;
            firePointAudioSource.Play();

            Destroy(muzzleEffect, 2f);
        }
    }


}
