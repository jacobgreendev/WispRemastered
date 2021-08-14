using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateAchievementWizard : EditorWindow
{
    private string fileName, achievementID, achievementTitle, achievementDescription;
    private float amountToUnlock;

    private bool progressable, addToList;
    private AchievementList achievementListToAddTo;

    [MenuItem("Game Utilities/Achievements/Create Achievement")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateAchievementWizard));
    }

    private void OnGUI()
    {
        GUILayout.Label("Achievement Settings", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("File Name (no extension)", fileName);
        achievementID = EditorGUILayout.TextField("Achievement ID", achievementID);
        achievementTitle = EditorGUILayout.TextField("Achievement Title", achievementTitle);
        achievementDescription = EditorGUILayout.TextField("Achievement Description", achievementDescription);

        progressable = EditorGUILayout.BeginToggleGroup("Progressable", progressable);
        amountToUnlock = EditorGUILayout.FloatField("Amount To Unlock", amountToUnlock);
        EditorGUILayout.EndToggleGroup();

        addToList = EditorGUILayout.BeginToggleGroup("Progressable", addToList);
        achievementListToAddTo = (AchievementList) EditorGUILayout.ObjectField("Achievement List To Add To", achievementListToAddTo, typeof(AchievementList), false);
        EditorGUILayout.EndToggleGroup();


        if (GUILayout.Button("Create"))
        {
            CreateAchievement();
        }
    }

    private void CreateAchievement()
    {
        if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(achievementID)) return;

        AchievementDetails newAchievement = ScriptableObject.CreateInstance<AchievementDetails>();
        newAchievement.id = achievementID;
        newAchievement.title = achievementTitle;
        newAchievement.description = achievementDescription;
        newAchievement.amountToUnlock = progressable ? amountToUnlock : 0;

        AssetDatabase.CreateAsset(newAchievement, $"Assets/ScriptableObjects/Achievements/{fileName}.asset");

        if (addToList && achievementListToAddTo != null)
        {
            achievementListToAddTo.list.Add(newAchievement);
        }

        AssetDatabase.SaveAssets();
    }
}
