using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float minX; 
    public float maxX;
    public float speed = 15f;
    public Button playButton;
    public GameObject ball;

    bool isDragging = false;
    Vector3 targetPosition;
    float originX;
    PlayButtonController playButtonController;

    private void Awake()
    {
        playButtonController = playButton.GetComponent<PlayButtonController>();
    }

    private void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (playButtonController.isPlay)
        {
            MoveCamOnDrag();
        }
        else
        {
            MoveCamWithBall();
        }
    }

    private void MoveCamOnDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            originX = mouseWorldPositon.x;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            // Get new postion of object at each frame it is being dragged
            float newPositionX = Mathf.Clamp( 2 * originX - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, minX, maxX);
            targetPosition = new Vector3(newPositionX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(targetPosition, transform.position, Time.fixedDeltaTime * speed);
            Debug.Log(originX);
            Debug.Log(newPositionX);
        }

    }

    private void MoveCamWithBall()
    {
        if (ball.transform.position.x >= minX && ball.transform.position.x <= maxX)
        {
            transform.position = new Vector3(ball.transform.position.x, transform.position.y, transform.position.z);
        }
        
    }
}
