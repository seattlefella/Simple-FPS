using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Munition-1", menuName = "Weapon/Munition", order = 1)]
    public class Munitions : ScriptableObject {

        // Variables needed by all munitions
        public bool IsRayCast;

        public string Name;     // the name of the munition class. Caliber, color, power - descriptive and helps the user select which to use
        public string Type;     // Not sure what this will be used for yet.  Tracer? Kill Power

        public GameObject MuzzleEffect;
        public GameObject TravelEffect;
        public GameObject DestructEffect;

        public float Range;         // How far will the rayCaster look for or how far can the gun shoot.
        public float LifeTime;      // All effects and instances go away
        public float TargetDamage;  // Just how much damage will this munition inflict on the target it hits
        public float WeaponDamage;  // The more powerful the munition the bigger the toll it takes on the weapon.
    }
}
