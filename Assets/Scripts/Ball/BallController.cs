using System;
using UnityEngine;

public class BallController : MonoBehaviour, IResetable
{
    public float terminalVelocity; 
    private Rigidbody2D ballRb;
    [NonSerialized]public float starsCollected = 0;

    Vector3 originalPosition;
    Vector3 originalVelocity;
    Quaternion originalRotation;
    private void Awake()
    {
        ballRb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PlayButtonController.Instance.RegisterResetableObject(this);
    }

    private void OnDisable()
    {
        PlayButtonController.Instance.UnregisterResetableObject(this);
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
        if (collision.gameObject.CompareTag(TagConstants.Goal))
        {
            Debug.Log("You win!");
            Time.timeScale = 0f;
        }

        if (collision.gameObject.CompareTag(TagConstants.Collectible))
        {
            starsCollected ++;
            collision.gameObject.GetComponent<Star>().OnCollect();
        }
    }

    public void GetOriginalState()
    {
        // Get original position of ball and velocity
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalVelocity = ballRb.velocity;

    }

    public void SetOriginalState()
    {
        // Set original position and velocity of ball
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        ballRb.velocity = originalVelocity;
        Debug.Log("Stars Collected this run: " + starsCollected);
        starsCollected = 0f;
    }
}
