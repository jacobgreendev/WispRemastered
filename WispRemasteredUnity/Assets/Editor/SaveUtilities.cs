using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveUtilities : MonoBehaviour
{
    [MenuItem("Save Options/Save Test File")]
    static void SaveTestFile()
    {
        if (LocalSaveData.Instance == null)
            PlayerSaveManager.CreateSaveFile();

        LocalSaveData.Instance.endlessHiScore = 10;
        LocalSaveData.Instance.levelScores = new Dictionary<int, Dictionary<int, int>>();

        if (!LocalSaveData.Instance.levelScores.ContainsKey(1))
        {
            LocalSaveData.Instance.levelScores[1] = new Dictionary<int, int>();
        }

        LocalSaveData.Instance.levelScores[1][2] = 200;

        PlayerSaveManager.SaveFile();
    }

    [MenuItem("Save Options/View Save Details")]
    static void LoadTestFile()
    {
        if (PlayerSaveManager.LoadSaveFile())
        {
            var saveData = LocalSaveData.Instance;
            Debug.Log($"Endless HiScore: {LocalSaveData.Instance.endlessHiScore}");
            Debug.Log("Chapter-Level Scores:");
            foreach(var chapterPair in saveData.levelScores)
            {
                var chapter = chapterPair.Key;
                var scores = chapterPair.Value;
                foreach(var level in scores)
                {
                    Debug.Log($"Level {chapter}-{level.Key}: {level.Value}");
                }
            }
        }
        else
        {
            Debug.Log("No save file exists");
        }
    }

    [MenuItem("Save Options/Delete Save File")]
    static void DeleteSaveFile()
    {
        File.Delete(Application.persistentDataPath + GameConstants.SaveDataFilePath);
        Debug.Log("Save File deleted");
    }

}
