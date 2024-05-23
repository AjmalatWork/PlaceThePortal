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

    private void Awake()
    {
        PIImage = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public new void OnClick()
    {
        // Deactivate portal icon on click and create two portal placeholders on screen
        PIImage.enabled = false;
        button.interactable = false;
        Instantiate(portalX, portalXPosition, portalX.transform.rotation);
        Instantiate(portalY, portalYPosition, portalY.transform.rotation);
    }


}
