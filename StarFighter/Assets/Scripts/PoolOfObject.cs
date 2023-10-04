using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PoolOfObject : MonoBehaviour
{
    public Dictionary<Type, Pool> poolOfObject;
    public List<Pool> pools;
    public static PoolOfObject instance;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        poolOfObject = new Dictionary<Type, Pool>();
        foreach (var pool in pools)
        {
            poolOfObject.Add(pool.objectType, pool);
        }
    }

    private void Start()
    {
        foreach (var pool in poolOfObject)
        {
            for (int i = 0; i < pool.Value.count; i++)
            {
                var newObject = Instantiate(pool.Value.prefab, transform.position, Quaternion.identity, transform);
                pool.Value.objects.Add(newObject);
                newObject.SetActive(false);
            }
        }
    }

    public enum Type
    {
        Bullet, Explosion, BulletTurret, Missile
    }
    
    [Serializable]
    public class Pool
    {
        public Type objectType;
        public int count;
        public GameObject prefab;
        [NonSerialized] public List<GameObject> objects = new List<GameObject>();
        [NonSerialized] public int index = 0;
    }

    public GameObject SpawnFromPool(Type type, Vector3 position, Quaternion rotation)
    {
        var pool = poolOfObject[type];
        var newObject = pool.objects[pool.index];
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;

        if (type == Type.Bullet)
        {
            newObject.GetComponent<BulletBehaviour>().SetLifeSpan(5f);
        }
        
        newObject.SetActive(true);
        pool.index++;
        if (pool.index >= pool.objects.Count) pool.index = 0;
        
        return newObject;
    }
}
