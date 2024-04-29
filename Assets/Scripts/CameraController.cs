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
            // Perform a raycast from the mouse position in z direction
            // If no collider is hit, then only move the camera
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (hit.collider == null)
            {
                Vector3 mouseWorldPositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                originX = mouseWorldPositon.x;
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float newPositionX = Mathf.Clamp(transform.position.x + originX - Camera.main.ScreenToWorldPoint(Input.mousePosition).x,minX,maxX);
            Vector3 targetPosition = new Vector3(newPositionX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(targetPosition, transform.position, Time.deltaTime * speed);
        }

    }

    private void MoveCamWithBall()
    {
        float newPositionX = Mathf.Clamp(ball.transform.position.x, minX, maxX);
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);        
    }
}
