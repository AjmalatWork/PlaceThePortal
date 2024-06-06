using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public Vector3 arrowRotation;
    public float speed = 1.0f;

    private Vector3 target;
    private bool movingToB;

    void OnEnable()
    {
        transform.position = pointA;
        transform.rotation = Quaternion.Euler(arrowRotation);
        target = pointB;
        movingToB = true;
    }

    void Update()
    {
        if(PlayButtonController.Instance.isPlay)
        {
            gameObject.SetActive(false);
        }
        // Move towards the target point
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);

        // Check if the arrow reached the target point
        if (transform.position == target)
        {
            // Switch the target point
            if (movingToB)
            {
                target = pointA;
            }
            else
            {
                target = pointB;
            }
            movingToB = !movingToB;
        }
    }
}
