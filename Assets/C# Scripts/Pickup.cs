using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private PlayerBehavior player;
    private bool pickedUpByPlayer = false;
    private Vector3 velocity = Vector3.zero;

    // Waypoints for the key
    private GameObject wayPoint;
    private Vector3 wayPointPos;

    // H = health, J = Double Jump, K = Key
    public char pickupAbility;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player Reference
            player = collision.gameObject.GetComponent<PlayerBehavior>();

            // If the pick up is set to health
            if (pickupAbility == 'H')
            {
                // If the player is lower than max hp
                if (player.currentHealth < player.maxHealth)
                {
                    Destroy(this.gameObject);
                    player.ChangeHealth(1);
                    // Play particle system
                    player.gameObject.GetComponentsInChildren<ParticleSystem>()[2].Play();
                }
            }
            // If the pick up is set to double jump
            else if (pickupAbility == 'J')
            {
                player.DoubleJump(gameObject);
            }
            // If the pick up is set to key
            else if (pickupAbility == 'K')
            {
                // Create the key waypoint on the player
                player.PlayerGrabbedKey();
                // Get the waypoint object
                FindWaypoint();           
                // Set off the flag that the key was picked up by the player
                pickedUpByPlayer = true;
            }
            else if (pickupAbility == 'G')
            {
                // Destroy the powerup
                Destroy(this.gameObject);
                // Let the player flip gravity
                player.FlipGravity();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Grab the gameObject that's colliding with the pickup object
        var collider = collision.gameObject;

        // If the gameObject is a player
        if (collider.CompareTag("Player"))
        {
            // Player Reference
            player = collider.GetComponent<PlayerBehavior>();

            // If the pick up is set to health
            if (pickupAbility == 'H')
            {
                // If the player is lower than max hp
                if (player.currentHealth < player.maxHealth)
                {
                    Destroy(this.gameObject);
                    player.ChangeHealth(1);
                    // Play particle system
                    player.gameObject.GetComponentsInChildren<ParticleSystem>()[2].Play();
                }
            }
            // If the pick up is set to double jump
            else if (pickupAbility == 'J')
            {
                player.DoubleJump(gameObject);
            }
            // If the pick up is set to key
            else if (pickupAbility == 'K')
            {
                // Create the key waypoint on the player
                player.PlayerGrabbedKey();
                // Get the waypoint object
                FindWaypoint();           
                // Set off the flag that the key was picked up by the player
                pickedUpByPlayer = true;
            } 
            else if (pickupAbility == 'G')
            {
                // Destroy the powerup
                Destroy(this.gameObject);
                // Let the player flip gravity
                player.FlipGravity();
            }
        }
    }

    private void FixedUpdate()
    {
        // If the gameObject is picked up by the player (only the key for now)
        if (pickedUpByPlayer == true && player.waypoint != null)
        {
            // Get the wayPoint position
            wayPointPos = new Vector3(wayPoint.transform.position.x, wayPoint.transform.position.y, wayPoint.transform.position.z);
            // Have the gameObject follow the wayPoint
            transform.position = Vector3.SmoothDamp(transform.position, wayPointPos, ref velocity, .5f, 8f);
        }
    }

    private void FindWaypoint()
    {
        wayPoint = GameObject.FindGameObjectWithTag("KeyWaypoint");
    }
}
