using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Enemy Kill spot
    public CapsuleCollider2D killSpot;

    // Alive check because the Player was technically killing the same enemy twice
    private bool alive = true;

    public Animator animator;
    internal Transform tr;

    // Player Detection
    public CapsuleCollider2D detectionRange;
    public bool seesPlayer;
    public Transform target;
    public float speed;

    internal virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        speed = 4f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        seesPlayer = false;
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    internal virtual void FixedUpdate()
    {
        if (seesPlayer)
        {
            transform.position = Vector2.MoveTowards
                (transform.position, target.position, speed * Time.deltaTime);

            animator.SetFloat("hMove", 1f);
        }
        else
        {
            animator.SetFloat("hMove", 0f);
        }
    }

    // Trigger for collider
    internal void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

            // Player kills
            if (player.footTrigger.IsTouching(this.gameObject.GetComponent<CircleCollider2D>())
                || player.footTrigger.IsTouching(this.gameObject.GetComponent<CapsuleCollider2D>()))
            {
                // If the player's footTrigger hits the enemy
                if (player != null && alive == true)
                {
                    if (player.isThereUI)
                        UIElements.instance.UpdateScore(50);

                    // Play particle system
                    player.gameObject.GetComponentsInChildren<ParticleSystem>()[1].Play();

                    player.jumping = true;
                    alive = false;

                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    internal void OnTriggerStay2D(Collider2D collision)
    {
        // Sees Player
        if (detectionRange.IsTouching(collision))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                seesPlayer = true;
            }
        }
    }

    // Player out of range
    internal void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            seesPlayer = false;
        }
    }
}
