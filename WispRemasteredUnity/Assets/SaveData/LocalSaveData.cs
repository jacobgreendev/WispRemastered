using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class LocalSaveData
{
    public static LocalSaveData Instance;

    [OptionalField(VersionAdded = 2)]
    public int endlessHiScore = 0;

    [OptionalField(VersionAdded = 4)]
    public Dictionary<string, int> levelScores = new();

    [OptionalField(VersionAdded = 4)]
    public Dictionary<string, float> levelTimesSeconds = new();

    [OnDeserialized]
    void OnDeserialized(StreamingContext context)
    {
        if(levelScores == null)
        {
            levelScores = new();
        }

        if(levelTimesSeconds == null)
        {
            levelTimesSeconds = new();
        }
    }
}
