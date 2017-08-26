using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Magazine
    {
        private int size;
        public int CurrentCount;
        public bool IsEmpty;
        public bool IsReloading;
        private PoolManger poolManger;
        private PoolManger.Pool pool;

        // the data we are using to build new magazine clips
        private MagazineData currentMagazine;

        private Munition currentMunition;
        private List<Munition> munitions = new List<Munition>();

        // The data structure used to hold the munitions set up as a LIFO store
        private Stack<Munition> currentClip = new Stack<Munition>();


        // Let's create a pool of munition objects
//

        // As this is a non mono-behavior we need a constructor
        public Magazine(MagazineData _data, GameObject _firePoint, List<Munition> _munitions, int _mix)
        {
            // Let's keep a reference to the magazine operating parameters
            currentMagazine = _data;
            munitions = _munitions;

            // Let's get a reference to the PoolManager
            poolManger = PoolManger.Instance;

            // initialize the class properties
            size = _data.MaxSize;
            IsEmpty = true;
            CurrentCount = 0;
            IsReloading = false;

            // Load the clip using the mix of munitions specified
            loadClip(currentClip);

        }

        public Munition GetMunition()
        {
            if (currentClip.Count > 0)
            {
                currentMunition = currentClip.Pop();
                CurrentCount = currentClip.Count;
                // It will attempt to create a pool but if it already exits it will simply return it.
                // TODO:  We might have to check if the projectile prefab is null
                pool = poolManger.CreatePool(currentMunition.Projectile, currentMagazine.MaxSize);
                currentMunition.Pool = pool;

                // passing it on to the shoot method.
                return currentMunition;
            }

            // The clip is empty so return empty handed
            IsEmpty = true;
            return null;
        }

        public void Reload()
        {
            // This is in a separate function to allow for loading effects and other
            loadClip(currentClip);
        }

        private Stack<Munition> loadClip(Stack<Munition> _clip)
        {
            // Create an object pool for each potential munition
            foreach (Munition munitionType in munitions) 
            {
                // Create the pool
                var temp = poolManger.CreatePool(munitionType.Projectile, currentMagazine.MaxSize);
                // tie the new pool to the munition object
                munitionType.Pool = temp;
            }

            // TODO:  add logic to change the mix of munitions as specified in the MagazineData
            for (var i = 0; i < size; i++)
            {
                // That is the clip really only holds data about the bullet and a reference to a pool
                // Where a game object can be had.

                _clip.Push(munitions[0]);
            }

            IsReloading = false;
            CurrentCount = size;
            IsEmpty = false;

            return _clip;
        }

    }


}
