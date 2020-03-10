using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubDoor : MonoBehaviour
{
    public short playOrder;
    public short levelToLoad;
    public bool locked;
    public GameObject doorPrompt;

    private bool promptVisible;
    private GameObject instaniatedDoorPrompt;

    private void Start()
    {
        // Creates the door prompt above the door
        instaniatedDoorPrompt = Instantiate(doorPrompt, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), Quaternion.identity);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Player Reference
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        // if the player is standing in front of the door, presses 'w', isn't in the air, and the door isn't locked
        if (player != null && Input.GetAxis("Vertical") > 0 && player.inAir == false && !this.locked)
        {
            // Load the level specified by the door
            Application.LoadLevel(levelToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player Reference
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        // If the player is standing in front of an unlocked door
        if (player != null && !this.locked)
        {
            // Show the prompt
            instaniatedDoorPrompt.GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        // If the player left the trigger in front of the unlocked door
        if (player != null && !this.locked)
        {
            // Hide the prompt
            instaniatedDoorPrompt.GetComponent<Renderer>().enabled = false;
        }
    }
}
