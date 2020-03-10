using UnityEngine;

public class followPlayer : MonoBehaviour
{
    float alt;
    float maxDist = 4f;
    Camera thisCamera;
    public Transform target;
    public float speed = 0.08f;
    Vector3 desiredPosition, position;
    public float offset = -10f;
    //need to convert Vector3 to array to save
    public static float[] savePosition = { 0, 0, 0 };

    private void Start()
    {
        thisCamera = gameObject.GetComponent<Camera>();

        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        desiredPosition = new Vector3(target.position.x, target.position.y, offset);
    }

    private void Update()
    {
        alt = Input.GetAxis("Fire2");
    }

    void FixedUpdate()
    {
        // If right mouse or alt are not pressed
        if (alt != 0)
        {
            // Sets camera to follow mouse
            desiredPosition = thisCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset));
            desiredPosition = new Vector3(
                Mathf.Clamp(desiredPosition.x, target.position.x - maxDist, target.position.x + maxDist),
                Mathf.Clamp(desiredPosition.y, target.position.y - maxDist + 2, target.position.y + maxDist - 2),
                offset);
        }
        else
        {
            // Camera follows player
            desiredPosition = new Vector3(target.position.x, target.position.y, offset);
        }
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed);
        getPosition();
    }

    //get postion sets the savePosition array to pass to PlayerData to save
    void getPosition()
    {
        savePosition[0] = target.position.x;
        savePosition[1] = target.position.y;
        savePosition[2] = target.position.z;
    }

}
