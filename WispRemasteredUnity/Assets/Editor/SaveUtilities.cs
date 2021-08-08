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
        LocalSaveData.Instance.levelRecords = new();

        var newRecords = new LevelRecordInfo();
        newRecords.hiScore = 100;
        newRecords.timeRecord = 100;
        LocalSaveData.Instance.levelRecords["1-2"] = newRecords;

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
            foreach(var levelScore in saveData.levelRecords)
            {
                Debug.Log($"Level {levelScore.Key}: {levelScore.Value.hiScore} in {levelScore.Value.timeRecord} seconds");
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
