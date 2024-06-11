using UnityEngine;
using UnityEngine.UI;

public class Utilities : MonoBehaviour
{
    public static void SetTransparency(Image image, bool flag)
    {
        Color color = image.color;
        if (flag)
        {
            color.a = ValueConstants.alphaOpaque;
        }
        else
        {
            color.a = ValueConstants.alphaTransparent;
        }
        image.color = color;
    }
}
