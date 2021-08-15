using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AchievementHandler : MonoBehaviour
{
    public static AchievementHandler Instance;

    [SerializeField] private AchievementList achievementDetailsList;

    private PlayerController player;
    private GameUIController uiController;
    private DragManager dragManager;
    private ScoreManager scoreManager;

    private float storedDragPercentage, storedTimeoutTimer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
            SceneManager.sceneLoaded += SubscribeToEvents;
            SubscribeToEvents(SceneManager.GetActiveScene(), LoadSceneMode.Single);

            SceneData.achievementDetailsByID = new();
            foreach (var achievement in achievementDetailsList.list)
            {
                SceneData.achievementDetailsByID.Add(achievement.id, achievement);
            }

            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void SubscribeToEvents(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (PlayerController.Instance != null)
        {
            player = PlayerController.Instance;
            player.OnTimeoutTimerUpdate += StoreTimeoutTimer;
            player.OnFire += StoreDragPercentage;
            player.OnLand += CheckJustInTime;
            player.OnLand += CheckFullPower;
            player.OnLand += AddToLandCount;
        }

        if (GameUIController.Instance != null)
        {
            uiController = GameUIController.Instance;
        }

        if (DragManager.Instance != null)
        {
            dragManager = DragManager.Instance;
        }
    }

    private void UnlockAchievement(string id)
    {
        if (SceneData.achievementDetailsByID.ContainsKey(id))
        {
            if (LocalSaveData.Instance.achievementsInfo.ContainsKey(id))
            {
                if (!LocalSaveData.Instance.achievementsInfo[id].unlocked)
                {
                    LocalSaveData.Instance.achievementsInfo[id].unlocked = true;
                    GameUIController.Instance.ShowAchievementPopup(SceneData.achievementDetailsByID[id].title);
                }
            }
            else
            {
                LocalSaveData.Instance.achievementsInfo.Add(id, new AchievementInfo(id, true));
            }

            PlayerSaveManager.SaveFile();
        }
    }

    private void AddAchievementProgress(string id, int amount)
    {
        if (SceneData.achievementDetailsByID.ContainsKey(id))
        {
            var details = SceneData.achievementDetailsByID[id];
            float currentAmount;
            if (LocalSaveData.Instance.achievementsInfo.ContainsKey(id))
            {
                currentAmount = LocalSaveData.Instance.achievementsInfo[id].progressAmount + amount;
                LocalSaveData.Instance.achievementsInfo[id].progressAmount = currentAmount;
            }
            else
            {
                currentAmount = amount;
                LocalSaveData.Instance.achievementsInfo.Add(id, new AchievementInfo(id, false, amount, details.amountToUnlock));
            }

            if (currentAmount >= details.amountToUnlock)
            {
                UnlockAchievement(id);
            }
            else
            {
                PlayerSaveManager.SaveFile();
            }
        }
    }

    private void AddToLandCount(Interactable landedOn)
    {
        AddAchievementProgress("Land10", 1);
        AddAchievementProgress("Land100", 1);
        AddAchievementProgress("Land250", 1);
        AddAchievementProgress("Land500", 1);
    }

    private void StoreDragPercentage()
    {
        storedDragPercentage = dragManager.DragVector.magnitude / dragManager.MaxDragDistance;
    }

    private void StoreTimeoutTimer(float time)
    {
        storedTimeoutTimer = time;
    }

    private void CheckJustInTime(Interactable landedOn)
    {
        if (storedTimeoutTimer > 0.3)
        {
            UnlockAchievement("JustInTime");
        }
    }

    private void CheckFullPower(Interactable landedOn)
    {

    }
}
