using UnityEngine;

public class Laser : MonoBehaviour, IResetable
{
    [SerializeField] private float laserDistance = 100;
    public Transform laserStartPoint;
    LineRenderer lineRenderer;
    BoxCollider2D laserCollider;
    LayerMask ignoreLayer;
    public bool isLaserOn = true;
    bool originalStateLaser;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserCollider = GetComponent<BoxCollider2D>();
        ignoreLayer = ~(1 << gameObject.layer);
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
            ShootLaser();
        }
        
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
