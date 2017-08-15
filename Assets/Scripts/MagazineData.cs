using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Magazine-1", menuName = "Weapon/Magazine", order = 1)]
    public class MagazineData : ScriptableObject
    {
        public string Version;  // The version of this ScriptableObject

        public string Name;             // the name of the Magazine class. IE. 30 rounds
        public MagazineType Type;        // For now, just Single or double - faster reload time
 //       public Munition[] Supported;   // A list of the supported munitions

        // Magazine Specifics
        public int MaxSize;         // The maximum number of munitions this magazine can hold
        public int CurrentCount;    // The number of munitions currently remaining
        public bool IsEmpty;        // The current state of the magazine
        public float ReloadTime;    // How long will it take to reload


        // Visual and audio effects generated from this Weapon
        public GameObject ReloadingEffect;
        public AudioClip LoadingSound;

        // Health variables
        public float MagazineHealth;      // The current health of the weapon
        public float MagazineMaxHealth;   // The health when the weapon is new.
        public float MagazineHealRate;      // The rate at which the magazine will heal from over use.

    }



}
