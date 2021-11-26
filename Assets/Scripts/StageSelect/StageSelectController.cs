using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectController : MonoBehaviour
{
    private AudioManager audioManager;
    private StageManager stageManager;
    private FadeManager fadeManager;
    private AudioClip audioClip;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        stageManager.SelectStageLevel = 0;
  
        // AudioManagerのゲームオブジェクトを探す
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.CurrentSceneName = SceneManager.GetActiveScene().name;

        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
    }

    public void OnEasyButtonClicked()
    {
        // Level 1 (easy)
        stageManager.SelectStageLevel = 0;
        stageManager.SetStageInfo();

        // Assets / Resources / SE / StageSelect / EasyClick.wav
        audioClip = Resources.Load("SE/StageSelect/EasyClick") as AudioClip;

        audioManager.easyClickSE(audioClip);
        SelectToStage();
    }

    public void OnNormalButtonClicked()
    {
        // Level 2 (Normal)
        stageManager.SelectStageLevel = 1;
        stageManager.SetStageInfo();
        // Assets / Resources / SE / StageSelect / NormalClick.wav
        audioClip = Resources.Load("SE/StageSelect/NormalClick") as AudioClip;

        audioManager.normalClickSE(audioClip);
        SelectToStage();
    }

    public void OnHardButtonClicked()
    {
        // Level 3 (Hard)
        stageManager.SelectStageLevel = 2;
        stageManager.SetStageInfo();

        // Assets / Resources / SE / StageSelect / HardClick.wav
        audioClip = Resources.Load("SE/StageSelect/HardClick") as AudioClip;

        audioManager.hardClickSE(audioClip);
        SelectToStage();
    }

    public void OnBackButtonClicked()
    {
        // Assets / Resources / SE / StageSelect / BackClick.wav
        audioClip = Resources.Load("SE/StageSelect/BackClick") as AudioClip;

        audioManager.backClickSE(audioClip);

        fadeManager.SceneName = "StageSelect";

        fadeManager.FadeOutFlg = true;
        fadeManager.SceneName = "TitleScene";
    }

    void SelectToStage()
    {
        audioManager.OnActiveSceneChanged("RunToTheSpot");
       
        switch (stageManager.SelectStageLevel)
        {
            // Easy
            case 0:
                fadeManager.FadeOutFlg = true;
                fadeManager.SceneName = "RunToTheSpot";
                break;
            // Normal
            case 1:
                fadeManager.FadeOutFlg = true;
                fadeManager.SceneName = "RunToTheSpot";
                break;
            // Hard
            case 2:
                fadeManager.FadeOutFlg = true;
                fadeManager.SceneName = "RunToTheSpot";
                break;
            default:
                break;
        }
    }
}
