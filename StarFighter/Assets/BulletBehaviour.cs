using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float lifespan;
    private float spawnTime;
    private void OnTriggerEnter(Collider other)
    {
        PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Explosion, transform.position, quaternion.identity);
        Destroy(gameObject);
    }

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if(Time.time >= spawnTime + lifespan) Destroy(gameObject);
    }
}
