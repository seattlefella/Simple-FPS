using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Target : MonoBehaviour 
    {
        [Header("Parameters needed for all munitions ")]
        // The tag placed on an item to indicate that it will be effected by a munition
        [SerializeField]
        private string triggerTag;

        [SerializeField]
        private bool isDestroyable;

        [SerializeField]
        private bool isMarkable;

        [SerializeField]
        private bool isMoveable;

        [SerializeField]
        private bool isHitEffect;

        [Header("Parameters needed showing the point that was hit")]
        [SerializeField]
        private GameObject hitMark;


        [Header("Parameters needed for hit effects - explosions, smoke, sparks...")]
        // The effect shown when the munition hits a target.  And hitEffect, smoke, sparks,... depending on the target
        [SerializeField]
        private GameObject hitEffect;

        // We must know the renderer so we can shut it off at the end of the effect.
        // ToDo: add this to a start function so the designer does not have to populate it
        [SerializeField]
        private Renderer targetRenderer;

        [Header("Parameters needed for projectile damage ")]
        // We must put the projectile back in the pool after life time has passed.
        [SerializeField]
        private float lifeTime;


        #region Hit by a projectile


        // This method responds to the target being hit by a munition
        void OnCollisionEnter(Collision collision)

        {
            if (collision.gameObject.tag == triggerTag)
            {
                // We must destroy the gameObject of the munition that hit us.
                //TODO: eliminate the var  by declaring variables above
                var item = collision.gameObject.GetComponent<Projectial>().PoolItem;
                var pool = collision.gameObject.GetComponent<Projectial>().Pool;
                pool.ReturnItem(item);
                Explosion();
            }

        }

        private IEnumerator munitionLifetime(PoolManger.Pool _pool, PoolManger.PoolItem _poolItem, float _lifeTime)
        {
            yield return new WaitForSeconds(_lifeTime);
            _pool.ReturnItem(_poolItem);
        }

        #endregion


        #region Hit by a ray-cast

        void OnEnable()
        {
            EventManager.OnHitByMunition.AddListener(OnHitByRayMunition);
        }

        void OnDisable()
        {
            EventManager.OnHitByMunition.RemoveListener(OnHitByRayMunition);

        }

        public void OnHitByRayMunition(RaycastHit _hitInfo, WeaponType _type, Munition _currentMunition)
        {

            if (_hitInfo.transform.gameObject == this.gameObject)
            {
                if (WeaponType.Raycaster == _type)
                {

                    if (isMarkable)
                    {
                      Instantiate(hitMark, _hitInfo.point, Quaternion.identity);                      
                    }

                    if (isHitEffect)
                    {
                        HitEffect();
                    }

                    if (isMoveable)
                    {
                        ApplyMunitionForce(_hitInfo, _currentMunition);
                    }

                    if (isDestroyable)
                    {
                        Explosion();
                    }

                }

            }


        }

        #endregion

        void ApplyMunitionForce(RaycastHit _hitInfo,Munition _currentMunition)
        {
            this.GetComponent<Rigidbody>().AddForce(_hitInfo.transform.forward * _currentMunition.StrikeingForce, ForceMode.Impulse);

        }
        private void HitEffect()
        {
            throw new System.NotImplementedException();
        }

        void Explosion()
        {
                targetRenderer.enabled = false;
                hitEffect.SetActive(true);
        }

    }
}
