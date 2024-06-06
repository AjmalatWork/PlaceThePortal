using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // Variables needed in inspector
    public LayerMask targetLayer;
    public LayerMask portalLayer;
    public float distanceToSnap;
    public GameObject portal;
    public string color;
    [NonSerialized] public bool isDragging = false;
    [NonSerialized] public bool isPortalPlaced = false;

    // Private
    Vector3 offset;
    GameObject placePortalAt;
    Vector3 portalDirection;
    Vector3 portalPosition;
    readonly float endThreshold = 0.2f;
    Vector3 portalEndA;
    Vector3 portalEndB;
    readonly float portalOffset = 0.25f;

    SpriteRenderer portalIconSR;
    float portalSizeX;
    float portalSizeY;
    Bounds portalBounds;
    Vector3 extents;

    Camera mainCamera;
    AudioSource placeAudio;

    private void Awake()
    {
        placeAudio = GetComponent<AudioSource>();
        portalIconSR = gameObject.GetComponent<SpriteRenderer>();

        portalBounds = portalIconSR.sprite.bounds;
        portalSizeX = portalBounds.size.x;
        portalSizeY = portalBounds.size.y;
        mainCamera = Camera.main;

        extents = portalBounds.extents;
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // This is to fix bug where clicking on UI button also registered click on portal if it was beneath it
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, portalLayer);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                // Calculate the offset between the GameObject's position and the mouse position
                offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);

                // Increase the scale of the object to show it was clicked
                transform.localScale *= 1.2f;
                isDragging = true;
                isPortalPlaced = false;
                SetTransparency();
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            // Get new position of object at each frame it is being dragged
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;

            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            topRight -= new Vector3(0.5f, 0.5f, 0);
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            bottomLeft += new Vector3(0.5f, 1.2f, 0);

            newPosition.x = Mathf.Clamp(newPosition.x, bottomLeft.x + extents.x, topRight.x - extents.x);
            newPosition.y = Mathf.Clamp(newPosition.y, bottomLeft.y + extents.y, topRight.y - extents.y);

            transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            GetPortalPlacePostition();

            // Decrease the scale of the object to show click was released
            transform.localScale /= 1.2f;
            isDragging = false;
            
            PlacePortal();
            SetTransparency();
            ResetObject();  // This is done to fix a bug where collider did not move with the object
            AudioFeedback();
        }
    }

    void ResetObject()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void AudioFeedback()
    {
        if(isPortalPlaced) 
        {
            AudioManager.instance.PlayClip(placeAudio, SFX.PlacePortal);
        }
    }

    private bool CheckEnd(Vector3 portalEnd)
    {
        RaycastHit2D hit = Physics2D.Raycast(portalEnd, portalDirection * (-1), Mathf.Infinity, targetLayer);
        if (hit.collider != null)
        {
            if (Vector2.Distance(portalEnd, hit.point) < endThreshold)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 GetDirectionToMovePortal(Vector3 portalEnd)
    {
        Vector3 direction;
        direction = (portalEnd - portalPosition).normalized;
        return direction;
    }
    private float GetMagnitudeToMovePortal(Vector3 portalEnd)
    {
        float magnitude;
        magnitude = transform.localScale.x / 2;
        return magnitude;
    }

    private void GetPortalEnds(float angle)
    {
        portalEndA = new Vector3(portalPosition.x + portalSizeX / 2 * Mathf.Cos(angle * Mathf.Deg2Rad), portalPosition.y + portalSizeX / 2 * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        portalEndA = portalEndA + portalSizeY / 2 * portalDirection;
        portalEndB = new Vector3(portalPosition.x - portalSizeX / 2 * Mathf.Cos(angle * Mathf.Deg2Rad), portalPosition.y - portalSizeX / 2 * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        portalEndB = portalEndB + portalSizeY / 2 * portalDirection;
    }

    private bool UnplacePortal()
    {
        if (portalPosition == transform.position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsOverlapWithExistingPortal()
    {
        Vector2 portalSize = new Vector2(transform.localScale.x, 0.5f);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(portalPosition, portalSize, transform.rotation.eulerAngles.z, portalLayer);
        hitColliders = hitColliders.Where(collider => collider.gameObject != gameObject).ToArray();
        return hitColliders.Length > 0;
    }

    private void PlacePortal()
    {
        Vector3 direction;
        float magnitude;

        if (placePortalAt == null)
        {
            isPortalPlaced = false;
            return;
        }

        bool unplacePortal = UnplacePortal();
        if (unplacePortal)
        {
            isPortalPlaced = false;
            return;
        }

        if (IsOverlapWithExistingPortal())
        {
            isPortalPlaced = false;
            return;
        }

        float angle = Mathf.Atan2(portalDirection.y, portalDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GetPortalEnds(angle);

        bool checkEndA = CheckEnd(portalEndA);
        bool checkEndB = CheckEnd(portalEndB);

        if (checkEndA && checkEndB)
        {
            transform.rotation = rotation;
            transform.position = portalPosition + portalDirection.normalized * portalOffset;
            isPortalPlaced = true;
        }
        else if (checkEndA ^ checkEndB)
        {
            Vector3 portalEndOnBoundary = checkEndA ? portalEndA : portalEndB;
            Vector3 portalEndNotOnBoundary = checkEndA ? portalEndB : portalEndA;

            direction = GetDirectionToMovePortal(portalEndOnBoundary);
            magnitude = GetMagnitudeToMovePortal(portalEndNotOnBoundary);
            portalPosition += direction * magnitude;

            transform.position = portalPosition + portalDirection.normalized * portalOffset;
            transform.rotation = rotation;
            isPortalPlaced = true;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            isPortalPlaced = false;
        }
    }

    void SetTransparency()
    {
        Color color = portalIconSR.color;
        if (isPortalPlaced)
        {
            color.a = ValueConstants.alphaOpaque;            
        }
        else
        {
            color.a = ValueConstants.alphaTransparent;
        }
        portalIconSR.color = color;
    }

    private void GetPortalPlacePostition()
    {
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;
        Vector2 nearestDirection = Vector2.zero;
        Vector2 nearestHitPoint = Vector2.zero;

        if (isDragging)
        {
            for (float angle = 0; angle < 360; angle += 45)
            {
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, targetLayer);
                if (hit.collider != null)
                {
                    float distance = Vector2.Distance(transform.position, hit.point);

                    if (distance < nearestDistance)
                    {
                        nearestObject = hit.collider.gameObject;
                        nearestDistance = distance;
                        nearestDirection = direction.normalized * (-1);
                        nearestHitPoint = hit.point;
                    }
                }
            }

            if (nearestObject != null && nearestDistance <= distanceToSnap)
            {
                placePortalAt = nearestObject;
                portalDirection = nearestDirection;
                portalPosition = nearestHitPoint;
            }
            else
            {
                placePortalAt = null;
            }
        }
    }
}