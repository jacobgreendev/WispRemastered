using System;
using System.IO;
using UnityEngine;

public static class PlayerSaveManager
{
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
        return SerializationManager.SaveLocalPlayerData(LocalSaveData.Instance);
    }

    public static void CreateSaveFile()
    {
        LocalSaveData.Instance = new LocalSaveData();
    }
}
