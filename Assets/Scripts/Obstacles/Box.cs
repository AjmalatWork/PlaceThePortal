using UnityEngine;

public class Box : MonoBehaviour, IResetable
{
    Vector3 originalPosition;
    Vector3 originalVelocity;
    Quaternion originalRotation;
    Rigidbody2D boxRb;

    private void Awake()
    {
        boxRb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PlayButtonController.Instance.RegisterResetableObject(this);
    }

    private void OnDestroy()
    {
        PlayButtonController.Instance.UnregisterResetableObject(this);
    }

    public void GetOriginalState()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalVelocity = boxRb.velocity;
    }

    public void SetOriginalState()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        boxRb.velocity = originalVelocity;
    }
}
