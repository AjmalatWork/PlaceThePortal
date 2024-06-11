using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextButton : BaseUIButton, IClickableUI
{
    private Image buttonImage;
    private int currentLevel;
    private bool buttonClickable = true;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();        
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        currentLevel = currentScene.buildIndex;

        if(gameObject.name == NameConstants.NewGame)
        {
            return;
        }

        int stars = PlayerProgress.GetStarsForLevel(currentLevel);
        if (stars == 0)
        {
            buttonClickable = false;
            Utilities.SetTransparency(buttonImage, buttonClickable);        
        }        
    }

    public new void OnClick()
    {
        if(!buttonClickable)
        {
            return;
        }

        int nextSceneIndex = currentLevel + 1;                

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