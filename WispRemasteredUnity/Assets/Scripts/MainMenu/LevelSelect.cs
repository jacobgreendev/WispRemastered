using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform gridTransform;
    [SerializeField] private LevelList[] levelLists;
    [SerializeField] private Button nextChapterButton, previousChapterButton;
    [SerializeField] private List<TextMeshProUGUI> buttonTexts;

    private int currentChapter = 1;

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
            currentChapter = SceneData.levelJustCompleted.chapterNumber;
            SceneData.levelJustCompleted = null;
        }
        else
        {
            currentChapter = 1;
        }
        LoadChapter(currentChapter);
    }

    void NextChapter()
    {
        if (currentChapter - 1 < levelLists.Length)
        {
            currentChapter++;
            LoadChapter(currentChapter);
        }
    }

    void PreviousChapter()
    {
        if (currentChapter - 1 > 0)
        {
            currentChapter--;
            LoadChapter(currentChapter);
        }
    }

    void LoadChapter(int chapterNumber)
    {
        previousChapterButton.interactable = (currentChapter - 1 > 0);
        nextChapterButton.interactable = (currentChapter - 1 < levelLists.Length - 1);

        DestroyAllLevelButtons();

        var numberTextList = new List<TextMeshProUGUI>();
        var hiscoreTextList = new List<TextMeshProUGUI>();

        foreach (var levelInfo in levelLists[chapterNumber - 1].levels)
        {
            var newButtonGameObject = Instantiate(buttonPrefab, gridTransform);
            var newButtonInfo = newButtonGameObject.GetComponent<LevelButton>();
            newButtonInfo.ChapterLevelNumberText.text = $"{chapterNumber}-{levelInfo.levelNumber}";
            var newButton = newButtonGameObject.GetComponent<Button>();

            //Lists to be used in font size calculations
            numberTextList.Add(newButtonInfo.ChapterLevelNumberText);
            hiscoreTextList.Add(newButtonInfo.HiScoreText);

            var levelScores = LocalSaveData.Instance.levelScores;
            var levelTimes = LocalSaveData.Instance.levelTimesSeconds;

            //Check if level has a score attached already, and set the hiscore text to that
            bool chapterHasScores = levelScores.ContainsKey(chapterNumber);
            bool levelHasScore = false;
            newButtonInfo.HiScoreText.text = GameConstants.LevelLockedText;  //set hiscore text to locked text is level is locked
            newButtonInfo.BestTimeText.enabled = false;
            if (chapterHasScores)
            {
                levelHasScore = levelScores[chapterNumber].ContainsKey(levelInfo.levelNumber);
                if (levelHasScore)
                {
                    newButtonInfo.HiScoreText.text = GameConstants.LevelHiScorePrefix + levelScores[chapterNumber][levelInfo.levelNumber].ToString();
                    newButtonInfo.BestTimeText.enabled = true;
                    hiscoreTextList.Add(newButtonInfo.BestTimeText);
                    newButtonInfo.BestTimeText.text = TimeUtilities.GetMinuteSecondRepresentation(Mathf.Floor(levelTimes[chapterNumber][levelInfo.levelNumber]));
                }
            }

            bool levelUnlocked = false;
            if (levelHasScore)
            {
                levelUnlocked = true; //If level has score it must be unlocked, so checks can be skipped
            }
            else
            {
                var isFirstLevel = chapterNumber == 1 && levelInfo.levelNumber == 1; //Unlock first level by default
                if (isFirstLevel)
                {
                    levelUnlocked = true;
                }
                else if (levelInfo.levelNumber == 1 && levelScores.ContainsKey(chapterNumber - 1)) //if first level of a chapter and previous chapter has scores
                {
                    levelUnlocked = levelScores[chapterNumber - 1].ContainsKey(levelScores[chapterNumber - 1].Count - 1); //if final level of previous chapter is beaten
                }
                else if (levelScores.ContainsKey(chapterNumber)) //if current chapter has scores
                {
                    levelUnlocked = levelScores[chapterNumber].ContainsKey(levelInfo.levelNumber - 1); //if previous level in this chapter is beaten
                }
            }
            
            
            if (levelUnlocked)
            {
                newButton.onClick.AddListener(delegate { LoadLevel(levelInfo); });
                if (!levelHasScore)
                {
                    newButtonInfo.HiScoreText.text = GameConstants.LevelUnlockedAndUnplayedText; //most recent unlocked level
                }
            }
            else
            {
                newButton.interactable = false;
            }
        }

        RefreshFontSize(numberTextList);
        RefreshFontSize(hiscoreTextList);
    }

    void DestroyAllLevelButtons()
    {
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }
    }

    void RefreshFontSize(List<TextMeshProUGUI> tmpList)
    {
        StartCoroutine(RefreshFontSizeRoutine(tmpList));
    }

    private IEnumerator RefreshFontSizeRoutine(List<TextMeshProUGUI> tmpList)
    {
        float smallest = Mathf.Infinity;
        yield return new WaitForEndOfFrame();
        foreach (var text in tmpList)
        {
            text.ForceMeshUpdate();
            if (text.fontSize < smallest)
            {
                smallest = text.fontSize;
            }
        }

        foreach (var text in tmpList)
        {
            text.fontSize = smallest;
            text.enableAutoSizing = false;
        }
    }

    void LoadLevel(LevelInfo levelInfo)
    {
        SceneData.levelToLoad = levelInfo;
        SceneManager.LoadScene("LevelLoader");
    }
}
