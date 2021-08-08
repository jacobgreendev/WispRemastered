using System.Collections.Generic;

[System.Serializable]
public class LocalSaveData
{
    public static LocalSaveData Instance;

    public int endlessHiScore = 0;
    public Dictionary<int, Dictionary<int, int>> levelScores = new Dictionary<int, Dictionary<int, int>>();
}
