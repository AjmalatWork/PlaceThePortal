using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform PortalIcon;
    public static TutorialManager Instance { get; private set; }

    private Animator textAnimator;
    private int currentLevel;
    private GameObject[] portalPlaceholders;
    private PlayerController[] playerControllers;
    private bool pointArrow = false;
    private bool pointedToPlay = false;

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

    void OnEnable()
    {
        textAnimator = text.gameObject.GetComponent<Animator>();

        Scene currentScene = SceneManager.GetActiveScene();
        currentLevel = currentScene.buildIndex;
        text.text = TextConstants.TutorialText[currentLevel];
        text.rectTransform.anchoredPosition = TextVectorConstants.TutorialTextPos[currentLevel];

        StartCoroutine(FadeIn());

        if(ArrowMover.Instance == null)
        {
            return ;
        }

        if(PortalIcon.localScale == VectorConstants.PortalIconOffScale)
        {
            return;
        }

        switch (currentLevel)
        {
            case 1:
                ArrowMover.Instance.CallArrow(VectorConstants.PortalIconA,
                                              VectorConstants.PortalIconB,
                                              VectorConstants.PortalIconRotation,
                                              VectorConstants.PortalIconScale);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if(!pointedToPlay)
        {
            CheckPortalPlaced();                       

            switch (currentLevel)
            {
                case 1:
                    if(pointArrow && ArrowMover.Instance != null)
                    {
                        ArrowMover.Instance.CallArrow(VectorConstants.playButtonA,
                                                      VectorConstants.playButtonB,
                                                      VectorConstants.playButtonRotation,
                                                      VectorConstants.DefaultScale);
                        pointedToPlay = true;
                    }                
                    break;
                default :
                    break;
            }
        }
    }

    void CheckPortalPlaced()
    {
        portalPlaceholders = GameObject.FindGameObjectsWithTag(TagConstants.PortalPlaceholder);
        int numberOfPortals = portalPlaceholders.Length;
        if (numberOfPortals > 0)
        {
            playerControllers = new PlayerController[numberOfPortals];
            for (int i = 0; i < numberOfPortals; i++)
            {
                playerControllers[i] = portalPlaceholders[i].GetComponent<PlayerController>();
                if (playerControllers[i].isPortalPlaced)
                {
                    pointArrow = true;
                }
                else
                {
                    pointArrow = false;
                    break;
                }
            }
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
