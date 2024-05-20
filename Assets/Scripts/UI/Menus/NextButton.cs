using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : BaseUIButton, IClickableUI
{
    public new void OnClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int nextSceneIndex = currentScene.buildIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("No next scene in build settings!");
        }
    }

}