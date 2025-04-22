using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class PoolManager : MonoBehaviour
    {
        private Dictionary<GameObject, Stack<GameObject>> _prefabToPools = new();
        private Dictionary<GameObject, Stack<GameObject>> _objectToPools = new();

        public GameObject Get(GameObject prefab)
        {
            var pool = GetPool(prefab);
            var pooledObject = pool.Count > 0 ? pool.Pop() : Instantiate(prefab);
            _objectToPools[pooledObject] = pool;

            pooledObject.SetActive(true);
            return pooledObject;
        }

        public void Push(GameObject pooledObject)
        {
            var pool = _objectToPools[pooledObject];
            pool.Push(pooledObject);
            
            pooledObject.SetActive(false);
            pooledObject.transform.SetParent(transform);
        }
        
        private Stack<GameObject> GetPool(GameObject prefab)
        {
            if (_prefabToPools.TryGetValue(prefab, out var pool))
                return pool;

            pool = new Stack<GameObject>();
            _prefabToPools.Add(prefab, pool);

            return pool;
        }
    }
}