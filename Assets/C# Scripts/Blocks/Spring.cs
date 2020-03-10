using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public Animator animator;

    private bool sprung;

    private void Start()
    {
        sprung = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && sprung)
        {
            sprung = false;
            animator.SetBool("landedOn", false);
        }
    }

    void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !sprung)
        {
            PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

            // Player kills
            if (player.footTrigger.IsTouching(this.gameObject.GetComponent<CapsuleCollider2D>()))
            {
                // If the player's footTrigger hits the enemy
                if (player != null)
                {
                    player.Jump(player.jumpSpeed * 1.5f);
                    sprung = true;
                    animator.SetBool("landedOn", true);
                }
            }
        }
    }
}
