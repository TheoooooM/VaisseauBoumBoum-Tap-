using System;
using System.Collections.Generic;
using Enemies;
using Unity.Mathematics;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    [SerializeField] private ParticleSystem explosion;
    TrailRenderer[] trails;
    [SerializeField] private float lifespan;
    private float spawnTime;
    protected int damage = 10;


    private EnemyBehavior eb;

    private void Start()
    {
        trails = GetComponentsInChildren<TrailRenderer>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        eb = other.GetComponent<EnemyBehavior>();
        var hitable = other.GetComponent<IHitable>();

        /*Debug.Log(other.name);
        Debug.Log(other.transform.parent.transform.parent.name);*/

        //if (other.tag == "CapitalShip") eb = other.transform.parent.parent.gameObject.GetComponent<EnemyBehavior>();
        
        if (eb != null && !eb.IsPlayer() )
        {
            Debug.Log("Take damage");
            eb.Hit(damage);
        }
        if(hitable != null)hitable.Hit(damage);
        
        //Debug.Break();
        PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Explosion, transform.position, quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
        foreach (var trail in trails)
        {
            trail.Clear();
            //transform.GetComponentInChildren<TrailRenderer>().Clear();
        }
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
