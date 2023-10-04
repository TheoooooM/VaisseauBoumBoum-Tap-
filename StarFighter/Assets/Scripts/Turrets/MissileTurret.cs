using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : Turrets
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeadBased();
    }

    private Vector3 spawnPosition;
    private GameObject g;
    private Missile missile;
    public override void Shot()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawnPosition = spawners[i].transform.position + spawners[i].transform.up * -1;
            g = Instantiate( bulletPrefab, spawnPosition, spawners[i].transform.rotation,
                bulletParent.transform);
            
            //g.transform.rotation.z += Quaternion.Euler();
            g.transform.Rotate(new Vector3(+90, 0, 180));

            missile = g.GetComponent<Missile>();
            if (missile != null)
                missile.SetTarget(target);
        }
    }
}
