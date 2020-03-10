using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public HubDoor[] doors;

    public Sprite lockedDoor;

    private void Start()
    {
        Time.timeScale = 1;

        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < doors.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                doors[i].locked = true;
                doors[i].GetComponent<SpriteRenderer>().sprite = lockedDoor;
            }
        }   
    }
}
