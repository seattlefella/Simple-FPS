using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Weapon-1", menuName = "Weapon/Weapon", order = 1)]
    public class WeaponData : ScriptableObject
    {
        public string Version;  // The version of this ScriptableObject

        public string Name;             // the name of the weapon class. IE. AK-47
        public WeaponType Type;        // Ray-cast or projectile

        // Weapon specifics
        public Munition[] Munitions;   // A list of the supported munitions
        public MagazineData[] Magazines;   // The magazine's that this weapon will support

        public bool IsSingleFire;
        public float FireRate;
        public float JamRate;       //What percentage of shots will jam

        // Visual and audio effects generated from this Weapon
        public GameObject KickBackEffect;
        public GameObject HealthEffect;
        public GameObject DestructEffect;
        public GameObject GunJamEffect;
        public AudioClip GunJamSound;
        public AudioClip OutOfAmmoSound;

        // Health variables
        public float WeaponHealth;      // The current health of the weapon
        public float WeaponMaxHealth;   // The health when the weapon is new.
        public float WeaponHealRate;    // The rate at which the weapon will cool, or heal from damage

        // Changes if any to the weapon if the munition is a projectile
            // So far all have been moved to the munition class


    }


}

