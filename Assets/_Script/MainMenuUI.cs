using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] ToggleGroup difficultyToggleGroup;
    [SerializeField] ToggleGroup skillLevelToggleGroup;

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


        //Get skill level toggle
        IEnumerator<Toggle> skillLevelToggleEnum = skillLevelToggleGroup.ActiveToggles().GetEnumerator();
        skillLevelToggleEnum.MoveNext();
        Toggle skillLevelToggle = skillLevelToggleEnum.Current;

        switch (skillLevelToggle.tag)
        {
            case "NoviceToggleTag":
                GlobalData.instance.skillLevel = GlobalData.ESkillLevel.Novice;
                break;

            case "AdeptToggleTag":
                GlobalData.instance.skillLevel = GlobalData.ESkillLevel.Adept;
                break;

            case "MasterToggleTag":
                GlobalData.instance.skillLevel = GlobalData.ESkillLevel.Master;
                break;
        }

        SceneManager.LoadScene(gameSceneName);
    }
}
