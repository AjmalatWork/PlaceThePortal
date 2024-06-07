using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static TutorialManager Instance { get; private set; }

    private Animator textAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }         
    }

    void Start()
    {
        textAnimator = text.gameObject.GetComponent<Animator>();

        Scene currentScene = SceneManager.GetActiveScene();
        int currentLevel = currentScene.buildIndex;
        text.text = TextConstants.TutorialText[currentLevel];
        text.rectTransform.anchoredPosition = TextVectorConstants.TutorialTextPos[currentLevel];

        StartCoroutine(FadeIn());

        switch (currentLevel)
        {
            case 1: 

                break;
        }
    }

    public void TextFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        textAnimator.SetBool(AnimationConstants.FadeIn, true);
        textAnimator.SetBool(AnimationConstants.FadeOut, false);
    }

    IEnumerator FadeOut()
    {
        textAnimator.SetBool(AnimationConstants.FadeIn, false);
        textAnimator.SetBool(AnimationConstants.FadeOut, true);
        yield return null;
    }

}
