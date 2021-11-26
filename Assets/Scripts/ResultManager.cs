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

        // 結果表示
        resultText = canvas.GetComponentInChildren<Text>();

        // パネル上のボタンテキスト
        buttonTexts = panel.GetComponentsInChildren<Text>();
        SetSelectButtonText();
    }

    private void SetSelectButtonText()
    {
        buttonTexts[0].text = "Retry!!";

        // ゲームクリアの場合
        if (gameManager.ResultFlg)
        {
            resultText.text = "Game Clear !!";

            // クリアしたステージがHard以外の場合はNextStageとする
            if (stageManager.SelectStageLevel != 2)
            {
                buttonTexts[1].text = "Next Stage";
            }
            // クリアしたステージがHardなら終了(Topに戻す)
            else
            {
                resultText.text = "Thank you for playing !!";
                buttonTexts[1].text = "Top";
            }
        }
        // ゲームオーバーの場合
        else
        {
            resultText.text = "Game Over...";
            buttonTexts[1].text = "Stage Select";
        }
    }

    public void RetryButtonClicked()
    {
        // Hardの場合、ステージを広げるため情報を再設定する
        if (2 == stageManager.SelectStageLevel)
        {
            stageManager.SetStageInfo();
        }

        fadeManager.FadeOutFlg = true;
        fadeManager.SceneName = "RunToTheSpot";        
        gameManager.ResultFlg = false;

        // 使いまわす
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

        // 使いまわす
        // Assets / Resources / SE / Title / PlayClick.wav
        AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        audioManager.playClickSE(audio);

        // Titleまたはステージ選択画面の場合
        if ("RunToTheSpot" != fadeManager.SceneName)
        {
            // Title用のBGMに変更
            audioManager.TitleBGM();
        }
    }
}
