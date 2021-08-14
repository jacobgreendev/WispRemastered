using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Achievements/New Achievement")]
public class AchievementDetails : ScriptableObject
{
    public string id;
    public string title;
    public float amountToUnlock;
    public string description;
}
