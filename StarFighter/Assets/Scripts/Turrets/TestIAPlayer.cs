using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIAPlayer : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 positionTarget = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    float x, y, z;
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, positionTarget) <= 0.1 || positionTarget == Vector3.zero)
        {
            x = Random.Range(-6f, 0f);
            y = Random.Range(1f, 4f);
            z = Random.Range(4, 6f);

            positionTarget = new Vector3(x, y, z);
        }
        transform.position = Vector3.Lerp(transform.position, positionTarget, speed * Time.deltaTime);
    }
}
