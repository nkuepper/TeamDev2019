using UnityEngine;
using UnityEngine.UI;

public class FlagFixer : MonoBehaviour
{
    EndLevel endScript;
    Canvas endUI;
    Text finalScore;
    Text timeRemaining;

    void Awake()
    {
        endScript = gameObject.GetComponent<EndLevel>();
        endUI = GameObject.Find("LevelEndUI").GetComponent<Canvas>();
        finalScore = GameObject.Find("Final Score").GetComponent<Text>();
        timeRemaining = GameObject.Find("txtTimeRemaining").GetComponent<Text>();

        endScript.endUI = endUI;
        endScript.finalScore = finalScore;
        endScript.timeToFinish = timeRemaining;
    }
}
