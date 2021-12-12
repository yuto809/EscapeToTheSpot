using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    private AudioManager _audioManager;
    private FadeManager _fadeManager;

    private void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _audioManager.CurrentSceneName = SceneManager.GetActiveScene().name;

        _fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
    }

    public void OnStartButtonClicked()
    {
        _fadeManager.FadeOutFlg = true;
        _fadeManager.SceneName = "StageSelect";

        // Assets / Resources / SE / Title / PlayClick.wav
        AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        _audioManager.playClickSE(audio);
    }
}