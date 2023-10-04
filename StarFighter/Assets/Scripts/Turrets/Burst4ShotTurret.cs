using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst4ShotTurret : Turrets
{
    public GameObject subHeadLeft;
    public GameObject subHeadRight;
    
    // Start is called before the first frame update
    void Start()
    {
        turretCDRemaining = turretCD;
        isShooting = false;
        shotFired = 0;

        shotCDRemaining = shotCD;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeadBased();
    }
    
    public override void AimAtTarget()
    {
        AimAtTargetFullHead();
        AimAtTargetByHead(subHeadLeft);
        AimAtTargetByHead(subHeadRight);
    }
    
    public override void Shoot()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            position = spawners[i].transform.position;
            bulletDirection = Vector3.zero;
            if (Physics.Raycast(position, spawners[i].transform.forward, out hit, 10000f, layerMask))
            {
                bulletDirection = (hit.point - spawners[i].transform.position).normalized;
            }
            else
            {
                bulletDirection = -spawners[i].transform.up;
            }
            newBullet = PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.BulletTurret, spawners[i].transform.position + spawners[i].transform.up * -2, transform.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = bulletDirection * shotSpeed;
        }
    }
}
