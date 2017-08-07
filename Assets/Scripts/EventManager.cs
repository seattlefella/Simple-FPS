using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [System.Serializable]
    public class HitByMunition : UnityEvent<RaycastHit, WeaponType> { }

    public class EventManager : MonoBehaviour
    {
        // This must be a monoBehavior
        public static HitByMunition OnHitByMunition = new HitByMunition();

    }
}
    