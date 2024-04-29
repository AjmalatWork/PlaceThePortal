using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Button> buttons;
    private List<IClickableUI> clickableUIObjects = new();

    void Start()
    {
        FindClickableUIObjects();
        AddListenerToObjects();
    }

    void FindClickableUIObjects()
    {
        clickableUIObjects.Clear();
        clickableUIObjects.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IClickableUI>());
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
