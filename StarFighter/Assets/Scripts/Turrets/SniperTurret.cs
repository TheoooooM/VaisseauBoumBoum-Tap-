using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTurret : Turrets
{
    public float aimTime = 5f;
    public float aimTimeRemaining = 5f;

    public GameObject aimRay;
    public float rangeRay = 1;

    
    // Start is called before the first frame update
    void Start()
    {
        turretCDRemaining = turretCD;
        aimTimeRemaining = aimTime;
        isShooting = false;
        shotFired = 0;

        shotCDRemaining = shotCD;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        if (Vector3.Distance(gameObject.transform.position, target.transform.position) > agroRange)
        {
            aimRay.SetActive(false);
            return;
        }
        aimRay.SetActive(true);
        
        AimAtTarget();
        turretCDRemaining -= Time.deltaTime;

        if (turretCDRemaining <= 0 || true)
        {
            Vector3 rayTargetPosition = spawners[0].transform.position + spawners[0].transform.up * -rangeRay;
            bool hitPlayer = false;
            
            //First whe check if we can hit the player (or anything)
            Ray ray = new Ray(spawners[0].transform.position + spawners[0].transform.up * -1, spawners[0].transform.up * -2);
            RaycastHit rch;

            if (Physics.Raycast(ray, out rch, rangeRay)) {
                Vector3 targetHit = rch.point;
                GameObject go = rch.collider.gameObject;

                UnitWithHealth uwh = go.GetComponent<UnitWithHealth>();
                hitPlayer = uwh != null;
                if (uwh != null) {
                    rayTargetPosition = spawners[0].transform.position + -spawners[0].transform.up * Vector3.Distance(target.transform.position, spawners[0].transform.position);
                    //uwh.takeDamage(dmg);
                 }
                else
                {
                    turretCDRemaining = turretCD;
                }
            }
            
            
            
            aimRay.transform.position = (rayTargetPosition + spawners[0].transform.position) / 2;

            Vector3 scale = aimRay.transform.localScale;
            scale.y = (1 / head.transform.localScale.y) * Vector3.Distance(hitPlayer ? target.transform.position : rayTargetPosition, spawners[0].transform.position) / 2;
            aimRay.transform.localScale = scale;
            
            
            if (!isShooting)
            {
                isShooting = true;
                aimTimeRemaining = aimTime;
            }
            aimTimeRemaining -= Time.deltaTime;

            //TODO turretCDRemaining
        }
    }
    
    public override void AimAtTarget()
    {
        AimAtTargetFullHead();
    }
}