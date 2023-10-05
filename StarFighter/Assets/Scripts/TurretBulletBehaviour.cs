using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TurretBulletBehaviour : BulletBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private PlayerController pc;
    protected override void OnTriggerEnter(Collider other)
    {
        pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            other.GetComponent<PlayerBehavior>().Hit(damage);
        }
        Debug.Log(other.name);
        
        PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Explosion, transform.position, quaternion.identity);
        gameObject.SetActive(false);
    }
}
