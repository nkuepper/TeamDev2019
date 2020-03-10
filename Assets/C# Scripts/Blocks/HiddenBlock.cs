using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBlock : MonoBehaviour
{
    // Power Up assigned through the UI
    public GameObject powerUp;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // When the player hits the hidden block this method will run
    public void RevealHiddenBlock()
    {
        // The block must be invisible for this code to run
        if (gameObject.GetComponent<Renderer>().enabled == false)
        {
            // Show the block
            gameObject.GetComponent<Renderer>().enabled = true;

            // Make the block solid all around
            gameObject.GetComponent<PlatformEffector2D>().useOneWay = false;

            // Power Up exists
            if (powerUp != null)
            {
                // Power Up spawns from the block
                Instantiate(powerUp, gameObject.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            }
            // Power up does not exist
            else
            {
                // Increase Score
                UIElements.instance.UpdateScore(100);
            }
        }
    }
}
