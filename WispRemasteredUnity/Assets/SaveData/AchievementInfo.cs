using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementInfo
{
    public string achievementID;
    public bool unlocked;
    public float progressAmount, amountToUnlock;

    public AchievementInfo(string id, bool unlocked)
    {
        this.achievementID = id;
        this.unlocked = unlocked;
    }

    public AchievementInfo(string id, bool unlocked, float progressAmount, float amountToUnlock)
    {
        this.achievementID = id;
        this.unlocked = unlocked;
        this.progressAmount = progressAmount;
        this.amountToUnlock = amountToUnlock;
    }
}
