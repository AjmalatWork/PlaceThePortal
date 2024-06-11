using UnityEngine;

public class BackButton : BaseUIButton, IClickableUI
{
    public RectTransform InLevelPanel;
    public RectTransform TutorialPanel;

    public new void OnClick()
    {
        transform.parent.parent.gameObject.SetActive(false);
        InLevelPanel.gameObject.SetActive(true);
        TutorialPanel.gameObject.SetActive(true);
        if(!PlayButtonController.Instance.isPlay)
        {
            Time.timeScale = 1;
        }        
    }

}
