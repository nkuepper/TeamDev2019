using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadTutorialMenuSelector()
    {
        Application.LoadLevel(2);
    }

    public void LoadCreateYourOwnLevel()
    {
        Application.LoadLevel(7);
    }

    public void LoadTestLevel()
    {
        Application.LoadLevel(1);
    }
}
