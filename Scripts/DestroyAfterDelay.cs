using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay = 3f; // Adjust this value to set the delay before destruction

    void Start()
    {
        // Invoke the DestroyObject method after the specified delay
        Invoke("DestroyObject", delay);
    }

    void DestroyObject()
    {
        // Destroy the GameObject containing this script
        Destroy(gameObject);
    }
}
