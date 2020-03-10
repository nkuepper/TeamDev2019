using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public EdgeCollider2D sideOfPowerUp;

    private Rigidbody2D rb;
    private Vector2 moveTo;
    private int direction;

    private void Start()
    {
        moveTo = new Vector2(100000, 2);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveTo, 2f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Switch");
        moveTo.x *= -1;
    }
}
