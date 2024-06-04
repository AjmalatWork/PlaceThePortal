using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform destination;

    AudioSource teleportAudio;
    BoxCollider2D portalCollider;

    private void Awake()
    {
        teleportAudio = GetComponent<AudioSource>();
        portalCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        // This fixes a bug where sometimes the object did not detect collision with portal
        portalCollider.enabled = false;
        portalCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(destination == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag(TagConstants.Portable))
        {
            if (Vector2.Distance(collision.gameObject.transform.position, transform.position) > 0.25f)
            {
                AudioManager.instance.PlayClip(teleportAudio, SFX.Teleport); 
                // Get rigidbody2d component of colliding object
                Rigidbody2D portableRb = collision.gameObject.GetComponent<Rigidbody2D>();

                // Get direction of Entry Portal
                Vector3 entryDirection = transform.up.normalized;

                // Get direction of Exit Portal
                Vector3 exitDirection = destination.transform.up.normalized;

                // Get direction of Ball at the time of entry
                Vector3 portableEntryDirection = portableRb.velocity.normalized;

                // Calculate direction of Ball at the time of exit
                Vector3 portableExitDirection = entryDirection + exitDirection + portableEntryDirection;

                // Change velocity of ball at exit by using the exit ball direction just calculated above
                portableRb.velocity = portableExitDirection * portableRb.velocity.magnitude;

                // Change position to exit portal position
                collision.gameObject.transform.position = destination.transform.position + collision.gameObject.transform.localScale.magnitude / 2 * exitDirection;

            }
        }

        if (collision.gameObject.CompareTag(TagConstants.Laser))
        {
            Laser laser = collision.gameObject.GetComponent<Laser>();
            LayerMask layerMask = ~(1 << gameObject.layer);
            laser.TeleportLaser(destination.transform.position, destination.transform.up.normalized, layerMask);
        }
    }
}
