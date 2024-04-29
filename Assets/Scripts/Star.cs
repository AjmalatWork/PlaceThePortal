using UnityEngine;

public class Star : MonoBehaviour, IResetable
{
    Vector3 originalPosition;
    public void GetOriginalState()
    {
        originalPosition = transform.position;
    }

    public void SetOriginalState()
    {
        transform.position = originalPosition;
        gameObject.SetActive(true);
    }
}
