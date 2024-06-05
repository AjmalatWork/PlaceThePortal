using UnityEngine;

public class BasePortable : MonoBehaviour, IResetable
{
    Vector3 originalPosition;
    Vector3 originalVelocity;
    float originalAngVelocity;
    float originalInertia;
    Quaternion originalRotation;
    Rigidbody2D objRb;

    public TerminalVelocitySettings terminalVelocitySettings;
    float terminalVelocity;

    private void Awake()
    {
        objRb = GetComponent<Rigidbody2D>();
        terminalVelocity = terminalVelocitySettings.TerminalVelocity;
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
        if (objRb.velocity.magnitude > terminalVelocity)
        {
            objRb.velocity = objRb.velocity.normalized * terminalVelocity;
        }
    }

    public void GetOriginalState()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalVelocity = objRb.velocity;
        originalAngVelocity = objRb.angularVelocity;
        originalInertia = objRb.inertia;
    }

    public void SetOriginalState()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        objRb.velocity = originalVelocity;
        objRb.angularVelocity = originalAngVelocity;
        objRb.inertia = originalInertia;
    }

    public void StopMotion()
    {
        RigidbodyConstraints2D currentConstraints = objRb.constraints;
        objRb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
