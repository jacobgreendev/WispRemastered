using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    ///Time
    public const int FixedUpdatesPerSecond = 60;

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

    //Display Strings
    public const string LevelLockedText = "Locked";
    public const string LevelUnlockedAndUnplayedText = "New!";
    public const string LevelHiScorePrefix = "High Score: ";

    //Values
    public const int AverageLevelStarsForChapterUnlock = 4;
    public const float PixelPerUnitReferenceScreenWidth = 1080;
    public const float DefaultShadowSize = 20;
    public const float ShadowReferenceScreenWidth = 1080;
}
