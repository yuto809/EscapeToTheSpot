using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    private AudioManager _audioManager;
    private FadeManager _fadeManager;

    private void Awake()
    {
        _fadeManager = FadeManager.Instance;
        _fadeManager.SetFadeOutFlgEvent();
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    public void OnStartButtonClicked()
    {
        _fadeManager.CallFadeOutFlgEvent((int)FadeManager.NextScene.SCENE_STAGE_SELECT);
        //_fadeManager.SceneName = "StageSelect";

        // Assets / Resources / SE / Title / PlayClick.wav
        //AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        //_audioManager.PlayClickSE(audio);
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_START);
    }

    private void OnDisable()
    {
        _fadeManager.RemoveFadeOutFlgEvent();
        Debug.Log("OnDisable TitleController.cs");
    }
}