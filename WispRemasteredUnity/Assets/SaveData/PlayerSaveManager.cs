using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class PlayerSaveManager
{
    [MenuItem("Save Options/Save Test File")]
    static void SaveTestFile()
    {
        if (LocalSaveData.save == null)
            CreateSaveFile();

        LocalSaveData.save.maxScore = 10;
        SaveFile();          
    }

    [MenuItem("Save Options/Load Save File")]
    static void LoadTestFile()
    {
        if(LoadSaveFile())
        {
            Debug.Log("Saved file had a Max Score of " + LocalSaveData.save.maxScore);
        }
    }

    [MenuItem("Save Options/Delete Save File")]
    static void DeleteSaveFile()
    {
        File.Delete(Application.persistentDataPath + GameConstants.SaveDataFilePath);
        Debug.Log("Save File deleted");
    } 

    public static bool LoadSaveFile()
    {
        LocalSaveData localSave;
        try
        {
            localSave = (LocalSaveData) SerializationManager.Load(Application.persistentDataPath + GameConstants.SaveDataFilePath);
        }
        catch(InvalidCastException e)
        {
            Debug.LogError(e.ToString());
            return false;
        }

        return localSave != null;
    }

    public static bool SaveFile()
    {
        return SerializationManager.SaveLocalPlayerData(LocalSaveData.save);
    }

    public static void CreateSaveFile()
    {
        LocalSaveData.save = new LocalSaveData();
    }
}
