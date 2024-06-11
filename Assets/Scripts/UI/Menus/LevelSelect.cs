using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : BaseUIButton, IClickableUI
{
    [NonSerialized] public bool levelUnlocked = false;

    private int currentLevel;
    private TextMeshProUGUI buttonText;
    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();        
        if (int.TryParse(buttonText.text, out int levelBuildIndex))
        {
            currentLevel = levelBuildIndex;
        }
        else
        {
            Debug.LogError("Level Name does not correspond to a Build Index");
        }        
    }

    private void Start()
    {
        GetLockStatus();
        SetTransparency();
    }

    public new void OnClick()
    {
        if(levelUnlocked)
        {
            SceneManager.LoadScene(currentLevel);
        }        
    }

    void GetLockStatus()
    {        
        int prevLevel = currentLevel - 1;
        if (prevLevel == 0)
        {
            levelUnlocked = true;
        }
        else
        {
            int stars = PlayerProgress.GetStarsForLevel(prevLevel);
            if (stars != 0)
            {
                levelUnlocked = true;
            }
            else
            {
                levelUnlocked = false;
            }
        }

        // For levels 6, 11, 16 and so on
        if ((currentLevel - 1) % 5 == 0)
        {
            int CollectedStars = PlayerProgress.GetTotalStars();
            int TotalStars = (currentLevel - 1) * 3;
            if (CollectedStars >= 0.6 * TotalStars)
            {
                levelUnlocked = true;
            }
            else
            {
                levelUnlocked = false;
            }
        }
    }

    void SetTransparency()
    {
        Color color = buttonImage.color;
        if (levelUnlocked)
        {
            color.a = ValueConstants.alphaOpaque;
        }
        else
        {
            color.a = ValueConstants.alphaTransparent;
        }
        buttonImage.color = color;
    }

}