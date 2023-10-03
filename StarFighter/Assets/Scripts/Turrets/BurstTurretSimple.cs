using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstTurretSimple : Turrets
{
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

    void FixedUpdate()
    {}

    public override void AimAtTarget()
    {
        AimAtTargetFullHead();
    }

    public override void Shot()
    {
        Debug.Log("shot");
        GameObject g;
        for (int i = 0; i < spawners.Length; i++)
        {
            g = Instantiate(bulletPrefab, spawners[i].transform.position, spawners[i].transform.rotation,
                bulletParent.transform);
        }
    }
}
