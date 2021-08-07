using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

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
        PlayerController.Instance.OnDeath += CheckForHiScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        LootLockerSDKManager.StartSession("TestDevice", (response) =>
        {
            Debug.Log("Started session success");
        });

        LootLockerSDKManager.SetPlayerName("TestPlayer", (response) =>
        {
            Debug.Log("Player name set successfully");
        });

        LootLockerSDKManager.GetScoreList(322, 1, (response) =>
        {
            if (response.success && response.items.Length > 0)
            {
                LootLockerLeaderboardMember topScore = response.items[0];
                Debug.Log($"Current record held by {topScore.player.name} is {topScore.score}");
            }
        });
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        PlayerController.Instance.OnLand -= AddScore;
        PlayerController.Instance.OnDeath -= CheckForHiScore;
        LootLockerSDKManager.EndSession("TestDevice", (response) =>
        {
            Debug.Log("Session end successful");
        });
    }

    private void AddScore(Interactable landedOn)
    {
        if (landedOn.ScoreValue > 0)
        {
            score += landedOn.ScoreValue;
            OnScoreUpdate?.Invoke(score);
        }
    }

    private void CheckForHiScore()
    {
        PlayerSaveManager.LoadSaveFile();
        var saveData = LocalSaveData.Instance;
        if (score > saveData.hiScore)
        {
            saveData.hiScore = score;
        }

        LootLockerSDKManager.SubmitScore("TestPlayer", score, 322, (response) =>
        {
            Debug.Log("Submitted score success");
        });

        PlayerSaveManager.SaveFile();
    }

    public delegate void OnScoreUpdateEventHandler(int newScore);
}
