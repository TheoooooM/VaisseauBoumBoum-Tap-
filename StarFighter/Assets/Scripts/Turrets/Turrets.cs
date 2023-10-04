using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    public GameObject target;
    public GameObject[] spawners;
    public GameObject head;
    public LayerMask layerMask;

    
    public float agroRange = 5;
    public float turnSpeed = 2;
    
    public float turretCD = 5;
    public float turretCDRemaining = 0;
    
    public float shotCD = 0.2f;
    public float shotCDRemaining = 0;
    
    public int shotAmount = 1;
    public float shotSpeed = 1f;
    
    public bool isShooting = false;
    public int shotFired = 0;
    
    //public GameObject bulletParent;
    //public GameObject bulletPrefab;
    
    
    
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
        AimAtTargetFullHead();
    }

    protected RaycastHit hit;
    protected Vector3 position;
    protected Vector3 bulletDirection;
    protected GameObject newBullet;
    public virtual void Shoot()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
                position = spawners[i].transform.position;
                bulletDirection = Vector3.zero;
                if (Physics.Raycast(position, -spawners[i].transform.up, out hit, 10000f, layerMask))
                {
                    bulletDirection = (hit.point - spawners[i].transform.position).normalized;
                }
                else
                {
                    bulletDirection = -spawners[i].transform.up;
                }
                newBullet = PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.BulletTurret, spawners[i].transform.position + spawners[i].transform.up * -1, transform.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = bulletDirection * shotSpeed;
        }
    }


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
                Shoot();
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
        AimAtTargetByHead(head);
    }
    
    
    Quaternion rotation;
    Vector3 look;
    public void AimAtTargetByHead(GameObject headSelected)
    {
        look = target.transform.position - headSelected.transform.position;
        if (look != Vector3.zero)
        {
            rotation = Quaternion.LookRotation(look);
            head.transform.rotation = Quaternion.Slerp(headSelected.transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
    }
}
