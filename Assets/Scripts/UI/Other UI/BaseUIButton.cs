using UnityEngine;

public class BaseUIButton : MonoBehaviour, IClickableUI
{
    private void OnEnable()
    {
        UIManager.Instance.RegisterClickableObject(this);
    }
    private void OnDisable()
    {
        UIManager.Instance.UnregisterClickableObject(this);
    }

    public void OnClick()
    {
        // Implementation in child classes
        return;
    }
}