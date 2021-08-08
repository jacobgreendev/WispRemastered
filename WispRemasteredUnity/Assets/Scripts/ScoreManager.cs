using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score;
    private float timeElapsed;
    public event OnScoreUpdateEventHandler OnScoreUpdate;
    public event OnTimeElapsedUpdateEventHandler OnTimeElapsedUpdate;

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

    private void Update()
    {
        if(SceneData.levelJustCompleted == null)
        {
            timeElapsed += Time.deltaTime;
            OnTimeElapsedUpdate(timeElapsed);
        }
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

    public void UpdateHiScoreAndRecord()
    {
        var saveData = LocalSaveData.Instance;
        var levelScores = saveData.levelScores;
        UpdateRecord<int>(levelScores, SceneData.levelToLoad.chapterNumber, SceneData.levelToLoad.levelNumber, score, true);

        var levelTimes = saveData.levelTimesSeconds;
        UpdateRecord<float>(levelTimes, SceneData.levelToLoad.chapterNumber, SceneData.levelToLoad.levelNumber, timeElapsed, false);

        PlayerSaveManager.SaveFile();
    }

    public void UpdateRecord<T>(Dictionary<int, Dictionary<int,T>> recordDict, int chapterNumber, int levelNumber, T newRecord, bool higherWins = true) where T : IConvertible
    {
        if (!recordDict.ContainsKey(chapterNumber))
        {
            recordDict.Add(chapterNumber, new());
        }

        var currentChapterRecords = recordDict[chapterNumber];
        if (currentChapterRecords.ContainsKey(levelNumber))
        {
            T currentRecord = currentChapterRecords[levelNumber];
            float newRecordFloat = Convert.ToSingle(newRecord);
            float currentRecordFloat = Convert.ToSingle(currentRecord);
            if (CompareRecord(currentRecordFloat, newRecordFloat, higherWins))
            {
                currentChapterRecords[levelNumber] = newRecord;
            }
        }
        else
        {
            currentChapterRecords[levelNumber] = newRecord;
        }
    }

    private bool CompareRecord(float currentRecord, float newRecord, bool higherWins = true)
    {
        if (higherWins) return newRecord > currentRecord;
        else return newRecord < currentRecord;
    }


    public delegate void OnScoreUpdateEventHandler(int newScore);
    public delegate void OnTimeElapsedUpdateEventHandler(float newTime);
}