using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

	// The weapon manager handles the interactions between the player and his weapon.
	// This includes weapon selection, options, health and reporting.
	public class WeaponManager : MonoBehaviour
	{

		private static WeaponManager _instance;
		public static WeaponManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<WeaponManager>();
				}
				return _instance;
			}
		}

		// All of the potential weapons a player may chose from must reside int he weaponManager
		// Each is a Weapon Prefab that gets parented to the Weapon holder and each as a weapon script
		[SerializeField]
		private GameObject[] weapons ;

		public GameObject WeaponHolder;
		public GameObject CurrentWeaponGO;
		// The control script for the selected weapon
		[SerializeField]
		private Weapon currentWeapon;
		[SerializeField]
		private Magazine currentMagazine;

		// The list of potential magazines for the selected weapon
		private List<MagazineData> magazines = new List<MagazineData>();
		// The list of potential munitions to be held by a magazine for the selected weapon
		private List<Munition> munitions = new List<Munition>();


		void Start()
		{
			// Create the GameObject structure of the weapon
			CurrentWeaponGO = InitilizeWeapon(weapons[0]);

			// Get a reference to the weapons control logic
			currentWeapon = CurrentWeaponGO.GetComponent<Weapon>();

			// Get the list of potential Magazines and munitions
			magazines = currentWeapon.GetMagazines();
			munitions = currentWeapon.GetMunitions();

			//foreach (var name in magazines)
			//{
			//	Debug.Log(name.Name + "   count: " + name.MaxSize + "  current: " + name.CurrentCount);
			//}
			//foreach (var name in munitions)
			//{
			//	Debug.Log(name.Name + "   count: " + name.Name );
			//}

			// create a magazine with the load specified
			var mix = 1;
			currentMagazine = new Magazine(magazines[1], currentWeapon.FirePoint, munitions, mix);
			currentWeapon.SetCurrentMagazine(currentMagazine);
			
		}




		public GameObject InitilizeWeapon(GameObject _weapon)
		{
			// Instantiate the weapon gameObject.
			// We may want to create all of the potential weapons and simply disable them.
			// this would allow the user to change weapons during game play.
			var temp = Instantiate(_weapon, WeaponHolder.transform,false);

		   return (temp) ;
		}
		public void SetCurrentWeapon(Weapon _weapon)
		{
			// Make the given weapon the active game weapon
		}

		public float GetWeaponHealth()
		{
			return 1f;
		}


		// Get a list of all magazines that the current weapon can use.
		public List<MagazineData> GetMagazines(Weapon _weapon)
		{
			return _weapon.GetMagazines();
		}
		// These functions may belong in the magazine class
		public void CreateMagazine()
		{
			// Give the selected munitions, load the munition in some pattern.
			// Uniform, every other one, random, round robin
		}

		public void SetMagazineMix()
		{
			// Give the selected munitions, load the munition in some pattern.
			// Uniform, every other one, random, round robin
		}

		public void SelectMunitions()
		{
			// The mix of munitions that are placed into a magazine
		}
	}
}
