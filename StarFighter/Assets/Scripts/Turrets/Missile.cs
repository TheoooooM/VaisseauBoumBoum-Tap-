using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    
    
    [Header("Follow duration")]
    [SerializeField] private float timeFollow;
    [SerializeField] private float timeFollowRemaining;
    
    
    [Header("Life duration")]
    [SerializeField] private float life;
    [SerializeField] private float lifeRemaining;

    
    [Header("Damage")]
    [SerializeField] private float damage;
    [SerializeField] private float explosionRadius;

    [Header("[Debug] See target")]
    [SerializeField] private GameObject target;
    [SerializeField] private bool started = false;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeRemaining = life;
        timeFollowRemaining = timeFollow;
    }

    // Update is called once per frame
    // void Update()
    // {
    // }

    Quaternion rotation;
    Vector3 look;
    void FixedUpdate()
    {
        if (!started)
            return;
        
        if (timeFollowRemaining > 0 && target != null)
        {
            look = target.transform.position - transform.position;
            if (look != Vector3.zero)
            {
                rotation = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
            timeFollowRemaining -= Time.deltaTime;
        }
        else
        {
            if (lifeRemaining > 0)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                lifeRemaining -= Time.deltaTime;
            }
            else
            {
                Explode();
            }
        }
    }

    private void OnCollisionEnter()
    {
        Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private float distance;
    void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        PlayerController pc;
        PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.EnemyExplosion, transform.position, transform.rotation);
        
        foreach(Collider c in colliders) {
            pc = c.GetComponent<PlayerController>();
            if (pc != null) {
                distance = Vector3.Distance(transform.position, c.transform.position);
                c.GetComponent<PlayerBehavior>().Hit((int)((1f - (distance / explosionRadius)) * damage), true);
            }
        }
        gameObject.SetActive(false);
    }

    public void SetTarget(GameObject t)
    {
        target = t;
        started = true;
    }

    public void LoseFocus()
    {
        timeFollowRemaining = 0;
    }
}
