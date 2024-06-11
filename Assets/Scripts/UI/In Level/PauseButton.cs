using UnityEngine;

public class PauseButton : BaseUIButton, IClickableUI
{
    public RectTransform PausePanel;
    public RectTransform TutorialPanel;

    public new void OnClick()
    {
        transform.parent.parent.gameObject.SetActive(false);
        TutorialPanel.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

}