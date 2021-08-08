using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : SimpleLandable
{
    public override void DoInteraction(PlayerController player)
    {
        base.DoInteraction(player);
        player.IsInteracting = true;
        StartCoroutine(WaitAndEndLevel());
    }

    private IEnumerator WaitAndEndLevel()
    {
        ScoreManager.Instance.UpdateLevelHiScore();
        yield return new WaitForSeconds(2);
        SceneData.levelJustCompleted = SceneData.levelToLoad;
        SceneData.levelToLoad = null;
        SceneManager.LoadScene("MainMenu");
    }
}
