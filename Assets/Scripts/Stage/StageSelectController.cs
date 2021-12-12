using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectController : MonoBehaviour
{
    private AudioManager _audioManager;
    private StageManager _stageManager;
    private FadeManager _fadeManager;
    private AudioClip _audioClip;

    private void Start()
    {
        _stageManager = FindObjectOfType<StageManager>();
        _stageManager.SelectStageLevel = 0;

        // AudioManagerのゲームオブジェクトを探す
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _audioManager.CurrentSceneName = SceneManager.GetActiveScene().name;

        _fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
    }

    public void OnEasyButtonClicked()
    {
        // Level 1 (easy)
        _stageManager.SelectStageLevel = 0;
        _stageManager.SetStageInfo();

        // Assets / Resources / SE / StageSelect / EasyClick.wav
        _audioClip = Resources.Load("SE/StageSelect/EasyClick") as AudioClip;

        _audioManager.easyClickSE(_audioClip);
        SelectToStage();
    }

    public void OnNormalButtonClicked()
    {
        // Level 2 (Normal)
        _stageManager.SelectStageLevel = 1;
        _stageManager.SetStageInfo();
        // Assets / Resources / SE / StageSelect / NormalClick.wav
        _audioClip = Resources.Load("SE/StageSelect/NormalClick") as AudioClip;

        _audioManager.normalClickSE(_audioClip);
        SelectToStage();
    }

    public void OnHardButtonClicked()
    {
        // Level 3 (Hard)
        _stageManager.SelectStageLevel = 2;
        _stageManager.SetStageInfo();

        // Assets / Resources / SE / StageSelect / HardClick.wav
        _audioClip = Resources.Load("SE/StageSelect/HardClick") as AudioClip;

        _audioManager.hardClickSE(_audioClip);
        SelectToStage();
    }

    public void OnBackButtonClicked()
    {
        // Assets / Resources / SE / StageSelect / BackClick.wav
        _audioClip = Resources.Load("SE/StageSelect/BackClick") as AudioClip;

        _audioManager.backClickSE(_audioClip);

        _fadeManager.SceneName = "StageSelect";

        _fadeManager.FadeOutFlg = true;
        _fadeManager.SceneName = "TitleScene";
    }

    void SelectToStage()
    {
        _audioManager.OnActiveSceneChanged("RunToTheSpot");
       
        switch (_stageManager.SelectStageLevel)
        {
            // Easy
            case 0:
                _fadeManager.FadeOutFlg = true;
                _fadeManager.SceneName = "RunToTheSpot";
                break;
            // Normal
            case 1:
                _fadeManager.FadeOutFlg = true;
                _fadeManager.SceneName = "RunToTheSpot";
                break;
            // Hard
            case 2:
                _fadeManager.FadeOutFlg = true;
                _fadeManager.SceneName = "RunToTheSpot";
                break;
            default:
                break;
        }
    }
}
