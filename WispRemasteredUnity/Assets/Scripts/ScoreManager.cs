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
        PlayerController.Instance.OnLand += AddScoreOnLand;
        PlayerController.Instance.OnDeath += CheckForHiScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    private void OnDisable()
    {
        //Unsubscribe from all events
        PlayerController.Instance.OnLand -= AddScoreOnLand;
        PlayerController.Instance.OnDeath -= CheckForHiScore;
    }

    private void AddScoreOnLand(Interactable landedOn)
    {
        if (landedOn.ScoreValue > 0)
        {
            score += landedOn.ScoreValue;
            OnScoreUpdate?.Invoke(score);
        }
    }

    public void AddScoreWithMessage(int added, string message)
    {
        score += added;
        UIManager.Instance.ShowPopup($"{message} (+{added})");
    }

    private void CheckForHiScore()
    {
        PlayerSaveManager.LoadSaveFile();
        var saveData = LocalSaveData.Instance;
        if(score > saveData.hiScore)
        {
            saveData.hiScore = score;
        }
        PlayerSaveManager.SaveFile();
    }

    public delegate void OnScoreUpdateEventHandler(int newScore);
}
