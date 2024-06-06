using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour, IResetable
{
    public float minX; 
    public float maxX;
    public float speed = 15f;
    public Button playButton;    
    public GameObject ball;
    public RectTransform inLevelPanel;

    bool isDragging = false;
    bool shouldMoveWithPortal = false;
    float originX;
    PlayerController playerController;
    
    float camLeftX;
    float camRightX;
    Vector3 camDirection = Vector3.zero;
    Vector3 originalPosition;
    Vector3 targetPos;

    bool levelShown = false;
    bool movedBack = false;
    bool waited = false;
    readonly float startSpeed = 5f;

    private void OnEnable()
    {
        PlayButtonController.Instance.RegisterResetableObject(this);
    }

    private void OnDisable()
    {
        PlayButtonController.Instance.UnregisterResetableObject(this);
    }

    private void Start()
    {
        inLevelPanel.gameObject.SetActive(false);
        targetPos = new(maxX, transform.position.y, transform.position.z);
        StartCoroutine(StartUpdateAfterSeconds(0.5f));        
    }

    private void MoveCamAtoB(Vector3 a, Vector3 b, float speed)
    {   
        transform.position = Vector3.MoveTowards(a, b, speed * Time.fixedDeltaTime);
    }

    IEnumerator SetTrueAfterSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        levelShown = true;
    }

    IEnumerator StartUpdateAfterSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        waited = true;
    }

    void ShowLevel()
    {
        if (!levelShown)
        {
            MoveCamAtoB(transform.position, targetPos, startSpeed);
            if (transform.position == targetPos)
            {
                StartCoroutine(SetTrueAfterSeconds(1f));                
            }
        }

        if (levelShown)
        {            
            MoveCamAtoB(transform.position, originalPosition, startSpeed + 10);
            if (transform.position == originalPosition)
            {
                movedBack = true;
            }
        }
    }

    private void Update()
    {
        if (waited)
        {
            if (!movedBack)
            {
                ShowLevel();
            }

            if (movedBack)
            {
                if(!inLevelPanel.gameObject.activeSelf)
                {
                    inLevelPanel.gameObject.SetActive(true);
                }                    
                MoveCamera();
            }
        }                     
    }

    void MoveCamera()
    {
        if (PlayButtonController.Instance.isPlay)
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
            else
            {
                try
                {   
                    playerController = hit.collider.gameObject.GetComponent<PlayerController>();
                    if (playerController)
                    {
                        Vector3 mouseWorldPositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        originX = mouseWorldPositon.x;
                        shouldMoveWithPortal = true;               
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            shouldMoveWithPortal = false;
            camDirection = Vector3.zero;
        }

        if (isDragging)
        {
            float newPositionX = Mathf.Clamp(transform.position.x + originX - Camera.main.ScreenToWorldPoint(Input.mousePosition).x, minX, maxX);
            Vector3 targetPosition = new Vector3(newPositionX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(targetPosition, transform.position, Time.deltaTime * speed);
        }
        else if(shouldMoveWithPortal)
        {
            CalculateCameraBounds();
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > camLeftX && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < camLeftX + 1f)
            {
                camDirection = Vector3.left;
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > camRightX - 1f && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < camRightX)
            {
                camDirection = Vector3.right;
            }
            else
            {
                camDirection = Vector3.zero;
            }

            if(camDirection != Vector3.zero) 
            {   
                Vector3 tempCamPos = transform.position + camDirection.normalized * 0.2f;
                float newPositionX = Mathf.Clamp(tempCamPos.x, minX, maxX);
                Vector3 targetPosition = new Vector3(newPositionX, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(targetPosition, transform.position, Time.deltaTime * speed);
            }
        }

    }

    private void MoveCamWithBall()
    {
        float newPositionX = Mathf.Clamp(ball.transform.position.x, minX, maxX);
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);        
    }

    private void CalculateCameraBounds()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, Camera.main.nearClipPlane));
        camLeftX = leftEdge.x;

        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane));
        camRightX = rightEdge.x;
    }

    public void GetOriginalState()
    {
        originalPosition = transform.position;
    }

    public void SetOriginalState()
    {
        transform.position = originalPosition;
    }
}
