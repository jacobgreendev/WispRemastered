using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : Interactable
{
    public override void DoInteraction(PlayerController player)
    {
        player.Rigidbody.isKinematic = true;
        player.IsInteracting = true;
        ScoreManager.Instance.FinishLevel();
        SceneData.levelJustCompleted = SceneData.levelToLoad;
        SceneData.levelToLoad = null;
    }
}
