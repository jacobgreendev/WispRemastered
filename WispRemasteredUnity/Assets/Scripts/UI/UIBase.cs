using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected void RefreshFontSize(List<TextMeshProUGUI> tmpList)
    {
        StartCoroutine(RefreshFontSizeRoutine(tmpList));
    }

    private IEnumerator RefreshFontSizeRoutine(List<TextMeshProUGUI> tmpList)
    {
        float smallest = Mathf.Infinity;
        yield return new WaitForEndOfFrame();
        foreach (var text in tmpList)
        {
            text.ForceMeshUpdate();
            if (text.fontSize < smallest)
            {
                smallest = text.fontSize;
            }
        }

        foreach (var text in tmpList)
        {
            text.fontSize = smallest;
            text.enableAutoSizing = false;
        }
    }
}
