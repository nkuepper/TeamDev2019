using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Public Declarations
    public bool enableVerticalMovement;
    public bool enableHorizontalMovement;
    public float speed;
    public float distance;

    // Private Declarations
    private bool upToDown = true;
    private bool leftToRight = true;
    private bool playerTouched = false;
    private float startingY;
    private float startingX;

    // Start is called before the first frame update
    void Start()
    {
        // Grab the starting x and y coordinates
        startingY = gameObject.transform.position.y;
        startingX = gameObject.transform.position.x;
    }

    private void FixedUpdate()
    {
        // If the platform movement is up and down and the player touches it
        if (enableVerticalMovement && playerTouched)
        {
            // Start going up
            if (upToDown)
            {
                // If the platforms y position is less than or equal to the starting y coord + distance
                // The platform will go the distance specified, so here we're checking to see if we're under that specified distance
                if (gameObject.transform.position.y <= startingY + distance)
                {
                    // Platform moves up
                    gameObject.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                }

                // If the platforms y position is greater than or equal to the starting y coord + distance
                // The platform will stop at the distance specified and start going down
                if (gameObject.transform.position.y >= startingY + distance)
                {
                    // Trigger the platform going down
                    upToDown = false;
                }
            }
            // Start going down
            else if (upToDown == false)
            {
                // If the platform is above or equal to the starting position
                if (gameObject.transform.position.y >= startingY)
                {
                    // Platform position going down
                    gameObject.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
                }
                   
                // If the platform is less than or equal to the starting position
                if (gameObject.transform.position.y <= startingY)
                {
                    // Trigger the platform going up
                    upToDown = true;
                }
            }
        }

        // If the platform movement is left and right and the player touched it
        if (enableHorizontalMovement && playerTouched)
        {
            // Start going to the right
            if (leftToRight)
            {
                if (gameObject.transform.position.x <= startingX + distance)
                {
                    gameObject.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                }

                if (gameObject.transform.position.x >= startingX + distance)
                {
                    // Trigger left movement
                    leftToRight = false;
                }
            }
            // Start going to the left
            else if (leftToRight == false)
            {
                if (gameObject.transform.position.x >= startingX)
                {
                    gameObject.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                }
            
                if (gameObject.transform.position.x <= startingX)
                {
                    // Trigger right movement
                    leftToRight = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player is touching the platform, move the player with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTouched = true;
            collision.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If the player leaves the platform, stop moving the player with the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
