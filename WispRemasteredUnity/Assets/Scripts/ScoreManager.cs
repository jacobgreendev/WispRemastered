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
        var levelRecords = saveData.levelRecords;
        var levelID = $"{SceneData.levelToLoad.chapterNumber}-{SceneData.levelToLoad.levelNumber}";

        if (levelRecords.ContainsKey(levelID))
        {
            var records = levelRecords[levelID];

            //Update Score Record
            var currentScoreRecord = records.hiScore;
            if (score > currentScoreRecord)
            {
                records.hiScore = score;
            }

            //Update Time Record
            var currentTimeRecord = records.timeRecord;
            if (timeElapsed < currentTimeRecord)
            {
                records.timeRecord = timeElapsed;
            }
        }
        else
        {
            //If no LevelRecordInfo present, create a new one and set both records
            levelRecords[levelID] = new();
            levelRecords[levelID].hiScore = score;
            levelRecords[levelID].timeRecord = timeElapsed;
        }

        PlayerSaveManager.SaveFile();
    }


    public delegate void OnScoreUpdateEventHandler(int newScore);
    public delegate void OnTimeElapsedUpdateEventHandler(float newTime);
}