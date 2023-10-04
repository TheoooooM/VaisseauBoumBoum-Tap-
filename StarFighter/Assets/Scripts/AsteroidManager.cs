using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Header("Location")]
    [SerializeField] private int radius = 400;
    [SerializeField] private float minY = 0;
    [SerializeField] private float maxY = 30;
    [SerializeField] private float widthPercent = 0.2f;
    
    
    [Header("Asteroids")]
    [SerializeField] private int asteroidsAimed = 400;
    [SerializeField] private GameObject[] asteroidsPossible;
    
    
    // Start is called before the first frame update
    void Start()
    {
        float x, y, z;
        float theta, r;
        
        for (int i = 0; i < asteroidsAimed; i++)
        {
            r = radius + radius * widthPercent * Random.Range(0f, 1f);
            theta = Random.Range(0f, 1f) * 2 * Mathf.PI;

            x = 0 + r * Mathf.Cos(theta);
            z = 0 + r * Mathf.Sin(theta);
            y = Random.Range(minY, maxY);
            Instantiate(asteroidsPossible[Random.Range(0, asteroidsPossible.Length)], new Vector3(x, y, z), Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
