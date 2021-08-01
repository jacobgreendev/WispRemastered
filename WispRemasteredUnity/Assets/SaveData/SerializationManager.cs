using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager
{
    public static bool Save(string saveName, object saveData)
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        string savePath = Application.persistentDataPath + GameConstants.SaveFolderPath;

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        savePath = savePath + "/" + saveName + ".save";

        FileStream file = File.Create(savePath);
        formatter.Serialize(file, saveData);
        file.Close();

        return true;
    }

    public static bool SaveLocalPlayerData(object saveData)
    {
        return Save(GameConstants.SaveDataFileName, saveData);
    }

    public static object Load(string path)
    {
        if(!File.Exists(path))
            return null;

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed to load save file at {path}" + e.ToString());
            file.Close();
            return null;
        }

    }


    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        return formatter;
    }
}
