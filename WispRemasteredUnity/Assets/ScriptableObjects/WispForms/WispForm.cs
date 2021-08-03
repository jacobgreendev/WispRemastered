using UnityEngine;

[CreateAssetMenu(fileName = "WispForm_New", menuName = "Wisp Form/New Wisp Form")]
public class WispForm : ScriptableObject
{
    public WispFormType wispFormType;

    public float forwardForceMultiplier;
    public float verticalForceMultiplier;
    public float sidewaysForceMultiplier;
}

public enum WispFormType
{
    Flame,
    Spark,
    None = 99
}
