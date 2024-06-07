using UnityEngine;
using UnityEngine.UI;

public class PortalIconController : BaseUIButton, IClickableUI
{
    public GameObject portalX;
    public GameObject portalY;
    public Vector3 portalXPosition;
    public Vector3 portalYPosition;

    Button button;
    Image PIImage;
    RectTransform PIRectTransform;

    private void Awake()
    {
        PIImage = GetComponent<Image>();
        button = GetComponent<Button>();
        PIRectTransform = GetComponent<RectTransform>();
    }

    public new void OnClick()
    {
        if(ArrowMover.Instance != null)
        {
            ArrowMover.Instance.DisableArrow();
        }

        // Deactivate portal icon on click and create two portal placeholders on screen
        PIRectTransform.localScale = VectorConstants.PortalIconOffScale;        
        PIImage.raycastTarget = false;
        PIImage.enabled = false;
        button.interactable = false;        

        Instantiate(portalX, portalXPosition, portalX.transform.rotation);
        Instantiate(portalY, portalYPosition, portalY.transform.rotation);
    }


}
