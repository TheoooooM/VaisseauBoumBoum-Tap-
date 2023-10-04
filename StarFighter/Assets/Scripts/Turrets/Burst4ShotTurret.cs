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
}
