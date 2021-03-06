using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneData
{
    public static List<LevelInfo> levelList;
    public static LevelInfo levelToLoad, levelJustCompleted;
    public static int chapterLoaded;
    public static Dictionary<string, AchievementDetails> achievementDetailsByID;
}
