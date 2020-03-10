using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float currentSpeed;
    public float runSpeed = 8f;
    public float jumpSpeed = 10f;
    public bool inAir = true;
    public bool isThereUI = true;
    public bool isInvincible;
    public int maxHealth = 10;
    public int currentHealth;
    public float timeInvincible = 3.0f;
    public EdgeCollider2D footTrigger;
    public EdgeCollider2D headTrigger;
    public GameObject enemyPrefab;
    public GameObject healthDropPrefab;
    public GameObject brickPiecePrefab;
    public GameObject wayPoint;
    public bool editorModeOn = false;
    public bool jumping = false;
    public bool grabbedKey = false;
    public bool disableTimer = false;
    // Don't assign anything to this
    public GameObject waypoint;

    private float hMove = 0f;
    private float jumphMove;
    private float invincibleTimer;
    private bool doubleJumpAbility = false;
    private bool canDoubleJump = true;
    private bool flipGravityAbility = false;
    private bool canFlipGravity = true;
    private bool flipped = false;
    private float keyWaypointTimer = .2f;
    

    Animator animator;
    SpriteRenderer sr;
    Rigidbody2D rb;
    Transform playerTransform;

    GameObject Player;

    //need to convert Vector3 to array to save
    public static float[] savePosition = { 0, 0, 0 };

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerTransform = gameObject.GetComponent<Transform>();

        sr.sortingOrder = 2;

        if (isThereUI)
        {
            // Set health
            currentHealth = maxHealth;
            UIElements.instance.UpdateHealth(currentHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hMove = Input.GetAxis("Horizontal");

        // User presses space to jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // If we are on the ground
            if (!inAir)
            {
                // jumping is when the player is obtaining upward velocity
                // inAir is when the player is not on the ground
                jumping = true;
                canDoubleJump = true;
            }
            // If we aren't on the ground and have the double jump ability
            else if (doubleJumpAbility)
            {
                // If we haven't used our double jump
                if (canDoubleJump && !flipped)
                {
                    // Double Jump has been used
                    canDoubleJump = false;

                    // Reset Y velocity to account for gravity
                    rb.velocity = new Vector2(rb.velocity.x, 0);

                    // Jump
                    Jump(jumpSpeed * 1.1f);
                }
                else if (canDoubleJump && flipped)
                {
                    // Double Jump has been used
                    canDoubleJump = false;

                    // Reset Y velocity to account for gravity
                    rb.velocity = new Vector2(rb.velocity.x, 0);

                    // Jump
                    Jump(jumpSpeed * -1.1f);
                }
            }
        }

        // If the user presses the E key with the gravity power up
        if (Input.GetKeyDown(KeyCode.E) && flipGravityAbility && canFlipGravity)
        {
            // Flip Gravity
            rb.gravityScale *= -1;
            // Flip y
            Vector2 newPos = new Vector2(transform.localScale.x, transform.localScale.y);
            newPos.y = newPos.y * -1;
            transform.localScale = newPos;
            // Set flipped to true if not flipped or set flipped to false if flipped
            flipped = (flipped == true) ? false : true;
            canFlipGravity = false;
        }

        if (isThereUI)
        {
            //update health from PlayerData, as it's static there.
            currentHealth = PlayerData.loadHealth;
        }

        if (hMove >= 0)
            sr.flipX = true;
        if (hMove < 0)
            sr.flipX = false;
        if (!jumping)
        {
            animator.SetFloat("hMove", Mathf.Abs(hMove));
        }
        else
        {
            animator.SetFloat("hMove", 0);
        }
    }

    void FixedUpdate()
    {
        if (jumping)
        {
            if (flipped)
                Jump(jumpSpeed * -1);
            else
                Jump(jumpSpeed);
        }
        else if (inAir)
        {
            if (Mathf.Abs(jumphMove - hMove) > 0.3)
            {
                rb.velocity = new Vector2(hMove * runSpeed * 0.7f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(hMove * runSpeed * 1.2f, rb.velocity.y);
            }
        }
        else if (!inAir)
        {
            // Mathf.Clamp(n1, n2, n3)
            rb.velocity = new Vector2(hMove * runSpeed, rb.velocity.y);
        }

        // Player grabbed the key
        if (grabbedKey == true)
        {
            // if the timer is above 0
            if (keyWaypointTimer > 0)
            {
                // Decrement the Timer
                keyWaypointTimer -= Time.deltaTime;
            }

            // If the timer is below or equal to 0
            if (keyWaypointTimer <= 0)
            {
                // Set the wayPoint position to the player's position
                waypoint.transform.position = transform.position;

                // Reset the timer
                keyWaypointTimer = .2f;
            }
        }

        // Invincibility
        if (isInvincible)
        {
            // Visibly show invincibilty
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;

            // Minus invincible timer
            invincibleTimer -= Time.deltaTime;

            // When Invincibility is done
            if (invincibleTimer < 0)
            {
                // Disable invincibility
                isInvincible = false;

                // Make sure the character is visible
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }

        currentSpeed = rb.velocity.x;

        if (isThereUI && !disableTimer)
        {
            // Decrement the game timer
            UIElements.instance.IncrementTime();


        }
        else if (isThereUI && disableTimer)
        {
            // Disable the Timer
                UIElements.instance.uiTimer.enabled = false;
                UIElements.instance.uiTimerLabel.enabled = false;
        }

        //Update the save position
        getPosition();
    }

    public void Jump(float jumpSpd)
    {
        rb.velocity = new Vector2(hMove * 15f, jumpSpd);
        jumping = false;
        jumphMove = hMove;
        animator.SetBool("landed", false);
    }

    // Update Health Method
    public void ChangeHealth(int amount)
    {
        // If damaged
        if (amount < 0)
        {
            // If invincible then leave method
            if (isInvincible)
            {
                return;
            }

            // If the player has a double jump power up and gets hit
            if (doubleJumpAbility)
            {
                doubleJumpAbility = false;
            }

            // Turn invincible on
            InvincibilityOn(timeInvincible);

            // Play particle system
            gameObject.GetComponentsInChildren<ParticleSystem>()[3].Play();
        }
        
        // Update current health
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        // Check to see if player is still alive
        if (isThereUI)
        {
            if(currentHealth > 0)
            {
                //Debug.Log(currentHealth + "/" + maxHealth);
                UIElements.instance.UpdateHealth(currentHealth);
            }
            else
            {
                Debug.Log("Game Over!");
                UIElements.instance.UpdateHealth(currentHealth);
            }
        }
        
       
    }

    public void InvincibilityOn(float timeInvincible)
    {
        isInvincible = true;
        invincibleTimer = timeInvincible;
    }

    // Edge Collision is set to trigger and is on the bottom of the player so the edge collision will fire off the trigger events when the platform class touches it
    // Using the Stay and Exit methods, I got rid of setting the inAir bool to false when someone presses jump and then we won't need the if statement about negative velocity
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Touching the ground
        if (collision != null && footTrigger.IsTouching(collision))
        {
            if (!collision.isTrigger)
            {
                inAir = false;
                canFlipGravity = true;
                animator.SetBool("landed", true);
            }
        }

        // Head trigger is touching a something
        if (headTrigger.IsTouching(collision))
        {
            // Get collider's transform
            Transform blockTransform;
            blockTransform = collision.gameObject.GetComponent<Transform>();

            // A breakable block
            if (collision.gameObject.CompareTag("Break"))
            {
                // Individual bricks to fall
                for (int index = 0; index < 5; index++)
                {
                    gameObject.GetComponentsInChildren<ParticleSystem>()[0].Play();
                }
                
                collision.gameObject.SetActive(false);
            }

            // A hidden block
            // Will only appear if the player jumps to hit it
            if (collision.gameObject.CompareTag("Hidden") && gameObject.GetComponent<Rigidbody2D>().velocity.y >= -.5)
            {
                // Reveals the hidden block
                collision.gameObject.GetComponent<HiddenBlock>().RevealHiddenBlock();
            }

            // A mystery block
            if (collision.gameObject.CompareTag("Mystery"))
            {
                // Generate a random 0-2 int
                int rand;
                rand = Random.Range(0, 3);

                SpriteRenderer blockRenderer;
                blockRenderer = collision.gameObject.GetComponent<SpriteRenderer>();

                // Only fire if the block hasn't been used
                if (blockRenderer.color != Color.gray)
                {
                    if (rand == 0)
                    {
                        // Enemy pops out
                        var enemy = Instantiate(enemyPrefab, blockTransform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    }
                    else if (rand == 1)
                    {
                        // Health pops out
                        Instantiate(healthDropPrefab, blockTransform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    }
                    else
                    {
                        // Score increase pops out
                        UIElements.instance.UpdateScore(100);

                        // Play particle system
                        gameObject.GetComponentsInChildren<ParticleSystem>()[3].Play();
                    }

                    blockRenderer.color = Color.gray;
                }
            }
        }
    }

    // When the player is not touching the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Leaving the ground
        inAir = true;
    }

    // When the player is touching a damaging object
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            ChangeHealth(-1);
        }
    }

    // When the player grabs the double jump powerup
    public void DoubleJump(GameObject powerup)
    {
        // If they dont have the power up
        if (doubleJumpAbility == false)
        {
            // Destroy the powerup
            Destroy(powerup);

            // Give them the ability to double jump
            doubleJumpAbility = true;

            // TODO? Modify the player or indicate in the UI that they have a power up
        }
        // If they can already double jump
        else
        {
            // If the UI is active
            if (isThereUI)
            {
                // Destroy the power up
                Destroy(powerup);

                // Update the score by 100
                UIElements.instance.UpdateScore(100);
            }
        }
    }

    // When the player grabs a key
    public void PlayerGrabbedKey()
    {   
        if (grabbedKey == false)
        {
            // Create the wayPoint at the player's position
            waypoint = Instantiate(wayPoint, transform.position, Quaternion.identity);
        }
        // Set off the key flag
        grabbedKey = true;
    }

    // When player has the Gravity Powerup
    public void FlipGravity()
    {
        // Give them the ability to flip gravity
        flipGravityAbility = true;
    }

    // get postion sets the savePosition array to pass to PlayerData to save
    void getPosition()
    {
        savePosition[0] = rb.position.x;
        savePosition[1] = rb.position.y;
    }

    public void setPosition(float[] position)
    {
        Player = GameObject.Find("Player");
        Vector2 pos;
        pos.x = position[0];
        pos.y = position[1];
        Player.transform.position = pos;
    }


}
