
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class Weapon  : MonoBehaviour
    {

        // We want as many parameters as possible stored within the scriptableObject
        // and this is our reference for it.
        [SerializeField]
        private WeaponData weaponData;

        // this is the holder of all this weapons ammunition
        [SerializeField]
        private MagazineData magazineData;

        // The currentMagazine class this weapon will use to operate
        private Magazine currentMagazine;

        // The collection of currentMagazine's that this weapon will support
        [SerializeField]
        private Magazine[] magazines;


        // Delegates and events needed by the class
        public delegate void shootMethod();
        public shootMethod fire;

        // -------------------------GameObjects from the hierarchy needed by the class---------------------------------
        // The designer must use the editor to set the references
        // These cannot be stored int eh scriptableObject
        public GameObject FirePoint;
        [SerializeField]
        private AudioSource firePointAudioSource;

        // All weapons needs to know where the camera is
        private GameObject playerCamera;

        // We need to cache references to both the sound and visual so we can enable and change them
        // this comes from the currentMunition
        private GameObject muzzleEffect;
        [SerializeField]
        private Transform muzzleEffectParent;

        // -------------------------private variables needed for internally by the class---------------------------------
        // This is all of the parametric data on the currentMunition.
        private Munition currentMunition;
        private GameObject currentProjectile;
        private PoolManger.PoolItem currentPoolItem;

        // Variables related to the state of the weapon
        private bool isReloading;
        private bool isOutofAmmo;

        private GameObject launchedBullet;
        private RaycastHit hitInfo;
        private Ray ray;
        public bool MyDebug = false;
        public int AmmoRemaining;


        // Use this for initialization
        void Start()
        {
            // Get a reference to the player camera
            playerCamera = GameObject.FindGameObjectWithTag("FPCamera");
            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            // Set up the delegates so they know which shoot function to use
            setUpFireDelegate();

        }

        public void SetCurrentMagazine(Magazine _magazine)
        {
            currentMagazine = _magazine;
            // initialize the misc. state variables.
            isReloading = currentMagazine.IsReloading;
            isOutofAmmo = currentMagazine.IsEmpty;
        }

        public List<MagazineData> GetMagazines()
        {
            return weaponData.Magazines.ToList();
        }

        internal List<Munition> GetMunitions()
        {
            return weaponData.Munitions.ToList();
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


        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (MyDebug == true)
            {
                Gizmos.DrawRay(FirePoint.transform.position, FirePoint.transform.forward * 10);
            }
        }

        private void shoot()
        {

            // Get a currentMunition from the active currentMagazine and pool manager if out of ammo take out of ammo action
            if (currentMagazine != null)
            {
                // I am not sure why this test is required but with out it every other frame current magazine seems to be null.
                currentMunition = currentMagazine.GetMunition();
                if (currentMunition != null)
                {
                    // We will send this data to a UI update event soon enough.
                    AmmoRemaining = currentMagazine.CurrentCount;
                }

            }


            if (weaponData.Type == WeaponType.Projectile && currentMunition != null && !isOutofAmmo && !isReloading)
            {
                // This is where we might put a reference to a pool object
               currentPoolItem = currentMunition.Pool.GeItem(FirePoint.transform.position, FirePoint.transform.rotation);
               currentProjectile = currentPoolItem.Item;

                // We must place this data on the projectile so when it collides with an object it can be put back into the pool
                Projectial currentProjectialComponent = currentProjectile.GetComponent<Projectial>();
                currentProjectialComponent.Pool = currentMunition.Pool;
                currentProjectialComponent.LifeTime = currentMunition.LifeTime;
                currentProjectialComponent.PoolItem = currentPoolItem;


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
            ray.origin = FirePoint.transform.position;
            ray.direction = FirePoint.transform.forward;

            // Notify what ever was hit that it was hit and by what
            if (Physics.Raycast(ray, out hitInfo, currentMunition.Range))
            {
                // Send a call to the target that was hit so it can take appropriate action

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
            currentProjectile.GetComponent<Rigidbody>().AddForce(FirePoint.transform.forward * currentMunition.InitialVelocity, ForceMode.Impulse);
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
            if (currentMagazine == null)
            {
                return;
            }
            StartCoroutine(reload_CoRoutine());
        }

        private IEnumerator reload_CoRoutine()
        {


            isReloading = true;

            yield return new WaitForSeconds(magazineData.ReloadTime);
            // Let's replenish the currentMagazine
            currentMagazine.Reload();

            isReloading = false;
            isOutofAmmo = false;
        }

        private void checkHealth()
        {
            // Debug.Log("We are out of ammo and need to reload");
            throw new NotImplementedException();
        }


    }


}



