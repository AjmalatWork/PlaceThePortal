using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform destination;
    GameObject ball;
    Rigidbody2D ballRb;

    private void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Portable");
        ballRb = ball.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Portable"))
        {
            if (Vector2.Distance(ball.transform.position, transform.position) > 0.3f)
            {
                // Get direction of Entry Portal
                Vector3 entryDirection = transform.up.normalized;

                // Get direction of Exit Portal
                Vector3 exitDirection = destination.transform.up.normalized;

                // Get direction of Ball at the time of entry
                Vector3 ballEntryDirection = ballRb.velocity.normalized;

                // Calculate direction of Ball at the time of exit
                Vector3 ballExitDirection = entryDirection + exitDirection + ballEntryDirection;

                // Change velocity of ball at exit by using the exit ball direction just calculated above
                ballRb.velocity = ballExitDirection * ballRb.velocity.magnitude;

                // Change position to exit portal position
                ball.transform.position = destination.transform.position + ball.transform.localScale.magnitude / 2 * exitDirection;

            }
        }
    }

}
