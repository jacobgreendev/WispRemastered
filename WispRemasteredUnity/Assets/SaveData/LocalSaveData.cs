using System.Collections.Generic;

[System.Serializable]
public class LocalSaveData
{
    public static LocalSaveData Instance;

    public int hiScore = 0;
    public Dictionary<int, int> levelScores = new Dictionary<int, int>();
}
