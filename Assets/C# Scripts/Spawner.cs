using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float respawnTime;

    private float timer;
    private GameObject prefabReference;

    private void Awake()
    {
        // Hide the spawner
        gameObject.GetComponent<Renderer>().enabled = false;

        // Spawn in the prefab
        prefabReference = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);

        // Set the respawnTime if none is given or is less than 1
        if (respawnTime < 15)
        {
            // 15 second respawn time will be default value
            respawnTime = 15f;
        }

        // Set the timer
        timer = respawnTime;
    }

    private void FixedUpdate()
    {
        // Timer countdown - will never go below 0
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, respawnTime);

        // If the timer is 0 and the Prefab was picked up
        if (timer == 0 && prefabReference == null)
        {
            // Respawn the Prefab
            prefabReference = Instantiate(prefabToSpawn, gameObject.transform.position, Quaternion.identity);

            // Reset timer
            timer = respawnTime;
        }
    }
}
