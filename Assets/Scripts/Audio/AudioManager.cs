using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //シングルトン設定ここから
    static public AudioManager instance;

    AudioSource [] BGM;
    AudioSource clickSE;
    AudioSource spotAreaSE;
 
    public string CurrentSceneName { set; get; }

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        // [0]はSE用のオーディオクリップを格納する
        // [1]はTitle～StageSelectまでのBGM
        // [2]はPlay中のBGM
        BGM = GetComponents<AudioSource>();
        CurrentSceneName = SceneManager.GetActiveScene().name;
    }

    private void ClickPlaySE(AudioClip audioClip)
    {
        clickSE = GetComponent<AudioSource>();
        clickSE.clip = audioClip;
        clickSE.Play();
    }
    // 全部1つにまとめられる
    public void playClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void easyClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void normalClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void hardClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void backClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void DamageSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    // シーンが切り替わった時に呼ばれるメソッド　
    public void OnActiveSceneChanged(string nextSceneName)//(Scene nextScene)
    {
        // ステージセレクト画面からステージ画面に遷移するとき、BGMを切り替える
        if (CurrentSceneName == "StageSelect" && nextSceneName == "RunToTheSpot")
        {
            BGM[1].Stop();
            Invoke("PlayModeBGM", 1.0f);
        }
    }

    void PlayModeBGM()
    {
        BGM[2].Play();
    }

    public void TitleBGM()
    {
        BGM[2].Stop();
        Invoke("PlayTitleBGM", 1.0f);
    }

    private void PlayTitleBGM()
    {
        BGM[1].Play();
    }
}
