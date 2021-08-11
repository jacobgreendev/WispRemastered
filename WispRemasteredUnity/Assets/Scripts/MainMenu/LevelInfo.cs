using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "Levels/New LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public string levelID;
    public GameObject levelPrefab;
    public StarInfo scoreStarInfo, timeSecondsStarInfo;
}

[System.Serializable]
public class StarInfo
{
    public float[] starThresholds;

    public StarInfo(float[] starThresholds)
    {
        this.starThresholds = starThresholds;
    }

    public StarInfo()
    {

    }
}
