using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Target : MonoBehaviour {

        [SerializeField]
        private GameObject tempDamage;

        [SerializeField]
        private float lifeTime;

        [SerializeField]
        private string triggerTag;

        [SerializeField]
        private GameObject explosion;

        [SerializeField]
        private Renderer targetRenderer;



        // This method responds to the target being hit by a munition
        void OnCollisionEnter(Collision collision)

        {
            if (collision.gameObject.tag == triggerTag)
            {
                // We must destroy the gameObject of the munition that hit us.
                var item = collision.gameObject.GetComponent<Projectial>().PoolItem;
                var pool = collision.gameObject.GetComponent<Projectial>().Pool;
                pool.ReturnItem(item);
                Explosion();
            }

        }

        void Explosion()
        {
                targetRenderer.enabled = false;
                explosion.SetActive(true);
        }

        private IEnumerator munitionLifetime(PoolManger.Pool _pool, PoolManger.PoolItem _poolItem, float _lifeTime)
        {
            yield return new WaitForSeconds(_lifeTime);
            _pool.ReturnItem(_poolItem);
        }

        void OnEnable()
        {
            EventManager.OnHitByMunition.AddListener(OnHitByMunition);  

        }

        void OnDisable()
        {
            EventManager.OnHitByMunition.RemoveListener(OnHitByMunition);

        }

        public void OnHitByMunition(RaycastHit _hitInfo, WeaponType _type)
        {

            if (_hitInfo.transform.gameObject == this.gameObject)
            {
                Debug.Log("the OnHitByMunition event was called on the target");
                if (WeaponType.Raycaster == _type)
                {
                    //  Instantiate(tempDamage, hitInfo.point, Quaternion.identity);      
                    Explosion();
                }
            }


        }

    }
}
