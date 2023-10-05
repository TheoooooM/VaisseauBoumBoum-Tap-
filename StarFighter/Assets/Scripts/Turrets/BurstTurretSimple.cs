using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstTurretSimple : Turrets
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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
