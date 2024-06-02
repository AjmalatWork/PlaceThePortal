using UnityEditor.Animations;
using UnityEngine;

public class PortalAnimator : MonoBehaviour
{
    Animator animator;
    string newStateName;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer baseLayer = animatorController.layers[0]; 
        AnimatorStateMachine stateMachine = baseLayer.stateMachine;

        GetStateName();

        // Iterate through the states to find the new state by name
        foreach (ChildAnimatorState state in stateMachine.states)
        {
            if (state.state.name == newStateName)
            {
                // Set the new state as the default state of the base layer
                stateMachine.defaultState = state.state;
                return;
            }
        }

        Debug.LogWarning("New state not found: " + newStateName);
    }

    void GetStateName()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string colorName = spriteRenderer.sprite.name;
        colorName = colorName[10..];
        Debug.Log(colorName);
        newStateName = FileConstants.PortalAnim + colorName;
    }
}
