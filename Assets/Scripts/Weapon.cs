using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

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

        // This is all of the parametric data on the currentMunition.
        private Munition currentMunition;
        private GameObject currentProjectile;
        private PoolManger.PoolItem currentPoolItem;

        // Delegates and events needed by the class
        public delegate void shootMethod();
        public shootMethod fire;


        // The designer must use the editor to set the references
        // These cannot be stored int eh scriptableObject
        [SerializeField]
        private GameObject firePoint;
        [SerializeField]
        private AudioSource firePointAudioSource;

        // All weapons needs to know where the camera is
        private GameObject playerCamera;

        // We need to cache references to both the sound and visual so we can enable and change them
        // this comes from the currentMunition

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
            magazine = new Magazine(magazineData, firePoint);

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
            // Get a currentMunition from the active magazine if out of ammo take out of ammo action
            // TODO: Modify magazine.GetMunition() to return a projectile from a pool
            currentMunition = magazine.GetMunition();


            if (weaponData.Type == WeaponType.Projectile && currentMunition != null && !isOutofAmmo && !isReloading)
            {
                // This is where we might put a reference to a pool object
//                currentProjectile = (GameObject)Instantiate(currentMunition.Projectile, firePoint.transform.position, firePoint.transform.rotation);
               currentPoolItem = currentMunition.Pool.GeItem(firePoint.transform.position, firePoint.transform.rotation);
               currentProjectile = currentPoolItem.Item;


                // The currentMunition missed everything so simply destroy it at the end of its life time
                Disable(currentMunition.Pool, currentPoolItem, currentMunition.LifeTime);
            }

            if (currentMunition == null || isOutofAmmo || isReloading)
            {
                // Enter the out of ammo state until you are reloaded
                outOfAmmo();

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

        private void Disable(PoolManger.Pool _pool, PoolManger.PoolItem _poolItem, float _lifeTime)
        {
            // Wait lifeTime using a yield statement
            StartCoroutine(munitionLifetime( _pool,  _poolItem,  _lifeTime));

        }

        private IEnumerator munitionLifetime(PoolManger.Pool _pool, PoolManger.PoolItem _poolItem, float _lifeTime)
        {
            yield return new WaitForSeconds(_lifeTime);
            _pool.ReturnItem(_poolItem);
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
            if (Physics.Raycast(ray, out hitInfo, currentMunition.Range))
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
            currentProjectile.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * currentMunition.InitialVelocity, ForceMode.Impulse);
        }

        private void muzzleFireEffect()
        {
            // Visual at the location of the FirePoint GameObject
            muzzleEffect = (GameObject)Instantiate(currentMunition.MuzzleEffect, Vector3.zero, Quaternion.identity);
            muzzleEffect.transform.parent = muzzleEffectParent;
            muzzleEffect.transform.localPosition = Vector3.zero;
            muzzleEffect.transform.localRotation = Quaternion.identity;
            muzzleEffect.transform.localScale = Vector3.one;
            muzzleEffect.SetActive(true);

            // Audio at the location of the FirePoint GameObject
            firePointAudioSource.clip = currentMunition.FireSound;
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



