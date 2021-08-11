using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateLevelWizard : EditorWindow
{
    private string fileName, levelID;
    private GameObject levelPrefab;
    private float[] scoreStarThresholds, timeStarThresholds;

    private bool addToList;
    private LevelList chapterToAddTo;

    [MenuItem("Levels/Create Level")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateLevelWizard));
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("File Name (no extension)", fileName);
        levelID = EditorGUILayout.TextField("Level ID", levelID);
        levelPrefab = (GameObject) EditorGUILayout.ObjectField("Level Prefab", levelPrefab, typeof(GameObject), false);

        GUILayout.Space(10);

        GUILayout.Label("Score Stars (Score Ascending)", EditorStyles.boldLabel);
        if (scoreStarThresholds == null)
        {
            scoreStarThresholds = new float[3];
        }

        for (int i = 0; i < 3; i++)
        {
            scoreStarThresholds[i] = EditorGUILayout.FloatField($"Score Threshold {i + 1}", scoreStarThresholds[i]);
        }

        GUILayout.Space(10);

        GUILayout.Label("Time Stars (Time Descending - Lower is Better)", EditorStyles.boldLabel);
        if(timeStarThresholds == null)
        {
            timeStarThresholds = new float[3];
        }

        for (int i = 0; i < 3; i++)
        {
            timeStarThresholds[i] = EditorGUILayout.FloatField($"Time Threshold {i + 1}", timeStarThresholds[i]);
        }

        GUILayout.Space(10);

        addToList = EditorGUILayout.BeginToggleGroup("Add To Level List", addToList);
        chapterToAddTo = (LevelList)EditorGUILayout.ObjectField("Level List To Add To (can be null)", chapterToAddTo, typeof(LevelList), false);
        EditorGUILayout.EndToggleGroup();


        if (GUILayout.Button("Create"))
        {
            CreateLevel();
        }
    }

    private void CreateLevel()
    {
        if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(levelID) || levelPrefab == null) return;

        LevelInfo newLevel = ScriptableObject.CreateInstance<LevelInfo>();
        newLevel.levelID = levelID;
        newLevel.levelPrefab = levelPrefab;
        newLevel.scoreStarInfo = new StarInfo(scoreStarThresholds);
        newLevel.timeSecondsStarInfo = new StarInfo(timeStarThresholds);

        AssetDatabase.CreateAsset(newLevel, $"Assets/ScriptableObjects/Levels/{fileName}.asset");

        if (addToList && chapterToAddTo != null)
        {
            chapterToAddTo.levels.Add(newLevel);
        }

        AssetDatabase.SaveAssets();
    }
}
