using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    private GameManager _gameManager;
    private StageManager _stageManager;
    private FadeManager _fadeManager;
    private AudioManager _audioManager;

    [SerializeField]
    private GameObject _canvas; 

    [SerializeField]
    private GameObject _panel;

    private Text _resultText = null;
    private Text[] _buttonTexts = null;

    void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        _fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();

        // 結果表示
        _resultText = _canvas.GetComponentInChildren<Text>();

        // パネル上のボタンテキスト
        _buttonTexts = _panel.GetComponentsInChildren<Text>();
        SetSelectButtonText();
    }

    private void SetSelectButtonText()
    {
        _buttonTexts[0].text = "Retry!!";

        // ゲームクリアの場合
        if (_gameManager.ResultFlg)
        {
            _resultText.text = "Game Clear !!";

            // クリアしたステージがHard以外の場合はNextStageとする
            if (_stageManager.SelectStageLevel != 2)
            {
                _buttonTexts[1].text = "Next Stage";
            }
            // クリアしたステージがHardなら終了(Topに戻す)
            else
            {
                _resultText.text = "Thank you for playing !!";
                _buttonTexts[1].text = "Top";
            }
        }
        // ゲームオーバーの場合
        else
        {
            _resultText.text = "Game Over...";
            _buttonTexts[1].text = "Stage Select";
        }
    }

    public void RetryButtonClicked()
    {
        // Hardの場合、ステージを広げるため情報を再設定する
        if (2 == _stageManager.SelectStageLevel)
        {
            _stageManager.SetStageInfo();
        }

        _fadeManager.FadeOutFlg = true;
        _fadeManager.SceneName = "RunToTheSpot";
        _gameManager.ResultFlg = false;

        // 使いまわす
        // Assets / Resources / SE / Title / PlayClick.wav
        AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        _audioManager.playClickSE(audio);

        _gameManager.GameClearFlgSet(false);
        _gameManager.GameOverFlgSet(false);
        _gameManager.StaySpotArea = false;
    }

    public void SelectButtonClicked()
    {
        if (_gameManager.ResultFlg)
        {
            switch (_stageManager.SelectStageLevel)
            {
                // Next Stage
                case 0:
                    _fadeManager.SceneName = "RunToTheSpot";
                    _stageManager.SelectStageLevel += 1;
                    break;
                // Next Stage
                case 1:
                    _fadeManager.SceneName = "RunToTheSpot";
                    _stageManager.SelectStageLevel += 1;
                    break;
                // Top
                case 2:
                    _fadeManager.SceneName = "TitleScene";
                    break;
                default:
                    break;
            }

            _stageManager.SetStageInfo();
        }
        else
        {
            _fadeManager.SceneName = "StageSelect";
        }

        _stageManager.SetStageInfo();

        _fadeManager.FadeOutFlg = true;
        _gameManager.ResultFlg = false;

        _gameManager.GameClearFlgSet(false);
        _gameManager.GameOverFlgSet(false);
        _gameManager.StaySpotArea = false;

        // 使いまわす
        // Assets / Resources / SE / Title / PlayClick.wav
        AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        _audioManager.playClickSE(audio);

        // Titleまたはステージ選択画面の場合
        if ("RunToTheSpot" != _fadeManager.SceneName)
        {
            // Title用のBGMに変更
            _audioManager.TitleBGM();
        }
    }
}
