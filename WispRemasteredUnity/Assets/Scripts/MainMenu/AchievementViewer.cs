using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementViewer : UIBase
{
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform listContentTransform;
    [SerializeField] private RectMask2D invisibilityMask;

    private void OnEnable()
    {
        var titleTextList = new List<TextMeshProUGUI>();
        var descriptionTextList = new List<TextMeshProUGUI>();
        var entries = new List<AchievementEntry>();

        foreach (var pair in SceneData.achievementDetailsByID)
        {
            var id = pair.Key;
            var achievement = pair.Value;
            var entry = Instantiate(entryPrefab, listContentTransform).GetComponent<AchievementEntry>();
            entries.Add(entry);

            entry.GetComponent<LayoutElement>().preferredHeight = GameConstants.AchievementEntryVerticalSizePerScreenHeight * Screen.height;

            entry.TitleText.text = achievement.title;
            string description = achievement.description;
            bool unlocked = false;
            var hasSaveDataEntry = LocalSaveData.Instance.achievementsInfo.ContainsKey(id);
            if (hasSaveDataEntry)
            {
                unlocked = LocalSaveData.Instance.achievementsInfo[id].unlocked;
            }

            if (!unlocked && achievement.amountToUnlock > 0)
            {
                var progress = hasSaveDataEntry ? LocalSaveData.Instance.achievementsInfo[id].progressAmount : 0;
                description += $" ({progress}/{achievement.amountToUnlock})";
            }
            entry.SetUnlocked(unlocked);
            entry.DesriptionText.text = description;

            titleTextList.Add(entry.TitleText);
            descriptionTextList.Add(entry.DesriptionText);
        }

        RefreshFontSize(titleTextList);
        RefreshFontSize(descriptionTextList);

        StartCoroutine(ShowInOrder(entries));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach(Transform entryTransform in listContentTransform)
        {
            Destroy(entryTransform.gameObject);
        }
    }

    private IEnumerator ShowInOrder(List<AchievementEntry> entries)
    {
        foreach(var entry in entries)
        {
            entry.SetVisible(false);
        }

        yield return new WaitForEndOfFrame(); //Wait two frames for font size calculations to be complete
        yield return new WaitForEndOfFrame();
        foreach (var entry in entries)
        {
            entry.gameObject.SetActive(false); //To retrigger all OnEnable animations
        }

        foreach (var entry in entries)
        {
            entry.gameObject.SetActive(true); //To retrigger all OnEnable animations
            entry.SetVisible(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
