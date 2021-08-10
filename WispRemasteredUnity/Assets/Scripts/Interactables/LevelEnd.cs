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
        ScoreManager.Instance.FinishLevel();
        SceneData.levelJustCompleted = SceneData.levelToLoad;
        SceneData.levelToLoad = null;
    }
}
