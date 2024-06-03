using System.Collections;
using UnityEngine;

public class PortalAnimator : MonoBehaviour
{
    Animator animator;
    string newStateName;
    public string color;

    public void AnimatePortal()
    {
        animator = GetComponent<Animator>();

        // Start coroutine to delay the animation setup
        StartCoroutine(DelayedAnimatePortal());
    }

    void ChangeState()
    {
        newStateName = Mapping.PortalColorToAnim[color];            
        Debug.Log("Anim name " + newStateName);
        SetAnimatorParameter(newStateName);
    }

    private IEnumerator DelayedAnimatePortal()
    {
        // Wait for a frame to ensure the sprite is updated
        yield return new WaitForSeconds(0.05f);       
        ChangeState();
    }

    void SetAnimatorParameter(string stateName)
    {
        switch (stateName)
        {
            case FileConstants.PortalAnim + NameConstants.Orange:
                animator.SetTrigger("Orange");
                break;
            case FileConstants.PortalAnim + NameConstants.Blue:
                animator.SetTrigger("Blue");
                break;
            case FileConstants.PortalAnim + NameConstants.Green:
                animator.SetTrigger("Green");
                break;
            default:
                Debug.LogWarning("Unknown animation state: " + stateName);
                break;
        }
    }
}
