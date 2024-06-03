using System;
using System.Collections;
using UnityEngine;

public class PortalAnimator : MonoBehaviour
{
    Animator animator;
    string newStateName;
    public string color;
    readonly float forceMagnitude = 4f;

    public void AnimatePortal()
    {
        animator = GetComponent<Animator>();

        // Start coroutine to delay the animation setup
        StartCoroutine(DelayedAnimatePortal());
    }

    void ChangeState()
    {
        newStateName = Mapping.PortalColorToAnim[color];            
        SetAnimatorParameter(newStateName);
    }

    private IEnumerator DelayedAnimatePortal()
    {
        // Wait for a frame to ensure the sprite is updated
        yield return new WaitForSeconds(0.05f);       
        ChangeState();
        InitParticles();        
    }

    void InitParticles()
    {
        // Get particles from pool
        ParticleSystem portalParticles = ParticlePooler.Instance.SpawnFromPool(NameConstants.PortalFX, transform.position, Quaternion.identity);

        // Get start color of particles
        var main = portalParticles.main;
        string colorHex = Mapping.ColorNameToHex[color];
        ColorUtility.TryParseHtmlString(colorHex, out Color colorParticles);
        main.startColor = new ParticleSystem.MinMaxGradient(colorParticles);

        // Get force over lifetime of particles from the z rotation of portal
        float zRotation = transform.rotation.eulerAngles.z + 90f;
        zRotation = ((float)(Math.Round((double)zRotation / 10) * 10)); // round to nearest 10 due to floating point precision
        float zRotationRadians = zRotation * Mathf.Deg2Rad;
        Vector3 forceDirection = new(Mathf.Cos(zRotationRadians), Mathf.Sin(zRotationRadians), 0);
        forceDirection = forceMagnitude * forceDirection;

        var particlesForce = portalParticles.forceOverLifetime;
        particlesForce.enabled = true;
        particlesForce.x = new ParticleSystem.MinMaxCurve(forceDirection.x);
        particlesForce.y = new ParticleSystem.MinMaxCurve(forceDirection.y);
        particlesForce.z = new ParticleSystem.MinMaxCurve(forceDirection.z);

        var shape = portalParticles.shape;
        shape.rotation = transform.rotation.eulerAngles;
    }

    void SetAnimatorParameter(string stateName)
    {
        switch (stateName)
        {
            case FileConstants.PortalAnim + NameConstants.Orange:
                animator.SetTrigger(NameConstants.Orange);
                break;
            case FileConstants.PortalAnim + NameConstants.Blue:
                animator.SetTrigger(NameConstants.Blue);
                break;
            case FileConstants.PortalAnim + NameConstants.Green:
                animator.SetTrigger(NameConstants.Green);
                break;
            default:
                Debug.LogWarning("Unknown animation state: " + stateName);
                break;
        }
    }
}
