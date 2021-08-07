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

        LocalSaveData.Instance.hiScore = 10;
        LocalSaveData.Instance.levelScores = new Dictionary<int, int>();
        LocalSaveData.Instance.levelScores[1] = 100;
        LocalSaveData.Instance.levelScores[2] = 200;
        PlayerSaveManager.SaveFile();
    }

    [MenuItem("Save Options/Load Save File")]
    static void LoadTestFile()
    {
        if (PlayerSaveManager.LoadSaveFile())
        {
            Debug.Log($"Saved file had a Max Score of {LocalSaveData.Instance.hiScore} and had a score of {LocalSaveData.Instance.levelScores[2]} for level 2");
        }
    }

    [MenuItem("Save Options/Delete Save File")]
    static void DeleteSaveFile()
    {
        File.Delete(Application.persistentDataPath + GameConstants.SaveDataFilePath);
        Debug.Log("Save File deleted");
    }

}
