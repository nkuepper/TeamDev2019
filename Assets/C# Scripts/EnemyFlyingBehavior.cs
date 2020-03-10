using UnityEngine;

public class EnemyFlyingBehavior : EnemyBehavior
{
    float startingPositionY;
    public float divingDistance;
    public bool diving, rising;

    float amplitude = 0.25f;
    Vector3 initialPosition = new Vector3();
    Vector3 newPosition = new Vector3();

    internal override void Start()
    {
        // Initialize variables
        diving = false;
        rising = false;
        speed = 10f;
        divingDistance = 7f;
        tr = GetComponent<Transform>();
        initialPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        startingPositionY = transform.position.y;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        seesPlayer = false;
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // Floating up and down
        if (!seesPlayer && !diving && !rising)
        {
            // Floating up and down
            newPosition = initialPosition;
            newPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI) * amplitude;

            transform.position = newPosition;
        }
    }

    internal override void FixedUpdate()
    {
        // Dive when player enters range
        if (seesPlayer && !diving && !rising)
        {
            diving = true;
            startingPositionY = transform.position.y;
        }

        // Diving
        // Enemy goes straight down
        if (diving)
        {
            transform.position = Vector2.MoveTowards
               (transform.position, new Vector2(transform.position.x, startingPositionY - divingDistance), speed * Time.deltaTime);

            // Enemy reaches end of dive and starts rising
            if (Mathf.Approximately(transform.position.y, startingPositionY - divingDistance))
            {
                diving = false;
                rising = true;
                animator.SetBool("PlayerSeen", true);
            }
        }

        // Rising
        // Goes back up to starting y level at 1/2 speed
        if (rising)
        {
            transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(transform.position.x, startingPositionY), speed / 2 * Time.deltaTime);

            if (Mathf.Approximately(transform.position.y, startingPositionY))
            {
                rising = false;
                animator.SetBool("PlayerSeen", false);
            }
        }
    }
}
