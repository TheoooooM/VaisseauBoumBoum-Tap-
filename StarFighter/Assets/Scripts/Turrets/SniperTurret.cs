using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SniperTurret : Turrets
{
    public float aimTime = 5f;
    public float aimTimeRemaining = 5f;

    public GameObject aimRay;
    public float rangeRay = 1;

    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    
    
    private PlayerBehavior pb;
    Vector3 rayTargetPosition;
    bool hitPlayer;
    Ray ray;
    RaycastHit rch;
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
        
        
        AimAtTarget();
        turretCDRemaining -= Time.deltaTime;

        if (turretCDRemaining <= 0)
        {
            aimRay.SetActive(true);
            rayTargetPosition = spawners[0].transform.position + spawners[0].transform.up * -rangeRay*2;
            hitPlayer = false;
            
            //First whe check if we can hit the player (or anything)
            ray = new Ray(spawners[0].transform.position + spawners[0].transform.up * -4, spawners[0].transform.up * -6);

            if (Physics.Raycast(ray, out rch, rangeRay)) {
                Vector3 targetHit = rch.point;
                GameObject go = rch.collider.gameObject;
            
                pb = go.GetComponent<PlayerBehavior>();
                hitPlayer = pb != null;
                if (pb != null) {
                    rayTargetPosition = spawners[0].transform.position + -spawners[0].transform.up * Vector3.Distance(go.transform.position, spawners[0].transform.position);
                    aimTimeRemaining -= Time.deltaTime;
                 }
                else
                {
                    turretCDRemaining = turretCD;
                    aimTimeRemaining = aimTime;
                }
                
                if (go != null && !hitPlayer) aimRay.SetActive(false);

                if (hitPlayer && aimTimeRemaining < 0)
                {
                    pb.Hit(damage);
                    aimRay.SetActive(false);
                    turretCDRemaining = turretCD;
                    aimTimeRemaining = aimTime;
                }
            }
            
            
            
            aimRay.transform.position = (rayTargetPosition + spawners[0].transform.position) / 2;

            Vector3 scale = aimRay.transform.localScale;
            scale.y = (1 / transform.localScale.y) * (1 / head.transform.localScale.y) * Vector3.Distance(hitPlayer ? target.transform.position : rayTargetPosition, spawners[0].transform.position) / 2;
            aimRay.transform.localScale = scale;
        }
    }
    
    public override void AimAtTarget()
    {
        AimAtTargetFullHead();
    }
}
