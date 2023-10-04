using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float lifespan;
    private float spawnTime;
    private int damage = 10;
    

    private EnemyBehavior eb;
    private void OnTriggerEnter(Collider other)
    {
        eb = other.GetComponent<EnemyBehavior>();
        if (eb != null)
        {
            Debug.Log("Enemy hit");
            eb.Hit(damage);
        }
        
        PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Explosion, transform.position, quaternion.identity);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if(Time.time >= spawnTime + lifespan) gameObject.SetActive(false);
    }

    public void SetLifeSpan(float life)
    {
        lifespan = life;
        spawnTime = Time.time;
    }
}
