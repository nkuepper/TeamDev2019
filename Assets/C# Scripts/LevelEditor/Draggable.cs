using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector2 mousePosition;
    private float deltaX, deltaY;
    private bool missingFloating;
    private Floating floatingScript;
    private bool editorModeOn;
    private GameObject player;

    private void Start()
    {
        // Finds player and sees if editor mode is on
        player = GameObject.FindGameObjectWithTag("Player");
        editorModeOn = player.GetComponent<PlayerBehavior>().editorModeOn;
    }

    public void OnMouseDown()
    {
        if (editorModeOn && player.GetComponent<EditorGUI>().toggleDrag)
        {
            // If the object being dragged is  floating (like the health drop)
            // The Floating script is temporarily removed
            if (!missingFloating && gameObject.GetComponent<Floating>())
            {
                floatingScript = gameObject.GetComponent<Floating>();
                Destroy(floatingScript);
                missingFloating = true;
            }

            // Pulls object to the cursor
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }
    }

    private void OnMouseDrag()
    {
        if (editorModeOn && player.GetComponent<EditorGUI>().toggleDrag)
        {
            // Object follows the cursor while being dragged
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);
        }
    }

    private void OnMouseUp()
    {
        if (editorModeOn && player.GetComponent<EditorGUI>().toggleDrag)
        {
            // Object is released
            // Snap to grid
            if (gameObject.tag != "Spring")
            {
                transform.position = new Vector3(Mathf.Round(mousePosition.x - deltaX), Mathf.Round(mousePosition.y - deltaY), 1);
            }
            else
            {
                transform.position = new Vector3(Mathf.Round(mousePosition.x - deltaX), Mathf.Round(mousePosition.y - deltaY) - 0.5f, 1);
            }

            if (missingFloating)
            {
                // Adds floating back
                gameObject.AddComponent<Floating>();
                missingFloating = false;
            }
        }
    }
}
