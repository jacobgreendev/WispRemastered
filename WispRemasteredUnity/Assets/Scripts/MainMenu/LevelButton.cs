using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chapterLevelNumberText, hiScoreText, bestTimeText, newlyUnlockedText, lockedText;
    [SerializeField] private Image[] scoreStars, timeStars;
    [SerializeField] private Color scoreStarEarnedColor, timeStarEarnedColor, starUnearnedColor;
    [SerializeField] private GameObject completedDisplay, lockedDisplay, newlyUnlockedDisplay;

    public TextMeshProUGUI ChapterLevelNumberText
    {
        get => chapterLevelNumberText;
    }

    public TextMeshProUGUI HiScoreText
    {
        get => hiScoreText;
    }

    public TextMeshProUGUI BestTimeText
    {
        get => bestTimeText;
    }

    public TextMeshProUGUI NewlyUnlockedText
    {
        get => newlyUnlockedText;
    }

    public TextMeshProUGUI LockedText
    {
        get => lockedText;
    }

    public void SetDisplayType(LevelButtonLockState lockState)
    {
        completedDisplay.SetActive(lockState == LevelButtonLockState.Completed);
        lockedDisplay.SetActive(lockState == LevelButtonLockState.Locked);
        newlyUnlockedDisplay.SetActive(lockState == LevelButtonLockState.NewlyUnlocked);
    }

    public void SetScoreStars(int amount)
    {
        SetStars(scoreStars, amount, scoreStarEarnedColor, starUnearnedColor);
    }

    public void SetTimeStars(int amount)
    {
        SetStars(timeStars, amount, timeStarEarnedColor, starUnearnedColor);
    }

    private void SetStars(Image[] stars, int amount, Color earnedColor, Color unearnedColor)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = i < amount ? earnedColor : unearnedColor;
        }
    }
}

public enum LevelButtonLockState
{
    Completed,
    Locked,
    NewlyUnlocked
}