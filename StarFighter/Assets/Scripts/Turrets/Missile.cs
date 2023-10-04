using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    
    public float timeFollow;
    public float timeFollowRemaining;
    public float life;
    public float lifeRemaining;

    public float damage;
    public float explosionRadius;

    public GameObject target;

    [SerializeField] private bool started = false;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeRemaining = life;
        timeFollowRemaining = timeFollow;
    }

    // Update is called once per frame
    void Update()
    {
    }

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
            transform.Translate(speed * Time.deltaTime * transform.forward);
            timeFollowRemaining -= Time.deltaTime;
        }
        else
        {
            if (lifeRemaining > 0)
            {
                transform.Translate(speed * Time.deltaTime * transform.forward);
                lifeRemaining -= Time.deltaTime;
            }
            else
            {
                Explode();
            }
        }
    }

    void OnTriggerEnter() {
        Explode();
    }

    void Explode() {
        Destroy(gameObject);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        PlayerController pc;

        foreach(Collider c in colliders) {
            pc = c.GetComponent<PlayerController>();
            if (pc != null) {
                float distance = Vector3.Distance(transform.position, c.transform.position);
                float dmgM = 1f - (distance / explosionRadius);

                //PlayerController.takeDamage(damage * dmgM);
            }
        }
    }

    public void SetTarget(GameObject t, float sp)
    {
        target = t;
        speed = sp;
        started = true;
    }
}
