using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    ///Tags
    public const string Tag_LandingTrigger = "LandingTrigger";
    public const string Tag_Ground = "Ground";
    public const string Tag_InteractableTrigger = "InteractableTrigger";

    ///Layers
    public const string Layer_Ground = "Ground";

    //Paths
    public const string SaveFolderPath = "/LocalData";
    public const string SaveDataFilePath = SaveFolderPath + "/" + SaveDataFileName + ".save";
    public const string SaveDataFileName = "saveFile";
}
