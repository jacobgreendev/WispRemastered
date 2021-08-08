using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class LocalSaveData
{
    public static LocalSaveData Instance;

    [OptionalField(VersionAdded = 2)]
    public int endlessHiScore = 0;

    [OptionalField(VersionAdded = 2)] 
    public Dictionary<int, Dictionary<int, int>> levelScores = new Dictionary<int, Dictionary<int, int>>();

    [OnDeserialized]
    void OnDeserialized(StreamingContext context)
    {
        if(levelScores == null)
        {
            levelScores = new();
        }
    }
}
