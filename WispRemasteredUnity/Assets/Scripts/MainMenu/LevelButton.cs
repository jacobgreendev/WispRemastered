using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chapterLevelNumberText, hiScoreText, bestTimeText, newlyUnlockedText, lockedText;
    [SerializeField] private GameObject completedDisplay, lockedDisplay, newlyUnlockedDisplay;
    [SerializeField] private StarCounter scoreStars, timeStars;

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

    public int SetScoreStars<T>(float[] thresholds, T score, bool higherWins) where T : IConvertible
    {
        return scoreStars.SetStarAmount(thresholds, score, higherWins);
    }

    public int SetTimeStars<T>(float[] thresholds, T score, bool higherWins) where T : IConvertible
    {
        return timeStars.SetStarAmount(thresholds, score, higherWins);
    }
}

public enum LevelButtonLockState
{
    Completed,
    Locked,
    NewlyUnlocked
}