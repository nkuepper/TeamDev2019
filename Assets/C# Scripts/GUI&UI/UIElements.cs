using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : MonoBehaviour
{
    public static UIElements instance { get; private set; }
    public Text uiHealth;
    public Text uiTimer;
    public Text uiScore;
    public Text uiTimerLabel;
    public float totalTime;
    //used to send health to PlayerData class
    public int health;
    public float currentTime;

    // End UI References
    public Canvas endUI;
    public GameObject timeUILine;
    public Text finalScore;
    public Text endTitle;

    void Awake()
    {
        instance = this;

        totalTime = 300.0f;
        currentTime = totalTime;
        uiTimer.text = totalTime.ToString();

        if (endUI == null)
        {
            Debug.Log("THERE IS NO END UI");
        }
    }

    public void UpdateHealth(int value)
    {
        health = value;
        PlayerData.updateHealth(value);
        uiHealth.text = value.ToString();
        if (health == 0) { gameOver(); }
    }

    public void UpdateScore(int value)
    {
        uiScore.text = (int.Parse(uiScore.text) + value).ToString();
    }

    public void IncrementTime()
    {
        if (int.Parse(uiTimer.text) == 0) { gameOver(); }

        currentTime = currentTime - Time.deltaTime;

        uiTimer.text = Mathf.Round(currentTime).ToString();
    }
    
    public string CalculateTime()
    {
        int timeLeft = int.Parse(uiTimer.text);
        
        int timeToComplete = (int)totalTime - timeLeft;

        return timeToComplete.ToString();
    }

    public void ResetTime()
    {
        currentTime = 300;
    }

    //Start pause methods
    GameObject[] pauseObjects;

    // Use this for initialization
    public void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    public void Update()
    {
        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }

    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
        PlayerData updateInfo = new PlayerData(this);
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //needed to refresh total time after game is loaded
    public void load()
    {
        currentTime = PlayerData.loadTotalTime;
        uiHealth.text = PlayerData.loadHealth.ToString();
        uiScore.text = PlayerData.loadScore.ToString();
        UpdateScore(0);
    }

    public void gameOver()
    {
        // Finds the player object and stores the reference
        PlayerBehavior player = FindObjectOfType<PlayerBehavior>();

        // Enables the End Screen UI
        endUI.enabled = true;

        // Changes the End Title to Game Over because the player lost
        endTitle.text = "Game Over!";

        // Removes the 'Beat the level in --- seconds' line
        Destroy(timeUILine);

        // Shows final score
        UIElements.instance.UpdateScore(int.Parse(UIElements.instance.uiTimer.text));
        finalScore.text = UIElements.instance.uiScore.text;

        // Removes the Game UI (health, score, timer)
        Destroy(UIElements.instance.gameObject);

        // Removes the Player
        player.GetComponent<Renderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Destroy(player);
    }

}