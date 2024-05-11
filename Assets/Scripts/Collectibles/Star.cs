using UnityEngine;

public class Star : MonoBehaviour, IResetable, ICollectible
{
    Vector3 originalPosition;
    Renderer starRenderer;

    private void Awake()
    {
        starRenderer = gameObject.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        PlayButtonController.Instance.RegisterResetableObject(this);
    }

    private void OnDisable()
    {
        PlayButtonController.Instance.UnregisterResetableObject(this);
    }

    public void GetOriginalState()
    {        
        originalPosition = transform.position;
    }

    public void SetOriginalState()
    {
        transform.position = originalPosition;
        starRenderer.enabled = true;
    }

    public void OnCollect()
    {
        starRenderer.enabled = false;
    }
}
