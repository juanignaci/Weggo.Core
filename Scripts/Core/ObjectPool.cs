using System.Collections.Generic;
using UnityEngine;

namespace Weggo.Core
{
    [System.Serializable]
    public class ObjectPool<T> where T : PoolObject
    {

        const int POOLSIZE = 64;

        T prefab;
        [SerializeField]
        T[] pool;

        public ObjectPool(T prefab)
        {
            this.prefab = prefab;
            pool = new T[POOLSIZE];

            for (int i = 0; i < POOLSIZE; i++) { pool[i] = null; }
        }

        public ObjectPool(T prefab, int poolSize)
        {
            this.prefab = prefab;
            pool = new T[poolSize];

            for (int i = 0; i < poolSize; i++) { pool[i] = null; }
        }

        public T GetNext()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i] == null)
                {
                    pool[i] = Object.Instantiate(prefab);
                    pool[i].Reset();
                    return pool[i];
                }
                else if (!pool[i].gameObject.activeSelf)
                {
                    pool[i].gameObject.SetActive(true);
                    pool[i].Reset();
                    return pool[i];
                }
            }

            return null;
        }

        public T GetNext(Vector3 position)
        {
            var p = GetNext();
            if (p != null)
                p.transform.position = position;

            return p;
        }
    }

}