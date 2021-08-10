using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : SimpleLandable
{
    public override void DoInteraction(PlayerController player)
    {
        Debug.Log($"levelToLoad = {SceneData.levelToLoad}");
        base.DoInteraction(player);
        player.IsInteracting = true;
        StartCoroutine(WaitAndEndLevel());
    }

    private IEnumerator WaitAndEndLevel()
    {
        ScoreManager.Instance.UpdateHiScoreAndRecord();
        SceneData.levelJustCompleted = SceneData.levelToLoad;
        SceneData.levelToLoad = null;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }
}
