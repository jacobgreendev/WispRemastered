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

    public int bounces;
    public float bouncePowerRetention;
}

//Keep indices the same when altering!
public enum WispFormType
{
    All = 0,
    Flame = 1,
    Spark = 2,
    Aether = 3,
    Frost = 4,
    None = 99
}
