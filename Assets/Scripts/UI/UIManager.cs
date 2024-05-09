using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// UIManager is singleton as there can be only one in each scene
public class UIManager : MonoBehaviour
{
    public List<Button> buttons;
    private List<IClickableUI> clickableUIObjects = new();

    public static UIManager Instance { get; private set; }

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

    public void RegisterClickableObject(IClickableUI clickableObject)
    {
        clickableUIObjects.Add(clickableObject);
    }

    public void UnregisterClickableObject(IClickableUI clickableObject)
    {
        clickableUIObjects.Remove(clickableObject);
    }

    void Start()
    {
        AddListenerToObjects();
    }

    void AddListenerToObjects()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    private void OnButtonClick(Button button)
    {
        foreach (IClickableUI clickable in clickableUIObjects)
        {
            if (clickable is MonoBehaviour monoBehaviour && monoBehaviour.gameObject == button.gameObject)
            {
                clickable.OnClick();
                break;
            }
        }
    }
}
