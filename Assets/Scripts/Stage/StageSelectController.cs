using UnityEngine;

public class StageSelectController : MonoBehaviour
{
    private AudioManager _audioManager;
    private StageManager _stageManager;
    private FadeManager _fadeManager;

    private void Awake()
    {
        _fadeManager = FadeManager.Instance;
        _fadeManager.SetFadeOutFlgEvent();
    }

    private void Start()
    {
        _stageManager = StageManager.Instance;
        _stageManager.SelectStageLevel = 0;
        _audioManager = AudioManager.Instance;
    }

    public void OnEasyButtonClicked()
    {
        // Level 1 (easy)
        _stageManager.SelectStageLevel = (int)StageManager.StageLevel.EASY;
        _stageManager.SetStageInfo();

        // Assets / Resources / SE / StageSelect / EasyClick.wav
        //_audioClip = Resources.Load("SE/StageSelect/EasyClick") as AudioClip;

        //_audioManager.EasyClickSE(_audioClip);
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_EASY);
        SelectToStage();
    }

    public void OnNormalButtonClicked()
    {
        // Level 2 (Normal)
        _stageManager.SelectStageLevel = (int)StageManager.StageLevel.NORMAL;
        _stageManager.SetStageInfo();
        // Assets / Resources / SE / StageSelect / NormalClick.wav
        //_audioClip = Resources.Load("SE/StageSelect/NormalClick") as AudioClip;

        //_audioManager.NormalClickSE(_audioClip);
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_NORMAL);
        SelectToStage();
    }

    public void OnHardButtonClicked()
    {
        // Level 3 (Hard)
        _stageManager.SelectStageLevel = (int)StageManager.StageLevel.HARD;
        _stageManager.SetStageInfo();

        // Assets / Resources / SE / StageSelect / HardClick.wav
        //_audioClip = Resources.Load("SE/StageSelect/HardClick") as AudioClip;

        //_audioManager.HardClickSE(_audioClip);
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_HARD);
        SelectToStage();
    }

    public void OnBackButtonClicked()
    {
        // Assets / Resources / SE / StageSelect / BackClick.wav
        //_audioClip = Resources.Load("SE/StageSelect/BackClick") as AudioClip;

        //_audioManager.BackClickSE(_audioClip);
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_BACK);
        
        _fadeManager.CallFadeOutFlgEvent((int)FadeManager.NextScene.SCENE_TITLE);
    }

    public void OnTutorialButtonClicked()
    {
        _stageManager.SelectStageLevel = (int)StageManager.StageLevel.TUTORIAL;
        _stageManager.SetStageInfo();
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_NORMAL);
        _audioManager.PlayMusicBGM((int)AudioManager.PlayBGM.BGM_PLAY_SCENE);

        _fadeManager.CallFadeOutFlgEvent((int)FadeManager.NextScene.SCENE_TUTORIAL);
    }

    private void SelectToStage()
    {
        _audioManager.PlayMusicBGM((int)AudioManager.PlayBGM.BGM_PLAY_SCENE);
        _fadeManager.CallFadeOutFlgEvent((int)FadeManager.NextScene.SCENE_ESCAPE_TO_THE_SPOT);
    }

    private void OnDisable()
    {
        _fadeManager.RemoveFadeOutFlgEvent();
    }
}
