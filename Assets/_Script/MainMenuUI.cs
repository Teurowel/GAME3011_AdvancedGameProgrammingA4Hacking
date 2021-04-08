using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] ToggleGroup difficultyToggleGroup;

    [SerializeField] string gameSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClicked()
    {
        //Get difficulty toggle
        IEnumerator<Toggle> difficultyToggleEnum = difficultyToggleGroup.ActiveToggles().GetEnumerator();
        difficultyToggleEnum.MoveNext();
        Toggle difficultyToggle = difficultyToggleEnum.Current;

        switch (difficultyToggle.tag)
        {
            case "EasyToggleTag":
                GlobalData.instance.difficulty = GlobalData.EDifficulty.EASY;
                break;

            case "MediumToggleTag":
                GlobalData.instance.difficulty = GlobalData.EDifficulty.MEDIUM;
                break;

            case "HardToggleTag":
                GlobalData.instance.difficulty = GlobalData.EDifficulty.HARD;
                break;
        }

        SceneManager.LoadScene(gameSceneName);
    }
}
