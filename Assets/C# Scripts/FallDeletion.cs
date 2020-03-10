using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeletion : MonoBehaviour
{
    public SpawnBlock spawn;

    private void Start()
    {
        // If we didn't set a spawn block
        if (spawn == null)
        {
            // set the spawn variable to the spawn block
            spawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<SpawnBlock>();
        }
    }

    void Update()
    {
        // If the gameObject has a y position less than -49
        if (transform.position.y < -49)
        {
            // If the gameObject is the player
            if (this.gameObject.tag == "Player")
            {
                // Make the player take 2 damage
                gameObject.GetComponent<PlayerBehavior>().ChangeHealth(-2);

                // Move the player to the spawnblock
                gameObject.transform.position = spawn.transform.position;

                // If the player is a child of any object, remove the player from the object
                gameObject.transform.parent = null;
            }

            // If the gameObject isn't the player and falls past -50
            if (transform.position.y < -50)
            {
                // Remove the gameObject from the scene
                Destroy(this.gameObject);
            }
        }
    }
}