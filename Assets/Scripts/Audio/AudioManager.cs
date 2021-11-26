using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //�V���O���g���ݒ肱������
    static public AudioManager instance;

    AudioSource [] BGM;
    AudioSource clickSE;
    AudioSource spotAreaSE;
 
    public string CurrentSceneName { set; get; }

    // �V���O���g����Scene���ׂ��ł��I�u�W�F�N�g�͎c���悤�ɂ���
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
        // [0]��SE�p�̃I�[�f�B�I�N���b�v���i�[����
        // [1]��Title�`StageSelect�܂ł�BGM
        // [2]��Play����BGM
        BGM = GetComponents<AudioSource>();
        CurrentSceneName = SceneManager.GetActiveScene().name;
    }

    private void ClickPlaySE(AudioClip audioClip)
    {
        clickSE = GetComponent<AudioSource>();
        clickSE.clip = audioClip;
        clickSE.Play();
    }
    // �S��1�ɂ܂Ƃ߂���
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

    // �V�[�����؂�ւ�������ɌĂ΂�郁�\�b�h�@
    public void OnActiveSceneChanged(string nextSceneName)//(Scene nextScene)
    {
        // �X�e�[�W�Z���N�g��ʂ���X�e�[�W��ʂɑJ�ڂ���Ƃ��ABGM��؂�ւ���
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
