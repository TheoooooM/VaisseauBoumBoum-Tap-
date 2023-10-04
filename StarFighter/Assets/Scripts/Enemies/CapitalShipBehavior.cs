using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShipBehaviour : EnemyBehavior
{
    [Header("Mouvement")]
    [SerializeField] private float speed = 0.2f;
    
    [Header("Components")]
    [SerializeField] private GameObject turrets;
    [SerializeField] private GameObject target;
    

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < turrets.transform.childCount; i++)
        {
            turrets.transform.GetChild(i).GetComponent<Turrets>().SetTarget(target); //TODO get PlayerComponent.instance
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        transform.position += Time.deltaTime * speed * transform.forward;
    }
}
