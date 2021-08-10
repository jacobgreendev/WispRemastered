using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelList", menuName = "Levels/New LevelList")]
public class LevelList : ScriptableObject
{
    public List<LevelInfo> levels;
}
