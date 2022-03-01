using UnityEngine;

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

        // Assets / Resources / SE / Title / PlayClick.wav
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_START);
    }

    private void OnDisable()
    {
        _fadeManager.RemoveFadeOutFlgEvent();
    }
}