using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IResetable
{
    [SerializeField] protected float laserDistance = 100;
    public Transform laserStartPoint;
    LineRenderer lineRenderer;
    BoxCollider2D laserCollider;
    LayerMask ignoreLayer;
    public bool isLaserOn = true;
    bool originalStateLaser;
    readonly float laserOffset = 1f;
    List<GameObject> laserChildren = new();

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserCollider = GetComponent<BoxCollider2D>();

        int allLayers = ~0;
        int excludedLayers = allLayers & ~(1 << 6) & ~(1 << gameObject.layer);
        ignoreLayer = excludedLayers;
    }

    private void OnEnable()
    {
        PlayButtonController.Instance.RegisterResetableObject(this);
    }

    private void OnDisable()
    {
        PlayButtonController.Instance.UnregisterResetableObject(this);
    }

    void Update()
    {
        if (isLaserOn)
        {
            ShootLaser(laserStartPoint.position, transform.right);
        }        
    }

    void ShootLaser(Vector3 start, Vector3 direction)
    {        
        // Cast a ray towards right of the laser gun
        RaycastHit2D hit = Physics2D.Raycast(start, direction, laserDistance, ignoreLayer);

        // if ray hits a collider, then we have both start and end point of the laser
        if (hit.collider != null)
        {            
            Draw2DRay(start, hit.point);
        } 
        // if not, then end point is taken from inspector
        else
        {
            Draw2DRay(start, direction * laserDistance);
        }
    }

    void Draw2DRay(Vector2 startpos, Vector2 endpos)
    {
        // Set start and end point
        lineRenderer.SetPosition(0, startpos);
        lineRenderer.SetPosition(1, endpos);

        // Set collider properties
        Vector2 colliderSize = new Vector2((endpos - startpos).magnitude + laserOffset, lineRenderer.startWidth);
        Vector2 colliderOffset = new Vector2((colliderSize.x/2 + transform.localScale.x/2), 0);
        laserCollider.size = colliderSize;
        laserCollider.offset = colliderOffset;
    }

    public void TeleportLaser(Vector3 start, Vector3 direction, LayerMask layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(start, direction, laserDistance, layerMask);
        if (hit.collider != null)
        {
            DrawTeleportedLaser(start, hit.point, layerMask, direction);
        }
        else
        {
            DrawTeleportedLaser(start, direction * laserDistance, layerMask, direction);
        }
    }

    void DrawTeleportedLaser(Vector2 startpos, Vector2 endpos, LayerMask layerMask, Vector2 direction)
    {
        GameObject emptyChild = new GameObject(NameConstants.LaserChild);
        emptyChild.transform.position = startpos;
        LaserChild laserChild = emptyChild.AddComponent<LaserChild>();
        laserChildren.Add(emptyChild);
        laserChild.EnableLaserChild(lineRenderer, startpos, endpos, layerMask, direction);
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

    public void SwitchLaser(bool switchLaser)
    {
        lineRenderer.enabled = switchLaser;
        laserCollider.enabled = switchLaser;
        isLaserOn = switchLaser;
    }

    public void GetOriginalState()
    {
        originalStateLaser = isLaserOn;
    }

    public void SetOriginalState()
    {
        isLaserOn = originalStateLaser;
        SwitchLaser(isLaserOn);
    }

}
