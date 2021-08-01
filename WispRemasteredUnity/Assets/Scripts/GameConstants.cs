using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    ///Tags
    public const string LandingTrigger_Tag = "LandingTrigger";

    //Paths
    public const string SaveFolderPath = "/LocalData";
    public const string SaveDataFilePath = SaveFolderPath + "/" + SaveDataFileName + ".save";
    public const string SaveDataFileName = "saveFile";
}
