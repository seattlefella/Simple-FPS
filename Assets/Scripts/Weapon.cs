using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Weapon : MonoBehaviour
    {

        // We want as many parameters as possible stored within the scriptableObject
        // and this is our reference for it.
        [SerializeField]
        private WeaponData weaponData;

        // this is the holder of all this weapons ammunition
        [SerializeField]
        private MagazineData magazineData;

        // The magazine class this weapon will use to operate
        private Magazine magazine;

        // This is all of the parametric data on the munition.
        // Todo: This will be re-factored to come from a magazine class.
 //       [SerializeField]
        private Munition munition;

        // Delegates and events needed by the class
        private delegate void shootMethod();
        private shootMethod fire;


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


        // Variables related to the state of the weapon
        private bool isReloading;

        private bool isOutofAmmo;

        // private variables needed for internal reasons by the class
        private GameObject launchedBullet;
        private RaycastHit hitInfo;
        private Ray ray;


        // Use this for initialization
        void Start()
        {
            // Get a reference to the player camera
            playerCamera = GameObject.FindGameObjectWithTag("FPCamera");
            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            // This class maintains the state of our available ammo
            magazine = new Magazine(magazineData);

            // Set up the delegates so they know which shoot function to use
            setUpFireDelegate();

            // initialize the misc. state variables.
            isReloading = magazine.IsReloading;
            isOutofAmmo = magazine.IsEmpty;

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
                    InvokeRepeating("shoot", 0f, 1f / weaponData.FireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("shoot");
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && isOutofAmmo)
            {

                reload();
            }

        }

        private void shoot()
        {
            // Get a munition from the active magazine if out of ammo take out of ammo action
            munition = magazine.GetMunition();
            if (munition == null || isOutofAmmo || isReloading)
            {
                // Enter the out of ammo state until you are reloaded
                outOfAmmo();
                // reload();

                // you cannot shoot with no ammo!
                return;
            }

            // Check the health of the weapon - one cannot fire if is over heated
            checkHealth();

            // Display the muzzle effects
            muzzleFireEffect();

            // fire the weapon using the method in Start()  projectile or ray.
            fire();

        }

        private void setUpFireDelegate() { 
            // Set up the delegates so they know which shoot function to use
            if (weaponData.Type == WeaponType.Projectile)
            {
                fire = fireProjectile;
            }

            else if (weaponData.Type == WeaponType.Raycaster)
            {
                fire = fireRaycast;
            }

            else
            {
                // just in case we add a type and not a method to shoot it.
                fire = fireRaycast;
            }
        }

        private void fireRaycast()
        {
            // Fire the shot via ray-cast
            ray.origin = firePoint.transform.position;
            ray.direction = firePoint.transform.forward;

            // Notify what ever was hit that it was hit and by what
            if (Physics.Raycast(ray, out hitInfo, munition.Range))
            {
                // Send a call to the target that was hit so it can take appropriate action
                // ToDo: eliminate the need for temp component and move to an event system

                if (EventManager.OnHitByMunition != null)
                {
                    // Send a message using events to the OnWeaponHit event
                    EventManager.OnHitByMunition.Invoke(hitInfo, weaponData.Type); 

                }
            }
        }

        private void fireProjectile()
        {
            // The target senses via OnCollisionEnter And takes action
            launchedBullet = (GameObject)Instantiate(weaponData.Projectile, firePoint.transform.position, firePoint.transform.rotation);
            launchedBullet.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * weaponData.InitialVelocity, ForceMode.Impulse);

            // The munition missed everything so simply destroy it at the end of its life time
            Destroy(launchedBullet, munition.LifeTime);
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

        private void outOfAmmo()
        {

            isOutofAmmo = true;
            // Audio at the location of the FirePoint GameObject
            firePointAudioSource.clip = weaponData.OutOfAmmoSound;
            firePointAudioSource.Play();

            //  Update the UI
            //  Put the weapon in a visible out of ammo state
            //  make a out of ammo sound, IE. the gun did not fire
        }

        private void reload()
        {
            Debug.Log("We have started the  reloading of the magazine");
            StartCoroutine(reload_CoRoutine());
          Debug.Log("We have finished the reloading the magazine");
        }

        private IEnumerator reload_CoRoutine()
        {
            isReloading = true;

            yield return new WaitForSeconds(magazineData.ReloadTime);
            // Let's replenish the magazine
            magazine.Reload();

            isReloading = false;
            isOutofAmmo = false;
        }

        private void checkHealth()
        {
            // Debug.Log("We are out of ammo and need to reload");
            return;
        }


    }


}



