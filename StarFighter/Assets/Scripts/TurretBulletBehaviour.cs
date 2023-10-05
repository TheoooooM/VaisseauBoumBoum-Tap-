using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TurretBulletBehaviour : BulletBehaviour
{
    private PlayerController pc;
    protected override void OnTriggerEnter(Collider other)
    {
        pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            other.GetComponent<PlayerBehavior>().Hit(damage);
        }
        
        PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Explosion, transform.position, quaternion.identity);
        gameObject.SetActive(false);
    }
}
