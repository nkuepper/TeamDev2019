using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    public short levelToUnlock;

    public Text timeToFinish;
    public Text finalScore;
    public Canvas endUI;

    private void Start()
    {
        // Disable the end UI
        endUI.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player Reference
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        // If the player touches the end level trigger
        if (player != null)
        {
            // Enable the ending UI
            endUI.enabled = true;

            // Show how long it took to beat the level
            timeToFinish.text = UIElements.instance.CalculateTime();

            // Add up the remaining time to the score
            UIElements.instance.UpdateScore(int.Parse(UIElements.instance.uiTimer.text));

            // Show the final score
            finalScore.text = UIElements.instance.uiScore.text;

            // Remove the Game UI (health, score, timer)
            Destroy(UIElements.instance.gameObject);

            // Make the player invisible
            player.GetComponent<Renderer>().enabled = false;

            // Remove all momentum from the player
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            // Trigger the unlocking of the next tutorial level
            WinLevel();

            // Remove the player object
            Destroy(player);
        }
    }

    public void WinLevel()
    {
        // Set the level to unlock
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
    }
}
