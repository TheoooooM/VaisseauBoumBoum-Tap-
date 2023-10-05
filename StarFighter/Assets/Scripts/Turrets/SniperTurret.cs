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
        aimTimeRemaining = aimTime;
    }


    private GameObject aimGo, lazerOriginGo;
    
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

        //Instantiate(, target.transform.position, quaternion.identity);
        
        AimAtTarget();
        turretCDRemaining -= Time.deltaTime;
        
        lazerOriginGo = spawners[0];

        if (turretCDRemaining <= 0)
        {
            aimRay.SetActive(true);
            
            
            rayTargetPosition = lazerOriginGo.transform.position + -lazerOriginGo.transform.forward * rangeRay;
            hitPlayer = false;
            
            //First whe check if we can hit the player (or anything)
            ray = new Ray(lazerOriginGo.transform.position, lazerOriginGo.transform.forward * -1);
            //Debug.DrawRay(head.transform.position, head.transform.forward * -rangeRay, Color.red);
            //Debug.DrawRay(spawners[0].transform.position, spawners[0].transform.forward * -rangeRay, Color.green);
            
            if (Physics.Raycast(ray, out rch, rangeRay, ~layerMask)) {
                Vector3 targetHit = rch.point;
                GameObject go = rch.collider.gameObject;

                pb = go.GetComponent<PlayerBehavior>();
                hitPlayer = pb != null;
                if (pb != null) {
                    rayTargetPosition = lazerOriginGo.transform.position + -lazerOriginGo.transform.forward * Vector3.Distance(go.transform.position, lazerOriginGo.transform.position);
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
            
            aimRay.transform.position = (rayTargetPosition + lazerOriginGo.transform.position) / 2;

            Vector3 scale = aimRay.transform.localScale;
            scale.y = Vector3.Distance(hitPlayer ? target.transform.position : rayTargetPosition, lazerOriginGo.transform.position) / 2;
            aimRay.transform.localScale = scale;
        }
    }
    
    public override void AimAtTarget()
    {
        aimGo = spawners[0];
        
        //AimAtTargetFullHead();
        look = aimGo.transform.position - (target.transform.position + target.transform.forward);
        if (look != Vector3.zero)
        {
            rotation = Quaternion.LookRotation(look);
            aimGo.transform.rotation = Quaternion.Slerp(aimGo.transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
    }
}
