using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Weapon-1", menuName = "Weapon/Weapon", order = 1)]
    public class WeaponData : ScriptableObject
    {
        public string Version;  // The version of this ScriptableObject

        public string Name;             // the name of the weapon class. IE. AK-47
        public WeaponType Type;        // Ray-cast or projectile
        public Munitions[] Supported;   // A list of the supported munitions

        public bool IsSingleFire;
        public float FireRate;

        // Visual and audio effects generated from this Weapon
        public GameObject KickBackEffect;
        public GameObject HealthEffect;
        public GameObject DestructEffect;
        public AudioClip GunJamSound;

        // Power and health variables
        public float TargetDamage;      // Just how much damage will this munition inflict on the target it hits
        public float WeaponDamage;      // The more powerful the munition the bigger the toll it takes on the weapon. 
        public Vector3 StrikeingForce;  // The resulting impulse force hitting a target

        // Parameters needed if the munition is a projectile
        // Todo: Add a custom inspector to only show these fields if this weapon can fire projectiles
        [Header("Parameters needed to shoot a Projectile ")]
        public GameObject Projectile;

        public float InitialVelocity;   // ToDo: put a range limit on this
        public float Lifetime;          // The max. time this projectile will remain in the scene

    }


}

