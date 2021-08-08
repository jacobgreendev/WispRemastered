using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton, levelSelectBackButton;
    [SerializeField] private GameObject mainMenuPanel, levelSelectPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(ShowLevelSelect);
        levelSelectBackButton.onClick.AddListener(ShowMainMenu);
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
    }

    private void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
}
