using UnityEngine;

public class ChangePanel : BaseUIButton, IClickableUI
{
    public RectTransform currentPanel;
    public RectTransform nextPanel;

    public new void OnClick()
    {
        Debug.Log(currentPanel);
        Debug.Log(nextPanel);
        currentPanel.gameObject.SetActive(false);
        nextPanel.gameObject.SetActive(true);
    }

}
