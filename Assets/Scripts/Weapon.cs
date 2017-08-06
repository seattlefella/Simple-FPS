﻿using UnityEditor;
using UnityEngine;

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


        // Variables related to the state of the weapon
        private bool isReloading;

        private bool isOutofAmmo;

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

            // This class maintains the state of our available ammo
            magazine = new Magazine(magazineData);

            // Set up the delegates so they know which shoot function to use
            setUpShootdeligates();

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
                    InvokeRepeating(shoot.Method.Name, 0f, 1f / weaponData.FireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke(shoot.Method.Name);
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && isOutofAmmo)
            {
                // note:  reloading will be set up to take some seconds and have other effects.
                // We do not want to allow the user to start another reload until it is over.
                reload();
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
            munition = magazine.GetMunition();
            if (munition == null  || magazine.IsEmpty)
            {
                // Enter the out of ammo state until you are reloaded
                outOfAmmo();
                // reload();

                // you cannot shoot with no ammo!
                return;           
            }

            // Check the health of the weapon - one cannot fire if is over heated
            checkHealth();

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

        private void setUpShootdeligates() { 
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

        private void reload()
        {
            isReloading = true;
            // there should be a multi second delay on reloading.
            magazine.Reload();
            isOutofAmmo = false;
            isReloading = false;

          Debug.Log("We have reloaded the magazine");
        }

        private void outOfAmmo()
        {
            //  Update the UI
            //  Put the weapon in a visible out of ammo state
            //  make a out of ammo sound, IE. the gun did not fire
            isOutofAmmo = true;
        }

        private void checkHealth()
        {
            // Debug.Log("We are out of ammo and need to reload");
            return;
        }


    }


}
