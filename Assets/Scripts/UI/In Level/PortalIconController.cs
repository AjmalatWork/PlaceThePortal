using UnityEngine;

public class PortalIconController : BaseUIButton, IClickableUI
{
    public GameObject portalX;
    public GameObject portalY;
    public Vector3 portalXPosition;
    public Vector3 portalYPosition;

    public new void OnClick()
    {
        // Deactivate portal icon on click and create two portal placeholders on screen
        gameObject.SetActive(false);
        Instantiate(portalX, portalXPosition, portalX.transform.rotation);
        Instantiate(portalY, portalYPosition, portalY.transform.rotation);
    }


}
