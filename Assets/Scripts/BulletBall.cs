using UnityEngine;

namespace Assets.Scripts
{
	public class BulletBall : MonoBehaviour
	{

		[SerializeField]
		private float lifeTime;

		[SerializeField]
		private string triggerTag;

	
		private GameObject explosion;

		// Use this for initialization
		void Start () {
		
 
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		void OnCollisionEnter(Collision collision)

		{

			//Debug.Log("++++++++++++++++++++++++++++++++++++");
			//Debug.Log("The name of what we hit is: " + collision.gameObject.name);
			//Debug.Log("---"+collision.gameObject.tag+"---");
			//Debug.Log("++++++++++++++++++++++++++++++++++++");
			//var myTag = collision.gameObject.tag;

			if(collision.gameObject.tag == triggerTag)
			{
				//Debug.Log("------------------------------------");
				//Debug.Log("The name of what we hit is: " + collision.gameObject.name);
				//Debug.Log("---" + collision.gameObject.tag + "---");
				//Debug.Log("------------------------------------");

				foreach (Transform child in collision.gameObject.transform)
				{
					if (child.tag == "Explosion")
					{
						collision.gameObject.GetComponent<Renderer>().enabled = false;
						child.gameObject.SetActive(true);
						break;
					}
				}

				// Destroy the munition
				Destroy(gameObject);
			}

			else
			{
				// The munition missed everything so simply destroy it at the end of its life time
				Destroy(gameObject, lifeTime);
			}

		}
	}
}
