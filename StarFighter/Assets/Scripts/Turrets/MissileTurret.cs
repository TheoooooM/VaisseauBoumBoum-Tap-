using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : Turrets
{
    //public GameObject bulletParent;
    //public GameObject bulletPrefab;
    
    // Start is called before the first frame update
    public override void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeadBased();
    }

    
    /*
     *
     * 
    public virtual void Shoot()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
                position = spawners[i].transform.position;
                bulletDirection = Vector3.zero;
                if (Physics.Raycast(position, spawners[i].transform.forward, out hit, 10000f, layerMask))
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
     */
    
    
    
    
    private Missile missile;
    public override void Shoot()
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
            newBullet = PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Missile, spawners[i].transform.position + spawners[i].transform.up * -1, spawners[i].transform.rotation);
            newBullet.transform.Rotate(new Vector3(+90, 0, 0));

            missile = newBullet.GetComponent<Missile>();
            if (missile != null)
                missile.SetTarget(target);
        }
    }
}
