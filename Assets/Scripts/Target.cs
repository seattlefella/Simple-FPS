using UnityEngine;

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

        // This method responds to the Target being hit by a rayShooter
        public void OnShot(RaycastHit hitInfo, WeaponType type)
        {
            if (WeaponType.Raycaster == type)
            {
                //  Instantiate(tempDamage, hitInfo.point, Quaternion.identity);      
                Explosion();
            }

        }


        // This method responds to the target being hit by a munition
        void OnCollisionEnter(Collision collision)

        {
            if (collision.gameObject.tag == triggerTag)
            {
                // We must destroy the gameObject of the munition that hit us.
                Destroy(collision.gameObject);
                Explosion();
            }

        }

        void Explosion()
        {
                targetRenderer.enabled = false;
                explosion.SetActive(true);
        }

    }
}
