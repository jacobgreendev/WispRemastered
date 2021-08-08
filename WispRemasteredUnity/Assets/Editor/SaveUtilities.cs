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
        LocalSaveData.Instance.levelScores = new Dictionary<string, int>();

        LocalSaveData.Instance.levelScores["1-2"] = 200;

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
            foreach(var levelScore in saveData.levelScores)
            {
                Debug.Log($"Level {levelScore.Key}: {levelScore.Value}");
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
