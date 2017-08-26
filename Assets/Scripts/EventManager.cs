using UnityEngine;
using UnityEngine.Events;
using System;

namespace Assets.Scripts
{
    [System.Serializable]
    public class HitByMunition : UnityEvent<RaycastHit, WeaponType, Munition> { }

    public class EventManager : MonoBehaviour
    {
        // This must be a monoBehavior
        public static HitByMunition OnHitByMunition = new HitByMunition();

    }


}
    