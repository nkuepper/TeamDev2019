using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    //Start pause methods
    GameObject[] pauseObjects;

    // Use this for initialization
    public void Start()
    {
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
    }

    //Reloads the Level
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Exit()
    {
        Application.Quit();
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            UIElements.instance.showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            UIElements.instance.hidePaused();
        }
    }

    public void SaveGame()
    {
        SaveLoad.SaveGame(PlayerData.getPlayer());
    }

    public void LoadGame()
    {
        //Reload();
        PlayerData data = SaveLoad.LoadGame();
        PlayerData.setPlayer(data);
        PlayerData.setScene(data);
        UIElements.instance.load();
        pauseControl();        
    }

    public void LoadMainMenu()
    {
        Application.LoadLevel(0);
    }

    public void LoadHubWorld()
    {
        Application.LoadLevel(2);
    }
}
