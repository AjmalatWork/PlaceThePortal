using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX; 
    public float maxX;
    public float speed = 15f;
    public Collider2D[] restrictCamOnColliders;

    float originX = 0f;
    bool isDragging = false;
    Vector3 targetPosition;
    PlayButtonController playButtonController;
    GameObject playButton;
    GameObject ball;

    private void Awake()
    {
        playButton = GameObject.FindGameObjectWithTag("PlayButton");
        ball = GameObject.FindGameObjectWithTag("Portable");
        playButtonController = playButton.GetComponent<PlayButtonController>();
    }

    private void Update()
    {
        if (playButtonController.isPlay)
        {
            move_cam_on_drag();        
        }
        else
        {
            move_cam_with_ball();
        }
    }

    private void move_cam_on_drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //// Check if the point is overlapping with each collider
            //foreach ( Collider2D collider in restrictCamOnColliders)
            //{
            //    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPositon, Vector3.forward, 11f);
            //    Debug.Log(collider);
            //    Debug.Log(mouseWorldPositon);
            //    Debug.Log(hit.collider);
            //    Debug.Log(Vector3.forward);
            //    if (hit.collider == collider)
            //    {
            //        return;
            //    }
            //}
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
            float newPositionX = 2 * originX - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            targetPosition = new Vector3(newPositionX, transform.position.y, transform.position.z);

            if (newPositionX >= minX && newPositionX <= maxX)
            {
                transform.position = Vector3.Lerp(targetPosition, transform.position, Time.fixedDeltaTime * speed);
            }
        }

    }

    private void move_cam_with_ball()
    {
        if (ball.transform.position.x >= minX && ball.transform.position.x <= maxX)
        {
            transform.position = new Vector3(ball.transform.position.x, transform.position.y, transform.position.z);
        }
        
    }
}
