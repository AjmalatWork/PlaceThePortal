using System;
using UnityEngine;
using UnityEngine.UI;

public class BallController : BasePortable, IResetable
{
    [NonSerialized]public int starsCollected = 0;
    public RectTransform levelEndPanel;
    public RectTransform inLevelPanel;
    public Transform headTransform;

    bool endLevel = false;
    public float moveSpeed = 15f;
    public float rotateSpeed = 15f;
    SpriteRenderer bodyRenderer;
    GameObject body;
    public GameObject happyBody;

    private void Start()
    {
        body = headTransform.parent.gameObject;
        bodyRenderer = body.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (endLevel) 
        {
            Invoke(nameof(MoveHeadToBody),0.2f);
            base.StopMotion();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConstants.Goal))
        {            
            endLevel = true;
            inLevelPanel.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag(TagConstants.Collectible))
        {
            Star currentStar = collision.gameObject.GetComponent<Star>();
            if (currentStar.starRenderer.enabled)
            {
                currentStar.OnCollect(starsCollected);
                starsCollected++;                
            }
        }
    }

    public new void SetOriginalState()
    {
        base.SetOriginalState();
        Debug.Log("Stars Collected this run: " + starsCollected);
        starsCollected = 0;
    }

    void MoveHeadToBody()
    {        
        transform.position = Vector3.MoveTowards(transform.position, headTransform.position, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * rotateSpeed);
        Invoke(nameof(MakeHappy), 0.5f);       
    }

    void MakeHappy()
    {
        bodyRenderer.enabled = false;
        happyBody.SetActive(true);
        gameObject.SetActive(false);
        Invoke(nameof(CallLevelEnd), 1f);
    }

    void CallLevelEnd()
    {
        Image endImage = levelEndPanel.GetComponent<Image>();
        String imageName = FileConstants.End + starsCollected;
        endImage.sprite = Resources.Load<Sprite>(imageName);
        levelEndPanel.gameObject.SetActive(true);        
    }

}
