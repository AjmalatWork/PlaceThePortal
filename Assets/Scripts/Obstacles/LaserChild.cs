using UnityEngine;

public class LaserChild : Laser
{
    Vector2 currentStartpos;
    Vector2 currentDirection;
    LayerMask currentLayerMask;
    LineRenderer portalLineRenderer;
    BoxCollider2D portalLaserCollider;
    bool isLaserChildEnabled = false;

    private void Update()
    {
        if (isLaserOn && isLaserChildEnabled)
        {
            ShootLaserChild();
        }
    }

    void ShootLaserChild()
    {
        RaycastHit2D hit = Physics2D.Raycast(currentStartpos, currentDirection, laserDistance, currentLayerMask);
        if (hit.collider != null)
        {
            DrawLaser(currentStartpos, hit.point);
        }
        else
        {
            DrawLaser(currentStartpos, currentDirection * laserDistance);
        }
    }

    void DrawLaser(Vector2 startpos, Vector2 endpos)
    {
        // Set start and end point
        portalLineRenderer.SetPosition(0, startpos);
        portalLineRenderer.SetPosition(1, endpos);

        // Set collider properties
        Vector2 colliderSize = new Vector2((endpos - startpos).magnitude, portalLineRenderer.startWidth);
        Vector2 colliderOffset = new Vector2(colliderSize.x / 2, 0);
        portalLaserCollider.size = colliderSize;
        portalLaserCollider.offset = colliderOffset;
    }

    public void EnableLaserChild(LineRenderer lineRenderer, Vector2 startpos, Vector2 endpos, LayerMask layerMask, Vector2 direction)
    {
        currentStartpos = startpos;
        currentLayerMask = layerMask;
        currentDirection = direction;

        portalLineRenderer = gameObject.AddComponent<LineRenderer>();
        portalLineRenderer.startWidth           = lineRenderer.startWidth;
        portalLineRenderer.endWidth             = lineRenderer.endWidth;
        portalLineRenderer.widthCurve           = lineRenderer.widthCurve;
        portalLineRenderer.useWorldSpace        = lineRenderer.useWorldSpace;
        portalLineRenderer.numCapVertices       = lineRenderer.numCapVertices;
        portalLineRenderer.numCornerVertices    = lineRenderer.numCornerVertices;
        portalLineRenderer.positionCount        = lineRenderer.positionCount;
        portalLineRenderer.colorGradient        = lineRenderer.colorGradient;
        portalLineRenderer.materials            = lineRenderer.materials;
        portalLineRenderer.SetPosition(0, startpos);
        portalLineRenderer.SetPosition(1, endpos);

        portalLaserCollider = gameObject.AddComponent<BoxCollider2D>();
        Vector2 colliderSize = new Vector2((endpos - startpos).magnitude, portalLineRenderer.startWidth);
        Vector2 colliderOffset = new Vector2(colliderSize.x / 2, 0);
        portalLaserCollider.size        = colliderSize;
        portalLaserCollider.offset      = colliderOffset;
        portalLaserCollider.isTrigger   = true;

        isLaserChildEnabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the ball touches the laser, you lose!
        if (collision.gameObject.name == NameConstants.Ball)
        {
            Debug.Log("Restart!");
            Time.timeScale = 0f;
        }
    }
}
