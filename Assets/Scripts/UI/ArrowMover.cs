using UnityEngine;

public class ArrowMover : MonoBehaviour
{    
    public Vector3 pointA;
    public Vector3 pointB;
    public Vector3 arrowRotation;
    public float speed = 50f;

    private RectTransform arrowPos;
    private Vector3 target;
    private bool movingToB;

    private void Awake()
    {
        arrowPos = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        arrowPos.anchoredPosition = pointA;
        arrowPos.rotation = Quaternion.Euler(arrowRotation);
        target = pointB;
        movingToB = true;
    }

    void Update()
    {
        if (PlayButtonController.Instance.isPlay)
        {
            gameObject.SetActive(false);
            return;
        }

        MoveToAndFro();
    }

    void MoveToAndFro()
    {
        arrowPos.anchoredPosition = Vector2.MoveTowards(arrowPos.anchoredPosition, target, speed * Time.fixedDeltaTime);

        if ((Vector3)(arrowPos.anchoredPosition) == target)
        {
            target = movingToB ? pointA : pointB;
            movingToB = !movingToB;
        }
    }
}
