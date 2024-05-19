using System;
using UnityEngine;

public class BallController : BasePortable, IResetable
{
    [NonSerialized]public float starsCollected = 0;

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
            Star currentStar = collision.gameObject.GetComponent<Star>();
            if (currentStar.starRenderer.enabled)
            {
                starsCollected++;
                currentStar.OnCollect();
            }
        }
    }

    public new void SetOriginalState()
    {
        base.SetOriginalState();
        Debug.Log("Stars Collected this run: " + starsCollected);
        starsCollected = 0f;
    }
}