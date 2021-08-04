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

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        PlayerController.Instance.OnLand += AddScore;
        PlayerController.Instance.OnDeath += CheckForHiScore;
    }

    private void AddScore(Interactable landedOn)
    {
        score += landedOn.ScoreValue;
        OnScoreUpdate?.Invoke(score);
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
