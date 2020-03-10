using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {

    //from UIElements float totalTime
    public float time;
    //from PlayerMovement class, int currenthealth
    public int health;
    public float[] position;
    public int score;
    //need to pass the UIElements object to this class
    public static UIElements main;
    public static int loadHealth = 10;
    public static float loadTotalTime;
    public static int loadScore;
    public static GameObject[] enemies = GameObject.FindGameObjectsWithTag("Damage");
    public static GameObject[] breakableBlocks = GameObject.FindGameObjectsWithTag("Break");
    public static GameObject[] mysteryBlocks = GameObject.FindGameObjectsWithTag("Mystery");
    public List<float[]> enemyList = new List<float[]>();
    public List<bool> enemyActive = new List<bool>();
    public List<string> enemyNames = new List<string>();
    public List<float[]> breakList = new List<float[]>();
    public List<bool> breakActive = new List<bool>();
    public List<string> breakNames = new List<string>();
    public List<float[]> mysteryList = new List<float[]>();
    public List<bool> mysteryActive = new List<bool>();
    public List<string> mysteryNames = new List<string>();
    public static int level;

    public PlayerData(UIElements ui)
    {
        main = new UIElements();


        foreach (GameObject g in enemies)
        {
            if (g != null)
            {
                float[] temp = { 0, 0 };
                temp[0] = g.transform.position.x;
                temp[1] = g.transform.position.y;
                enemyList.Add(temp);
                if (g.active) { enemyActive.Add(true); } else { enemyActive.Add(false); }
                enemyNames.Add(g.name);
            }
        }

        foreach (GameObject g in breakableBlocks)
        {
            if (g != null)
            {
                float[] temp = { 0, 0 };
                temp[0] = g.transform.position.x;
                temp[1] = g.transform.position.y;
                breakList.Add(temp);
                if (g.active) { breakActive.Add(true); } else { breakActive.Add(false); }
                breakNames.Add(g.name);
            }
        }

        foreach (GameObject g in mysteryBlocks)
        {
            if (g != null)
            {
                float[] temp = { 0, 0 };
                temp[0] = g.transform.position.x;
                temp[1] = g.transform.position.y;
                mysteryList.Add(temp);
                SpriteRenderer blockRenderer;
                blockRenderer = g.GetComponent<SpriteRenderer>();
                if (blockRenderer.color == Color.gray) { mysteryActive.Add(false); }
                else { mysteryActive.Add(true); }
                mysteryNames.Add(g.name);
            }
        }

        PlayerData.main = ui;
        health = ui.health;
        time = ui.currentTime;
        score = int.Parse(ui.uiScore.text);
        position = PlayerBehavior.savePosition;
        level = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(level);
    }

    public static void updateHealth(int health)
    {
        loadHealth = health;
    }

    public static UIElements getPlayer(){
        return main;
    }

    public static void setPlayer(PlayerData load) {

        loadHealth = load.health;
        loadTotalTime = load.time;
        loadScore = load.score;

        PlayerBehavior behavior = new PlayerBehavior();
        behavior.setPosition(load.position);

    }

    public static void setScene(PlayerData loadData)
    {
        GameObject temp;
        int x = 0;
        foreach (GameObject g in enemies)
        {
            g.SetActive(false);
        }

        foreach (string g in loadData.enemyNames)
        {
            temp = enemies[x];
            if (temp != null)
            {
                temp.SetActive(loadData.enemyActive[x]);
                Vector2 pos;
                pos.x = loadData.enemyList[x][0];
                pos.y = loadData.enemyList[x][1];
                temp.transform.position = pos;
            }
            x++;
        }

        x = 0;
        foreach (GameObject g in breakableBlocks)
        {
            g.SetActive(false);
        }

        foreach (string g in loadData.breakNames)
        {
            temp = breakableBlocks[x];
            if (temp != null)
            {
                temp.SetActive(loadData.breakActive[x]);
                Vector2 pos;
                pos.x = loadData.breakList[x][0];
                pos.y = loadData.breakList[x][1];
                temp.transform.position = pos;
            }
            x++;
        }

        x = 0;

        foreach (bool g in loadData.mysteryActive)
        {
            temp = mysteryBlocks[x];
            if (temp != null)
            {
                SpriteRenderer blockRenderer;
                blockRenderer = temp.GetComponent<SpriteRenderer>();
                if (g == false) { blockRenderer.color = Color.gray; }
                else { }
            }
            x++;
        }
    }
}
