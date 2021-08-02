using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score;

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

    private void AddScore(Transform landedOn)
    {
        score++;
        Debug.Log(score);
    }

    private void CheckForHiScore()
    {
        PlayerSaveManager.LoadSaveFile();
        if(score > LocalSaveData.save.hiScore)
        {
            LocalSaveData.save.hiScore = score;
        }
        PlayerSaveManager.SaveFile();
    }
}
