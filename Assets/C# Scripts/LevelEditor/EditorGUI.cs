using UnityEngine;

public class EditorGUI : MonoBehaviour
{
    // Need both an image and prefab for each object on the menu
    private Rect windowRect = new Rect(20, 20, 1140, 95);
    public Sprite[] textures;
    public Object[] objects;
    Vector3 playerPos;
    public bool toggleAI = true;
    public bool toggleDrag = true;

    private GameObject player;

    void OnGUI()
    {
        // Finds player and adds menu to GUI
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.GetComponent<Transform>().position;
        windowRect = GUI.Window(0, windowRect, DoMyWindow, "Editor Menu");
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        int windowX = 10;

        for (int i = 0; i < textures.Length; i++)
        {
            // Each texture and object statement makes a button
            if (GUI.Button(new Rect(windowX + i * 100, 20, 100, 60), textureFromSprite(textures[i])))
            {
                InstantiateObject(objects[i]);
            }
        }

        toggleAI = GUI.Toggle(new Rect(windowX + textures.Length * 100, 20, 100, 20), toggleAI, "Enemy AI");
        toggleDrag = GUI.Toggle(new Rect(windowX + textures.Length * 100, 40, 100, 20), toggleDrag, "Dragging");

        if (GUI.Button(new Rect(windowX + textures.Length * 100, 60, 100, 20), "Reset Player"))
        {
            // Reset score and health and timer
            player.GetComponent<PlayerBehavior>().ChangeHealth(10);
            UIElements.instance.UpdateScore(int.Parse(UIElements.instance.uiScore.text) * -1);
            UIElements.instance.ResetTime();
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    private void FixedUpdate()
    {
        // Shuts off basic enemy player tracking if toggled
        if (!toggleAI)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Damage"))
            {
                if (!g.name.Contains("Variant") && g.name.Contains("Enemy"))
                    g.GetComponent<EnemyBehavior>().seesPlayer = false;
            }
        }
    }

    // Creates instance of button clicked
    public void InstantiateObject(Object prefab)
    {
        if (prefab.name != "Spring")
        {
            Instantiate(prefab, new Vector3(Mathf.Round(playerPos.x), Mathf.Round(playerPos.y + 2f), 0f), Quaternion.identity);
        }
        else
        {
            Instantiate(prefab, new Vector3(Mathf.Round(playerPos.x), Mathf.Round(playerPos.y + 1.5f), 0f), Quaternion.identity);
        }
    }

    // Convert sprite to texture2D
    // For the buttons
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}