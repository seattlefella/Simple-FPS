using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PoolManger : MonoBehaviour
    {

        public int Count = 0;
        public GameObject PoolParent;

        private int poolKey;
        private Pool NewPool;

        // The game can only have one pool manager
        private static PoolManger _instance;
        public static PoolManger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PoolManger>();
                }
                return _instance;
            }
        }

        private Dictionary<int, Pool> poolDictionary = new Dictionary<int,Pool>();

        public Pool CreatePool(GameObject _preFab, int _poolSize)
        {
            // The pool manager must keep track of all of the pools
            poolKey = _preFab.GetInstanceID();
            if (!poolDictionary.ContainsKey(poolKey))
            {
                NewPool = new Pool(_preFab, _poolSize);
                poolDictionary.Add(poolKey, NewPool);
                return NewPool;
            }

            // The pool already exists so return it
            return poolDictionary[poolKey];

        }

        // Return the pool collection based on the prefab's instanceID
        public Pool GetPool(int _poolKey)
        {
            return poolDictionary[_poolKey];
        }

        // There must be a way to destroy all pools
        public void DestroyAllPools()
        {
            throw new NotImplementedException();
        }

        public class Pool
        {
            public int Count;
            public int EmptyCount;
//            private GameObject preFab;
            private Queue<PoolItem> pool;

            public Pool(GameObject _preFab, int _size)
            {
                // The data structure to hold the items in the pool
                pool = new Queue<PoolItem>();

                // every pool should be able to report on some basic parameters
                Count = _size;
//                preFab = _preFab;

                for (int i = 0; i < _size; i++)
                {
                    GameObject item = Instantiate(_preFab);
                    item.SetActive(false);
                    item.transform.parent = Instance.PoolParent.transform;

                    // Each item in the pool has a prefab,  an enable and in-use flag
                    PoolItem pItem = new PoolItem(item, false, false);

                    // add the new pool item to our list based pool
                    pool.Enqueue(pItem);
                }
            }

            public PoolItem GeItem(Vector3 _position, Quaternion _rotation)
            {
                if (pool.Count > 0)
                {
                    // If the queue is not empty get the next available GameObject
                    PoolItem temp = pool.Dequeue();

                    // It may be useful to keep track of the use state of the pool member
                    // Also we will keep count of how often an item is used to help size the pool
                    temp.IsInUse = true;
                    temp.UsedCount++;
                    // Put the re-used game object in the right position
                    temp.Item.transform.position = _position;
                    temp.Item.transform.rotation = _rotation;

                    // Now that we are in the correct location let's activate the re-used game object
                    temp.Item.SetActive(true);

                    return temp;
                }

                
                // No Item Available
                // TODO:  We may want to take some action here such as make the queue bigger
                EmptyCount++;
                Debug.Log("the pool has run out of items: "+ EmptyCount);
                return null;
            }

            public void ReturnItem(PoolItem _poolItem)
            {
                _poolItem.IsInUse = false;
                _poolItem.Item.SetActive(false);
                pool.Enqueue(_poolItem);
            }

            // Destroy the pool
            public void DestroyPool(Pool _pool)
            {
                // There is some debate on how to destroy List<T> objects
                throw new NotImplementedException();
            }

        }

        public class PoolItem
        {

            // TODO:  check out wither or not these should be public or private
            // TODO:  Should these be protected variables?  Ie. Only member elements of this file can change them?
            public GameObject Item;
            public bool IsInUse;
            public int UsedCount;

            public PoolItem(GameObject _go, bool _isEnabled,bool _isInUse)
            {
                Item = _go;
                IsInUse = _isInUse;
            }
        }

    }

}
