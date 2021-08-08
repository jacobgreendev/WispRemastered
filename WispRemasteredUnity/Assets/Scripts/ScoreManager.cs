using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score;
    public event OnScoreUpdateEventHandler OnScoreUpdate;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        PlayerController.Instance.OnLand += AddScore;
        PlayerController.Instance.OnDeath += UpdateEndlessHiScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        PlayerController.Instance.OnLand -= AddScore;
        PlayerController.Instance.OnDeath -= UpdateEndlessHiScore;
    }

    private void AddScore(Interactable landedOn)
    {
        if (landedOn.ScoreValue > 0)
        {
            score += landedOn.ScoreValue;
            OnScoreUpdate?.Invoke(score);
        }
    }

    private void UpdateEndlessHiScore()
    {
        var saveData = LocalSaveData.Instance;
        if(score > saveData.endlessHiScore)
        {
            saveData.endlessHiScore = score;
        }
        PlayerSaveManager.SaveFile();
    }

    public void UpdateLevelHiScore()
    {
        var saveData = LocalSaveData.Instance;
        var currentChapter = SceneData.levelToLoad.chapterNumber;
        var currentLevelNumber = SceneData.levelToLoad.levelNumber;

        if (!saveData.levelScores.ContainsKey(currentChapter))
        {
            saveData.levelScores.Add(currentChapter, new());
        }

        var currentChapterScores = saveData.levelScores[currentChapter];

        var currentHiScore = 0;

        if (currentChapterScores.ContainsKey(currentLevelNumber))
        {
            currentHiScore = currentChapterScores[currentLevelNumber];
        }

        if (score > currentHiScore)
        {
            currentChapterScores[currentLevelNumber] = score;
            PlayerSaveManager.SaveFile();
        }

    }

    public delegate void OnScoreUpdateEventHandler(int newScore);
}
