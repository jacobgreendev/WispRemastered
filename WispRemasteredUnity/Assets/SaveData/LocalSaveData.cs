using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class LocalSaveData
{
    public static LocalSaveData Instance;

    public int endlessHiScore = 0;
    public Dictionary<int, Dictionary<int, int>> levelScores = new Dictionary<int, Dictionary<int, int>>();

    [OnDeserialized]
    void OnDeserialized()
    {
        if(levelScores == null)
        {
            levelScores = new();
        }
    }
}
