using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText, descriptionText;
    [SerializeField] private Color unlockedColor, lockedColor;

    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public TextMeshProUGUI TitleText
    {
        get => titleText;
    }

    public TextMeshProUGUI DesriptionText
    {
        get => descriptionText;
    }

    public void SetUnlocked(bool unlocked, float progress = 0, float maxProgress = 0)
    {
        background.color = unlocked ? unlockedColor : lockedColor;
    }

    public void SetVisible(bool visible)
    {
        background.enabled = visible;
        titleText.alpha = visible ? 1 : 0;
        descriptionText.alpha = visible ? 1 : 0;
    }
}
