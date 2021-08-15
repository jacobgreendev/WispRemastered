using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievementList", menuName = "Achievements/New Achievement List")]
public class AchievementList: ScriptableObject
{
    public List<AchievementDetails> list;
}
