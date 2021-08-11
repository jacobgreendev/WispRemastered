using UnityEngine;

[CreateAssetMenu(fileName = "WispForm_New", menuName = "Wisp Form/New Wisp Form")]
public class WispForm : ScriptableObject
{
    public WispFormType wispFormType;

    public float forwardForceMultiplier;
    public float verticalForceMultiplier;
    public float sidewaysForceMultiplier;

    public float gravityMultiplier;
    public float drag;
}

public enum WispFormType
{
    Flame,
    Spark,
    Aether,
    None = 99
}
