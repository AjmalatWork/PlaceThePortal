using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    public float speed = 40f;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 arrowRotation;
    private Vector3 arrowScale;    
    private RectTransform arrowPos;
    private Vector3 target;
    private bool movingToB;

    public static ArrowMover Instance { get; private set; }

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

        arrowPos = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        arrowPos.anchoredPosition = pointA;
        arrowPos.rotation = Quaternion.Euler(arrowRotation);
        arrowPos.localScale = arrowScale;
        target = pointB;
        movingToB = true;
    }

    void Update()
    {
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

    public void CallArrow(Vector3 A, Vector3 B, Vector3 rotation, Vector3 scale)
    {
        pointA = A;
        pointB = B;
        arrowRotation = rotation;
        arrowScale = scale;
        gameObject.SetActive(true);
    }

    public void DisableArrow()
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }        
    }
}
