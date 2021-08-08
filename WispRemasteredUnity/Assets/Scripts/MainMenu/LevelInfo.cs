using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "Levels/New LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public int chapterNumber, levelNumber;
    public GameObject levelPrefab;
    public StarInfo scoreStarInfo, timeSecondsStarInfo;
}

[System.Serializable]
public class StarInfo
{
    public int star1Theshold, star2Threshold, star3Threshold;
}
