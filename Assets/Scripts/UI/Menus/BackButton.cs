using UnityEngine;

public class BackButton : BaseUIButton, IClickableUI
{
    public RectTransform InLevelPanel;

    public new void OnClick()
    {
        transform.parent.parent.gameObject.SetActive(false);
        InLevelPanel.gameObject.SetActive(true);
        if(!PlayButtonController.Instance.isPlay)
        {
            Time.timeScale = 1;
        }        
    }

}
