using UnityEngine;

[CreateAssetMenu(fileName = "TerminalVelocitySettings", menuName = "Settings/Terminal Velocity Settings")]
public class TerminalVelocitySettings : ScriptableObject
{
    [SerializeField] private float terminalVelocity = 15f;

    public float TerminalVelocity
    {
        get { return terminalVelocity; }
        set { terminalVelocity = value; }
    }
}
