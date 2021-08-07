using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "Levels/New LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public int levelNumber;
    public GameObject levelPrefab;
}