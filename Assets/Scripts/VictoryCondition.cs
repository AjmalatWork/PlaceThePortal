using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the ball touches the goal, you win!
        if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("You win!");
            Time.timeScale = 0f;
        }
    }
}
