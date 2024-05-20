using UnityEngine;

public class ExitButton : BaseUIButton, IClickableUI
{
    public new void OnClick()
    {
        Application.Quit();
    }
}
