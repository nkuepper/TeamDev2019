using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlatform : MonoBehaviour
{
    public Transform[] wayPoints;

    public bool fallsOff;
    public int currentWayPoint;
    public float speed;

    private Transform targetWayPoint;
    private bool playerTouched = false;
    private bool reverse;

    private void FixedUpdate()
    {
        // If the player touches the platform and the current waypoint won't give us an index out of range error
        if (currentWayPoint < this.wayPoints.Length && playerTouched)
        {
            // If the target waypoint isn't assigned
            if (targetWayPoint == null)
            {
                // Assign the target waypoint to the specified waypoint
                targetWayPoint = wayPoints[currentWayPoint];
            }
            
            // Make the platform move
            Travel();        
        }
    }

    private void Travel()
    {
        // Platform will move towards the target waypoint's position
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * .05f);

        // Once the platform reaches the target waypoint
        if (transform.position == targetWayPoint.position)
        {
            // Going Forwards
            if (!reverse)
            {
                // Increment the waypoint
                currentWayPoint++;

                // If the current waypoint is the last one
                if (currentWayPoint == this.wayPoints.Length)
                {
                    // If we want the platform to fall off
                    if (fallsOff)
                    {
                        // Add a rigid body and remove the script to make the platform fall
                        gameObject.AddComponent<Rigidbody2D>();
                        Destroy(this);
                    }
                    else
                    {
                        // Send the platform backwards
                        reverse = true;

                        // Go back to the waypoint before the last one
                        currentWayPoint -= 2;

                        // Assign the target waypoint to the waypoint before the last one
                        targetWayPoint = wayPoints[currentWayPoint];
                    }
                }
                else
                {
                    // Assign the target waypoint to the next waypoint
                    targetWayPoint = wayPoints[currentWayPoint];
                }
            }
            // Going backwards
            else if (reverse)
            {
                // Decrement the waypoint
                currentWayPoint--;

                // If the current waypoint is the first waypoint
                if (currentWayPoint == -1)
                {
                    // Send the platform forwards
                    reverse = false;

                    // Go back to the waypoint after the first one
                    currentWayPoint += 2;

                    // Assign the target waypoint to the waypoint after the first one
                    targetWayPoint = wayPoints[currentWayPoint];
                }
                else
                {
                    // Assign the target waypoint to the next waypoint
                    targetWayPoint = wayPoints[currentWayPoint];
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the player touches the platform, move the player with the platform
        if (collision.gameObject.tag == "Player")
        {
            playerTouched = true;
            collision.transform.parent = transform.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If the player leaves the platform, stop moving the player
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
