using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : BaseUIButton, IClickableUI
{
    private TextMeshProUGUI buttonText;
    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public new void OnClick()
    {
        int levelBuildIndex;
        if(int.TryParse(buttonText.text, out levelBuildIndex))
        {
            SceneManager.LoadScene(levelBuildIndex);
        }
        else
        {
            Debug.LogError("Level Name does not correspond to a Build Index");
        }
    }

}