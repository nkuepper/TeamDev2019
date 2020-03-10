using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float amplitude = 0.2f;

    // Position variables
    Vector3 initialPosition = new Vector3();
    Vector3 newPosition = new Vector3();
    
    void Start()
    {
        initialPosition = transform.position;
    }
    
    void Update()
    {
        // Floating up and down
        newPosition = initialPosition;
        newPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI) * amplitude;

        transform.position = newPosition;
    }
}
