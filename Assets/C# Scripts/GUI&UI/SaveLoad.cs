using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad{

    //Saves the game
    public static void SaveGame(UIElements ui)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamesave.save";
        FileStream file = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(ui);

        formatter.Serialize(file, data);
        file.Close();

        Debug.Log("Game Saved at: " + Application.persistentDataPath);
        Debug.Log("Health: " + data.health);
        Debug.Log("Score: " + data.score);
        Debug.Log("Time: " + data.time);
        Debug.Log("Position: x = " + PlayerBehavior.savePosition[0] + ", y = " + PlayerBehavior.savePosition[1] + ", z = " + PlayerBehavior.savePosition[2]);
    }

    public static PlayerData LoadGame()
    {
        string path = Application.persistentDataPath + "/gamesave.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Error finding load path");
            return null;
        }

    }

}

