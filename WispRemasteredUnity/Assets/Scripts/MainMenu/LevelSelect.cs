using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform gridTransform;
    [SerializeField] private LevelList levelList;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(var levelInfo in levelList.levels)
        {
            var newButton = Instantiate(buttonPrefab, gridTransform); ;
            newButton.GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(levelInfo); });
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = levelInfo.levelNumber.ToString();
        }
    }

    void LoadLevel(LevelInfo levelInfo)
    {
        SceneData.levelToLoad = levelInfo;
        SceneManager.LoadScene("LevelLoader");
    }
}
