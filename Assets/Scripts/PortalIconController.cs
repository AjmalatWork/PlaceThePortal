using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalIconController : MonoBehaviour
{
    public GameObject portalX;
    public GameObject portalY;
    public Vector3 portalXPosition;
    public Vector3 portalYPosition;

    private void OnMouseDown()
    {
        // Deactivate portal icon on click and create two portal placeholders on screen
        gameObject.SetActive(false);
        Instantiate(portalX, portalXPosition, portalX.transform.rotation);
        Instantiate(portalY, portalYPosition, portalY.transform.rotation);
    }
}
