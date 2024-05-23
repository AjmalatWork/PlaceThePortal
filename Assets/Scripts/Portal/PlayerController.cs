using System;
//using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables needed in inspector
    public LayerMask targetLayer;
    public float distanceToSnap;
    public GameObject portal;
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

    private void OnMouseDown()
    {
        // Calculate the offset between the GameObject's position and the mouse position
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Increase the scale of the object to show it was clicked
        transform.localScale *= 1.2f;

        // Set dragging as true
        isDragging = true;
    }

    private bool CheckEnd( Vector3 portalEnd)
    {        
        // Cast a ray in the direction opposite to portal direction from ends of the portal
        // if the ray hits a point that is very close to the portalEnd, then it is on the edge of the placeable object
        // return true if end is on edge
        // else return false
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
        // direction from centre to the end that is on the boundary
        Vector3 direction;
        direction = (portalEnd - portalPosition).normalized;
        return direction;
    }
    private float GetMagnitudeToMovePortal(Vector3 portalEnd)
    {
        // magnitude is half of the x scale of portal
        // this is approximate value, can be optimized
        float magnitude;
        magnitude = transform.localScale.x/2;
        return magnitude;
    }

    private void GetPortalEnds(float angle)
    {        
        // Endpoints are ( x +- rcosQ, y +- rsinQ) and then move a little in the direction of the portal so that the ray hits the collider
        portalEndA = new Vector3(portalPosition.x + transform.localScale.x / 2 * Mathf.Cos(angle * Mathf.Deg2Rad), portalPosition.y + transform.localScale.x / 2 * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        portalEndA = portalEndA + transform.localScale.y / 2 * portalDirection;
        portalEndB = new Vector3(portalPosition.x - transform.localScale.x / 2 * Mathf.Cos(angle * Mathf.Deg2Rad), portalPosition.y - transform.localScale.x / 2 * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        portalEndB = portalEndB + transform.localScale.y / 2 * portalDirection;
    }

    //Don't place portal if position is inside another object's collider
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

    private void PlacePortal()
    {
        Vector3 direction;
        float magnitude;

        // If object is close to a placeable object 
        if (placePortalAt == null)
        {
            isPortalPlaced = false;
            return;
        }

        // Don't place portal if inside another collider
        bool unplacePortal = UnplacePortal();
        if (unplacePortal)
        {
            isPortalPlaced = false;
            return;
        }

        // Calculate the rotation angle around the Z-axis (yaw) using atan2
        float angle = Mathf.Atan2(portalDirection.y, portalDirection.x) * Mathf.Rad2Deg - 90;

        // Create a rotation quaternion based on the calculated angle
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Get endpoints of the placed portal
        GetPortalEnds(angle);

        // Check if portal ends on placeable object boundary
        bool checkEndA = CheckEnd(portalEndA);
        bool checkEndB = CheckEnd(portalEndB);

        // If both ends are on edge
        if (checkEndA && checkEndB)
        {
            // Apply the resulting rotation to the object
            transform.rotation = rotation;

            // Set the position of the portal
            transform.position = portalPosition + portalDirection.normalized * 0.05f;
            // Set portalPlaced as true
            isPortalPlaced = true;
        }
        // If exactly one end is on edge
        else if (checkEndA ^ checkEndB)
        {
            // Determine which end is on the boundary and which is not
            Vector3 portalEndOnBoundary = checkEndA ? portalEndA : portalEndB;
            Vector3 portalEndNotOnBoundary = checkEndA ? portalEndB : portalEndA;

            // Get the direction and magnitude to move the portal in the direction of the end that is on boundary
            direction = GetDirectionToMovePortal(portalEndOnBoundary);
            magnitude = GetMagnitudeToMovePortal(portalEndNotOnBoundary);
            portalPosition += direction * magnitude;

            transform.position = portalPosition;
            transform.rotation = rotation;
            // Set portalPlaced as true
            isPortalPlaced = true;
        }
        // if no ends are on edge
        else
        {
            // The portal cannot be placed. Set its rotation to initial
            transform.rotation = Quaternion.identity;

            // Set portalPlaced as false
            isPortalPlaced = false;
        }
    }

    private void OnMouseUp()
    {
        GetPortalPlacePostition();

        // Decrease the scale of the object to show click was released
        transform.localScale /= 1.2f;

        // Set dragging as false
        isDragging = false;

        PlacePortal();
    }

    private void GetPortalPlacePostition()
    {
        // Initialize variables to track the nearest object and its distance
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;
        Vector2 nearestDirection = Vector2.zero;
        Vector2 nearestHitPoint = Vector2.zero;

        // If mouse is clicked  
        if (isDragging)
        {
            // Cast a ray at 8 directions divided at each 45 degrees
            for (float angle = 0; angle < 360; angle += 45)
            {
                // Get the direction from angle
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, targetLayer);
                if (hit.collider != null)
                {
                    // Calculate distance to the hit point
                    float distance = Vector2.Distance(transform.position, hit.point);

                    // Update nearest object if the distance is smaller
                    if (distance < nearestDistance)
                    {
                        nearestObject = hit.collider.gameObject;
                        nearestDistance = distance;
                        nearestDirection = direction.normalized * (-1);
                        nearestHitPoint = hit.point;
                    }
                }
            }

            // Check if a nearest object was found and distance is less than thresold distance to snap
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

    private void OnMouseDrag()
    {
        // Get new position of object at each frame it is being dragged
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPosition = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        // If mouse is clicked and current position of object changes
        if (isDragging && newPosition != transform.position)
        {
            // Change the position of object to new position
            transform.position = newPosition;
        }
    }


}
