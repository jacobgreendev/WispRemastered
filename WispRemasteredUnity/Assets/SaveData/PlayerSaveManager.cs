using System;
using UnityEditor;
using UnityEngine;

public static class PlayerSaveManager
{
    [MenuItem("Save Options/Save Test File")]
    static void SaveTestFile()
    {
        LocalSaveData.save.maxScore = 10;
        SaveFile();
    }

    [MenuItem("Save Options/Load Test File")]
    static void LoadTestFile()
    {
        if(LoadSaveFile())
        {
            Debug.Log("Saved file had a Max Score of " + LocalSaveData.save.maxScore);
        }
    }

    public static bool LoadSaveFile()
    {
        try
        {
            LocalSaveData.save = (LocalSaveData) SerializationManager.Load(Application.persistentDataPath + GameConstants.SaveDataFilePath);
        }
        catch(InvalidCastException e)
        {
            Debug.LogError(e.ToString());
            return false;
        }

        return true;
    }

    public static bool SaveFile()
    {
        return SerializationManager.SaveLocalPlayerData(LocalSaveData.save);
    }
}
