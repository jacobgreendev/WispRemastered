using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarCounter : UIBase
{
    [SerializeField] private Image[] stars;
    [SerializeField] private Color earnedColor, unearnedColor;
    [SerializeField] private TextMeshProUGUI[] starTexts;

    private int GetStarAmount<T>(float[] thresholds, T score, bool higherWins) where T : IConvertible
    {
        var value = Mathf.Floor(Convert.ToSingle(score)); //floored to match number representation
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (higherWins && value < thresholds[i])
            {
                return i; //If threshold not met, return index (one less than current amount as indexed from 0)
            }
            else if (!higherWins && value > thresholds[i])
            {
                return i;
            }
        }
        return thresholds.Length; //If all thresholds passed, return that amount
    }

    public int SetStarAmount<T>(float[] thresholds, T score, bool higherWins, bool convertTextToTime = false) where T : IConvertible
    {
        var amount = GetStarAmount<T>(thresholds, score, higherWins);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = i < amount ? earnedColor : unearnedColor;
            if(i < starTexts.Length)
            {
                starTexts[i].text = convertTextToTime ? TimeUtilities.GetMinuteSecondRepresentation(thresholds[i]) : thresholds[i].ToString();
            }
        }
        RefreshFontSize(starTexts.ToList());
        return amount;
    }
}
