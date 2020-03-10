using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorSetup : MonoBehaviour
{
    private GameObject player;
    
    void Start()
    {
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerBehavior>().editorModeOn = true;
            player.GetComponent<EditorGUI>().enabled = true;
    }
}
