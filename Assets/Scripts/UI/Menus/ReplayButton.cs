using UnityEngine.SceneManagement;

public class ReplayButton : BaseUIButton, IClickableUI
{
    public new void OnClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

}

