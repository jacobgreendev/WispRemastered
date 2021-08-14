using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class LocalSaveData
{
    public static LocalSaveData Instance;

    [OptionalField(VersionAdded = 2)]
    public int endlessHiScore = 0;

    [OptionalField(VersionAdded = 5)]
    public Dictionary<string, LevelRecordInfo> levelRecords = new();

    [OptionalField(VersionAdded = 6)]
    public Dictionary<string, AchievementInfo> achievementsInfo = new();

    [OnDeserialized]
    void OnDeserialized(StreamingContext context)
    {
        if(levelRecords == null)
        {
            levelRecords = new();
        }

        if (achievementsInfo == null)
        {
            achievementsInfo = new();
        }
    }
}
