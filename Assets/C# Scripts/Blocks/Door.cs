using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject[] doors;
    public char connection;
    public GameObject unlockedDoorPrompt;
    public GameObject lockedDoorPrompt;
    public bool locked;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    private GameObject unlockedDoorText;
    private GameObject lockedDoorText;

    private void Start()
    {
        // Grabs all the doors in the Scene
        doors = GameObject.FindGameObjectsWithTag("Door");

        // Creates the locked and unlocked door prompt above the door
        unlockedDoorText = Instantiate(unlockedDoorPrompt, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), Quaternion.identity);
        lockedDoorText = Instantiate(lockedDoorPrompt, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), Quaternion.identity);

        if (locked == true)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = lockedSprite;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Player Reference
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        // If the player has a key and presses 'W' on a locked door
        if (player != null && Input.GetKeyDown(KeyCode.W) && player.inAir == false && locked == true && player.grabbedKey == true)
        {
            // Make the door unlocked and destroy the key
            gameObject.GetComponent<SpriteRenderer>().sprite = unlockedSprite;
            Destroy(GameObject.Find("Key"));
            Destroy(player.waypoint);
            player.grabbedKey = false;
            locked = false;
        }

        // If the player is in front of the door, presses 'W', and isn't in the air
        if (player != null && Input.GetKeyDown(KeyCode.W) && player.inAir == false && locked == false)
        {
            // Check to see if the doors array has more than one door in it
            if (doors.Length > 1)
            {
                // Loop through the doors array
                foreach (GameObject door in doors) 
                {
                    // Find the other matching door based on connection
                    if (door.GetComponent<Door>().connection == this.gameObject.GetComponent<Door>().connection &&
                        door.GetComponent<Transform>().position != this.gameObject.GetComponent<Transform>().position &&
                        !player.isInvincible)
                    {
                        // Make player invincible
                        player.InvincibilityOn(1f);

                        // Unlock the other door
                        door.GetComponent<Door>().locked = false;
                        door.GetComponent<SpriteRenderer>().sprite = unlockedSprite;

                        // Move player to the other door
                        player.GetComponent<Transform>().position = door.GetComponent<Transform>().position;
                    }
                }
            }

            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player Reference
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        // If the player is standing in front of a door

        // If the player does NOT have a key and stands in front of the door
        if ((player != null && player.inAir == false && locked == true && player.grabbedKey == true)
            || (player != null && player.inAir == false && locked == false))
        {
            // Show the unlocked door text
            unlockedDoorText.GetComponent<Renderer>().enabled = true;
        }
        else if (player != null && player.inAir == false && locked == true && player.grabbedKey == false)
        {
            // Show the locked door text
            lockedDoorText.GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Player Reference
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        // If the player is standing in front of a door
        if (player != null)
        {
            // Hide the text
            unlockedDoorText.GetComponent<Renderer>().enabled = false;
            lockedDoorText.GetComponent<Renderer>().enabled = false;
        }
    }

}
