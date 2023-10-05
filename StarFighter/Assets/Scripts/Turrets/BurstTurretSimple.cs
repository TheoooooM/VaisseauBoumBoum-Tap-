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
        target = PlayerController.instance.gameObject;
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
    }
}
