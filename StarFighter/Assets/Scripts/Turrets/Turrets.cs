using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    [Header("Components & targets")]
    [SerializeField] protected GameObject target;
    [SerializeField] protected GameObject[] spawners;
    [SerializeField] protected GameObject head;
    [SerializeField] protected LayerMask layerMask;
    
    
    [Header("Combat variables")]
    [SerializeField] protected float agroRange = 5;
    [SerializeField] protected float turnSpeed = 2;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int shotAmount = 1;
    [SerializeField] protected float shotSpeed = 1f;
    
    
    [Header("CD between burst")]
    [SerializeField] protected float turretCD = 5;
    [SerializeField] protected float turretCDRemaining = 0;
    
    
    [Header("CD between each shot of one burst")]
    [SerializeField] protected float shotCD = 0.2f;
    
    
    protected float shotCDRemaining = 0;
    protected bool isShooting = false;
    protected int shotFired = 0;
    
    
    
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
                // if (Physics.Raycast(position, -spawners[i].transform.up, out hit, 10000f, layerMask))
                // {
                //     bulletDirection = (hit.point - spawners[i].transform.position).normalized;
                // }
                // else
                // {
                //     bulletDirection = -spawners[i].transform.up;
                // }
                bulletDirection = -spawners[i].transform.up;
                newBullet = PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.BulletTurret, spawners[i].transform.position + spawners[i].transform.up * -3, transform.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = bulletDirection * shotSpeed;
                newBullet.GetComponent<TurretBulletBehaviour>().SetDamage(damage);
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

    public void SetTarget(GameObject go)
    {
        target = go;
    }
}
