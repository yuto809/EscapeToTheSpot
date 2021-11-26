using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    private AudioManager audioManager;
    private FadeManager fadeManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.CurrentSceneName = SceneManager.GetActiveScene().name;

        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
    }

    public void OnStartButtonClicked()
    {
        fadeManager.FadeOutFlg = true;
        fadeManager.SceneName = "StageSelect";

        // Assets / Resources / SE / Title / PlayClick.wav
        AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        audioManager.playClickSE(audio);
    }
}