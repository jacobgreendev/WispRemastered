using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chapterLevelNumberText, hiScoreText;

    public TextMeshProUGUI ChapterLevelNumberText
    {
        get => chapterLevelNumberText;
    }

    public TextMeshProUGUI HiScoreText
    {
        get => hiScoreText;
    }
}
