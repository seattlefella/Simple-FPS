
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Munition-1", menuName = "Weapon/Munition", order = 1)]
    public class Munition : ScriptableObject
    {
        public string Version;
        // Variables needed by all munitions
        public bool IsRayCast;

        public string Name;         // the name of the munition class. Caliber, color, power - descriptive and helps the user select which to use
        public string Type;         // Ray-cast or projectile  TODO: Convert to an enum
        public string Supported;    // which weapon can use this munition

        // Visual and audio effects generated from this munition
        public GameObject MuzzleEffect;
        public GameObject TravelEffect;
        public GameObject DestructEffect;
        public AudioClip FireSound;

        // Te physical properties of this munition
        public float Range;             // How far will the rayCaster look for or how far can the gun shoot.
        public float LifeTime;          // All effects and instances go away

        public float TargetDamage;      // Just how much damage will this munition inflict on the target it hits
        public Vector3 StrikeingForce;  // The resulting impulse force hitting a target
        public float WeaponDamage;      // The toll this munition takes on the weapon


    }
}
