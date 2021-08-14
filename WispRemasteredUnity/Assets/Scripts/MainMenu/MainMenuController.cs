using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton, levelSelectBackButton, achievementsButton, achievementsBackButton;
    [SerializeField] private GameObject mainMenuPanel, levelSelectPanel, achievementsPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(ShowLevelSelect);
        levelSelectBackButton.onClick.AddListener(ShowMainMenu);
        achievementsButton.onClick.AddListener(ShowAchievements);
        achievementsBackButton.onClick.AddListener(ShowMainMenu);
        if (SceneData.levelJustCompleted == null)
        {
            ShowMainMenu();
        }
        else
        {
            ShowLevelSelect();
        }
        PlayerSaveManager.LoadSaveFile();
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        achievementsPanel.SetActive(false);
    }

    private void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        achievementsPanel.SetActive(false);
    }

    private void ShowAchievements()
    {
        achievementsPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }
}
