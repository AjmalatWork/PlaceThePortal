using System;
using UnityEngine;

public class BallController : BasePortable, IResetable
{
    [NonSerialized]public float starsCollected = 0;
    public RectTransform levelEndPanel;
    public RectTransform inLevelPanel;
    public Transform headTransform;

    bool endLevel = false;
    float elapsedTime = 0f;
    float duration = 2.0f; // Time it takes to move from A to B

    private void Update()
    {
        if (endLevel) 
        {
            MoveHeadToBody();
            base.StopMotion();
            //Invoke(nameof(CallLevelEnd), 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the ball touches the goal, you win!
        if (collision.gameObject.CompareTag(TagConstants.Goal))
        {            
            endLevel = true;
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

    void MoveHeadToBody()
    {        
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        transform.position = Vector3.Lerp(headTransform.position, transform.position, t);
    }

    void CallLevelEnd()
    {        
        inLevelPanel.gameObject.SetActive(false);
        levelEndPanel.gameObject.SetActive(true);        
    }

}
