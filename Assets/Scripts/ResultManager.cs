using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    private GameManager gameManager;
    private StageManager stageManager;
    private FadeManager fadeManager;
    private AudioManager audioManager;

    [SerializeField]
    private GameObject canvas; 

    [SerializeField]
    private GameObject panel;

    private Text resultText = null;
    private Text []buttonTexts = null;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();

        // ���ʕ\��
        resultText = canvas.GetComponentInChildren<Text>();

        // �p�l����̃{�^���e�L�X�g
        buttonTexts = panel.GetComponentsInChildren<Text>();
        SetSelectButtonText();
    }

    private void SetSelectButtonText()
    {
        buttonTexts[0].text = "Retry!!";

        // �Q�[���N���A�̏ꍇ
        if (gameManager.ResultFlg)
        {
            resultText.text = "Game Clear !!";

            // �N���A�����X�e�[�W��Hard�ȊO�̏ꍇ��NextStage�Ƃ���
            if (stageManager.SelectStageLevel != 2)
            {
                buttonTexts[1].text = "Next Stage";
            }
            // �N���A�����X�e�[�W��Hard�Ȃ�I��(Top�ɖ߂�)
            else
            {
                resultText.text = "Thank you for playing !!";
                buttonTexts[1].text = "Top";
            }
        }
        // �Q�[���I�[�o�[�̏ꍇ
        else
        {
            resultText.text = "Game Over...";
            buttonTexts[1].text = "Stage Select";
        }
    }

    public void RetryButtonClicked()
    {
        // Hard�̏ꍇ�A�X�e�[�W���L���邽�ߏ����Đݒ肷��
        if (2 == stageManager.SelectStageLevel)
        {
            stageManager.SetStageInfo();
        }

        fadeManager.FadeOutFlg = true;
        fadeManager.SceneName = "RunToTheSpot";        
        gameManager.ResultFlg = false;

        // �g���܂킷
        // Assets / Resources / SE / Title / PlayClick.wav
        AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        audioManager.playClickSE(audio);

        gameManager.GameClearFlgSet(false);
        gameManager.GameOverFlgSet(false);
        gameManager.StaySpotArea = false;
    }

    public void SelectButtonClicked()
    {
        if (gameManager.ResultFlg)
        {
            switch (stageManager.SelectStageLevel)
            {
                // Next Stage
                case 0:
                    fadeManager.SceneName = "RunToTheSpot";
                    stageManager.SelectStageLevel += 1;
                    break;
                // Next Stage
                case 1:
                    fadeManager.SceneName = "RunToTheSpot";
                    stageManager.SelectStageLevel += 1;
                    break;
                // Top
                case 2:
                    fadeManager.SceneName = "TitleScene";
                    break;
                default:
                    break;
            }

            stageManager.SetStageInfo();
        }
        else
        {
            fadeManager.SceneName = "StageSelect";
        }

        stageManager.SetStageInfo();

        fadeManager.FadeOutFlg = true;
        gameManager.ResultFlg = false;

        gameManager.GameClearFlgSet(false);
        gameManager.GameOverFlgSet(false);
        gameManager.StaySpotArea = false;

        // �g���܂킷
        // Assets / Resources / SE / Title / PlayClick.wav
        AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        audioManager.playClickSE(audio);

        // Title�܂��̓X�e�[�W�I����ʂ̏ꍇ
        if ("RunToTheSpot" != fadeManager.SceneName)
        {
            // Title�p��BGM�ɕύX
            audioManager.TitleBGM();
        }
    }
}
