using UnityEngine;
using UnityEngine.UI;

public class CanvasAligner : MonoBehaviour
{
    private Camera targetCamera;
    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        AdjustCanvasAspectRatio();
    }

    void AdjustCanvasAspectRatio()
    {
        if (targetCamera != null)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float targetAspect = targetCamera.aspect;
            Debug.Log(targetAspect);

            if (screenAspect >= targetAspect)
            {
                // Wider than target
                canvasScaler.matchWidthOrHeight = 1f; // Match height
            }
            else
            {
                // Taller than target
                canvasScaler.matchWidthOrHeight = 0f; // Match width
            }
        }
    }
}
