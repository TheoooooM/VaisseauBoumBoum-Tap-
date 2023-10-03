using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    public GameObject target;
    public GameObject bulletParent;
    public GameObject bulletPrefab;
    public GameObject[] spawners;
    public float agroRange = 5;

    public float turnSpeedX = 2;
    public float turnSpeedY = 2;
    
    public float turretCD = 5;
    public float turretCDRemaining = 0;
    
    public float shotCD = 0.2f;
    public float shotCDRemaining = 0;

    
    public int shotAmount = 1;

    public bool isShooting = false;
    public int shotFired = 0;

    public GameObject head;
    
    
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
        
    }

    public virtual void AimAtTarget()
    {
    }

    public virtual void Shot()
    {}


    public void UpdateHeadBased()
    {
        if (target == null)
            return;

        if (Vector3.Distance(gameObject.transform.position, target.transform.position) > agroRange)
            return;
        
        AimAtTarget();
        turretCDRemaining -= Time.deltaTime;
        
        if (turretCDRemaining <= 0)
        {
            if (isShooting)
            {
                shotCDRemaining -= Time.deltaTime;
            } 
            else
            {
                isShooting = true;
                shotFired = 0;
                shotCDRemaining = 0;
            }
            
            if (shotCDRemaining <= 0)
            {
                Shot();
                shotCDRemaining = shotCD;
                shotFired++;
            }
            
            if (shotFired == shotAmount)
            {
                isShooting = false;
                turretCDRemaining = turretCD;
            }
        }
    }
    
    public void AimAtTargetFullHead()
    {
        Quaternion rotation;
        Vector3 look;
        
        look = target.transform.position - head.transform.position;
        if (look != Vector3.zero)
        {
            rotation = Quaternion.LookRotation(look);
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotation, turnSpeedY * Time.deltaTime);
        }
    }
}
