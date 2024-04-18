using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float laserDistance = 100;
    public Transform laserStartPoint;
    LineRenderer lineRenderer;
    BoxCollider2D laserCollider;
    LayerMask ignoreLayer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserCollider = GetComponent<BoxCollider2D>();
        ignoreLayer = ~(1 << gameObject.layer);
    }

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        // Cast a ray towards right of the laser gun
        RaycastHit2D hit = Physics2D.Raycast(laserStartPoint.position, transform.right, laserDistance, ignoreLayer);

        // if ray hits a collider, then we have both start and end point of the laser
        if (hit.collider != null)
        {            
            Draw2DRay(laserStartPoint.position, hit.point);
        } 
        // if not, then end point is taken from inspector
        else
        {
            Draw2DRay(laserStartPoint.position, laserStartPoint.transform.right * laserDistance);
        }

    }

    void Draw2DRay(Vector2 startpos, Vector2 endpos)
    {
        // Set start and end point
        lineRenderer.SetPosition(0, startpos);
        lineRenderer.SetPosition(1, endpos);

        // Set collider properties
        Vector2 colliderSize = new Vector2((endpos - startpos).magnitude, lineRenderer.startWidth);
        Vector2 colliderOffset = new Vector2((colliderSize.x/2 + transform.localScale.x/2), 0);
        laserCollider.size = colliderSize;
        laserCollider.offset = colliderOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the ball touches the goal, you win!
        if (collision.gameObject.name == "Ball")
        {
            Debug.Log("Restart!");
            Time.timeScale = 0f;
        }
    }
}
