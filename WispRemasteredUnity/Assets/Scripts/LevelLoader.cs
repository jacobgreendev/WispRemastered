using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    private void Awake()
    {
        Instance = this;
        Instantiate(SceneData.levelToLoad.levelPrefab);
    }
}
