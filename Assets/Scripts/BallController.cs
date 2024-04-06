using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float terminalVelocity; // Maximum velocity limit (m/s)
    private Rigidbody2D ballRb;
    [NonSerialized]public float starsCollected = 0;

    private void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Limit velocity to terminal velocity
        if (ballRb.velocity.magnitude > terminalVelocity)
        {
            ballRb.velocity = ballRb.velocity.normalized * terminalVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the ball touches the goal, you win!
        if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("You win!");
            Time.timeScale = 0f;
        }

        // When the ball touches the goal, you win!
        if (collision.gameObject.CompareTag("Collectible"))
        {
            starsCollected ++;
            collision.gameObject.SetActive(false);
            Debug.Log("Stars collected: " + starsCollected);
        }
    }
}
