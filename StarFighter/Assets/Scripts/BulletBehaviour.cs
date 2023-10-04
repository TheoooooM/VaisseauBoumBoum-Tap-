using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float lifespan;
    private float spawnTime;
    protected int damage = 10;

    
    private EnemyBehavior eb;
    protected virtual void OnTriggerEnter(Collider other)
    {
        eb = other.GetComponent<EnemyBehavior>();

        Debug.Log(other.name);
        Debug.Log(other.transform.parent.transform.parent.name);

        if (other.tag == "CapitalShip")
            eb = other.transform.parent.transform.parent.gameObject.GetComponent<EnemyBehavior>();
        
        if (eb != null && !eb.IsPlayer() )
        {
            Debug.Log("Take damage");
            eb.Hit(damage);
        }
        
        PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Explosion, transform.position, quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= spawnTime + lifespan)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}
