using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlocks : MonoBehaviour
{  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();

        if (player != null)
        {
            Debug.Log("Break");
        }
    }
}
