using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public PlayerBehavior player;
    public GameObject playerGameObject;

    public GameObject brickParticlePrefab;
    public GameObject enemyParticlePrefab;
    public GameObject healthParticlePrefab;
    public GameObject playerParticlePrefab;

    private GameObject p;

    private void Awake()
    {
        // Make the Player spawner invisible
        gameObject.GetComponent<Renderer>().enabled = false;

        // If player is specified
        if (player != null)
        {
            // Move the player to the spawn block location
            player.transform.position = gameObject.transform.position;
            p = player.gameObject;
        }
        else
        {
            // TODO: Get the camera to follow the player when created through code
            p = Instantiate(playerGameObject, gameObject.transform.position, Quaternion.identity);

            // If there isn't UI
            if (!GameObject.Find("PauseUI_GUI"))
            {
                // Set the player object to not have UI
                p.GetComponent<PlayerBehavior>().isThereUI = false;
            }
            
            // Disable the EditorGUI
            p.GetComponent<EditorGUI>().enabled = false;

            // Assign camera to follow player
            followPlayer camera;
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<followPlayer>();
            camera.target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        // Give player particle children
        Instantiate(brickParticlePrefab, p.transform).transform.parent = p.transform;
        Instantiate(enemyParticlePrefab, p.transform).transform.parent = p.transform;
        Instantiate(healthParticlePrefab, p.transform).transform.parent = p.transform;
        Instantiate(playerParticlePrefab, p.transform).transform.parent = p.transform;
    }
}
