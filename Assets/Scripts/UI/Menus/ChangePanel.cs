using UnityEngine;

public class ChangePanel : BaseUIButton, IClickableUI
{
    public RectTransform currentPanel;
    public RectTransform nextPanel;

    public new void OnClick()
    {
        currentPanel.gameObject.SetActive(false);
        nextPanel.gameObject.SetActive(true);
    }

}
