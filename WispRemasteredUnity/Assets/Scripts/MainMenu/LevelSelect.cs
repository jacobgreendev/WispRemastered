using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : UIBase
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform gridTransform;
    [SerializeField] private LevelList[] levelLists;
    [SerializeField] private Button nextChapterButton, previousChapterButton;
    [SerializeField] private TextMeshProUGUI chapterStarsText;
    [SerializeField] private List<TextMeshProUGUI> buttonTexts;

    private int currentChapter, currentChapterStars;

    private void Awake()
    {
        nextChapterButton.onClick.AddListener(NextChapter);
        previousChapterButton.onClick.AddListener(PreviousChapter);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        RefreshFontSize(buttonTexts);
        if(SceneData.levelJustCompleted != null)
        {
            currentChapter = SceneData.chapterLoaded;
            SceneData.levelJustCompleted = null;
        }
        else
        {
            currentChapter = 0;
        }
        LoadChapter(currentChapter);
    }

    void NextChapter()
    {
        if (currentChapter < levelLists.Length - 1)
        {
            currentChapter++;
            LoadChapter(currentChapter);
        }
    }

    void PreviousChapter()
    {
        if (currentChapter> 0)
        {
            currentChapter--;
            LoadChapter(currentChapter);
        }
    }

    void LoadChapter(int chapterIndex)
    {
        previousChapterButton.interactable = (currentChapter > 0);

        DestroyAllLevelButtons();

        var numberTextList = new List<TextMeshProUGUI>();
        var hiscoreTextList = new List<TextMeshProUGUI>();
        var newLockedTextList = new List<TextMeshProUGUI>();

        currentChapterStars = 0;

        var levels = levelLists[chapterIndex].levels;
        for (int levelIndex = 0; levelIndex < levels.Count; levelIndex++)
        {
            var levelInfo = levels[levelIndex];

            var newButtonGameObject = Instantiate(buttonPrefab, gridTransform);
            var newButtonInfo = newButtonGameObject.GetComponent<LevelButton>();
            newButtonInfo.ChapterLevelNumberText.text = $"{chapterIndex + 1}-{levelIndex + 1}";
            var newButton = newButtonGameObject.GetComponent<Button>();

            //Lists to be used in font size calculations
            numberTextList.Add(newButtonInfo.ChapterLevelNumberText);

            var levelRecords = LocalSaveData.Instance.levelRecords;

            //Check if level has a score attached already, and set the hiscore text to that
            var levelID = levelInfo.levelID;
            bool levelHasRecord = levelRecords.ContainsKey(levelID);
            var levelUnlocked = false;
            if (levelHasRecord)
            {
                SetLevelButtonCompleted(newButtonInfo, levelRecords[levelID], levelInfo);
                hiscoreTextList.Add(newButtonInfo.HiScoreText);
                hiscoreTextList.Add(newButtonInfo.BestTimeText);
                levelUnlocked = true;
            }
            else
            {
                var isFirstLevel = levelIndex == 0 ; //Unlock first level by default
                if (isFirstLevel)
                {
                    levelUnlocked = true;
                }
                else 
                {
                    if (levelIndex > 0 && levelRecords.ContainsKey(levels[levelIndex - 1].levelID))
                    {
                        levelUnlocked = true;//if previous level in this chapter is beaten
                    }
                }
            }
            
            
            if (levelUnlocked)
            {
                newButton.onClick.AddListener(delegate { LoadLevel(levelInfo, chapterIndex); });
                if (!levelHasRecord)
                {
                    SetLevelButtonNewlyUnlocked(newButtonInfo);
                    newLockedTextList.Add(newButtonInfo.NewlyUnlockedText);
                }
            }
            else
            {
                SetLevelButtonLocked(newButtonInfo, newButton);
                newLockedTextList.Add(newButtonInfo.LockedText);
            }
        }

        var nextChapterExists = (currentChapter < levelLists.Length - 1);
        var nextChapterUnlocked =  (currentChapterStars >= levels.Count * GameConstants.AverageLevelStarsForChapterUnlock);
        if (nextChapterExists && !nextChapterUnlocked)
        {
            chapterStarsText.text = $"{currentChapterStars}/{levels.Count * GameConstants.AverageLevelStarsForChapterUnlock}";
        }
        else
        {
            chapterStarsText.text = currentChapterStars.ToString();
        }
        nextChapterButton.interactable = nextChapterExists && nextChapterUnlocked;
        RefreshFontSize(buttonTexts);
        RefreshFontSize(numberTextList);
        RefreshFontSize(hiscoreTextList);
        RefreshFontSize(newLockedTextList);
    }

    void DestroyAllLevelButtons()
    {
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }
    }

    void SetLevelButtonLocked(LevelButton buttonInfo, Button button)
    {
        button.interactable = false;
        buttonInfo.SetDisplayType(LevelButtonLockState.Locked);
    }

    void SetLevelButtonCompleted(LevelButton buttonInfo, LevelRecordInfo records, LevelInfo levelInfo)
    {
        buttonInfo.SetDisplayType(LevelButtonLockState.Completed);

        //Set hiscore text and hi score star amount
        buttonInfo.HiScoreText.text = records.hiScore.ToString();
        var scoreStarAmount = buttonInfo.SetScoreStars(levelInfo.scoreStarInfo.starThresholds, records.hiScore, true);
        currentChapterStars += scoreStarAmount;

        //Enable and set best time text 
        buttonInfo.BestTimeText.enabled = true;
        buttonInfo.BestTimeText.text = TimeUtilities.GetMinuteSecondRepresentation(Mathf.Floor(records.timeRecord));
        var timeStarAmount = buttonInfo.SetTimeStars(levelInfo.timeSecondsStarInfo.starThresholds, records.timeRecord, false);
        currentChapterStars += timeStarAmount;
    }

    void SetLevelButtonNewlyUnlocked(LevelButton buttonInfo)
    {
        buttonInfo.SetDisplayType(LevelButtonLockState.NewlyUnlocked);
    }

    void LoadLevel(LevelInfo levelInfo, int chapterIndexOfLoaded)
    {
        SceneData.levelToLoad = levelInfo;
        SceneData.chapterLoaded = chapterIndexOfLoaded;
        SceneManager.LoadScene("LevelLoader");
    }
}
