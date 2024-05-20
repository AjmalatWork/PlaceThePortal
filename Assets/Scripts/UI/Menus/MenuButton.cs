using UnityEngine.SceneManagement;

public class MenuButton : BaseUIButton, IClickableUI
{
    public new void OnClick()
    {
        SceneManager.LoadScene(0);
    }
}