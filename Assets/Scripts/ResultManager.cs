using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultManager : MonoBehaviour
{
    private GameManager _gameManager;
    private StageManager _stageManager;
    private FadeManager _fadeManager;
    private AudioManager _audioManager;
    private UnityEvent _setGameOverFlgEvent;

    [SerializeField]
    private GameObject _canvas; 

    [SerializeField]
    private GameObject _panel;

    [SerializeField]
    private Button _retryButton;

    [SerializeField]
    private Button _SelectButton;

    [SerializeField]
    private string _textRetry = "Retry !";

    [SerializeField]
    private string _textTop = "Top";

    [SerializeField]
    private string _textNextStage = "Next Stage";

    [SerializeField]
    private string _textSelectStage = "Stage Select";

    [SerializeField]
    private string _textGameClear = "Game Clear !!";

    [SerializeField]
    private string _textGameOver = "Game Over...";

    [SerializeField]
    private string _textGameEnd = "Thank you for playing !!";


    //private Text[] _buttonTexts = null;

    private int _nextSceneName;

    private Text _resultText;
    private Text _retryButtonText;
    private Text _selectButtonText;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _fadeManager = FadeManager.Instance;
        _gameManager.SetGameClearFlgEvent();
        _gameManager.SetGameOverFlgEvent();
        _fadeManager.SetFadeOutFlgEvent();
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _stageManager = StageManager.Instance;

        // 結果表示
        _resultText = _canvas.GetComponentInChildren<Text>();

        // パネル上のボタンテキスト
        //_buttonTexts = _panel.GetComponentsInChildren<Text>();




        _retryButtonText = _retryButton.GetComponentInChildren<Text>();
        _selectButtonText = _SelectButton.GetComponentInChildren<Text>();




        SetSelectButtonText();
    }

    private void SetSelectButtonText()
    {
        //_buttonTexts[0].text = "Retry !";
        _retryButtonText.text = _textRetry;

        // ゲームクリアの場合
        // if (_gameManager.ResultFlg)
        if (_gameManager.GameClearFlg)
        {
            _resultText.text = _textGameClear;
            //_resultText.text = "Game Clear !!";

            // クリアしたステージがHardの場合はゲーム終了(Topに戻す)
            if (_stageManager.SelectStageLevel == (int)StageManager.StageLevel.HARD)
            {
                _resultText.text = _textGameEnd;
                _selectButtonText.text = _textTop;

                //_resultText.text = "Thank you for playing !!";
                //_buttonTexts[1].text = "Top";
            }
            else
            {
                _selectButtonText.text = _textNextStage;

                //_buttonTexts[1].text = "Next Stage";
            }
        }
        // ゲームオーバーの場合
        else
        {
            _resultText.text = _textGameOver;
            _selectButtonText.text = _textSelectStage;


            //_resultText.text = "Game Over...";
            //_buttonTexts[1].text = "Stage Select";
        }
    }

    public void RetryButtonClicked()
    {
        // Hardの場合、ステージを広げるため情報を再設定する
        if ((int)StageManager.StageLevel.HARD == _stageManager.SelectStageLevel)
        {
            _stageManager.SetStageInfo();
        }

        _nextSceneName = (int)FadeManager.NextScene.SCENE_ESCAPE_TO_THE_SPOT;
        _fadeManager.CallFadeOutFlgEvent(_nextSceneName);
        //_fadeManager.SceneName = "RunToTheSpot";

        // gameoverせずにクリアした場合は処理スキップ
        // gameoverの場合はフラグを再設定
        if (_gameManager.GameOverFlg == true)
        {
            _gameManager.CallGameOverFlgEvent();
        }

        // gameclearした場合はフラグを再設定
        if (_gameManager.GameClearFlg == true)
        {
            _gameManager.CallGameClearFlgEvent();
        }

        //_gameManager.ResultFlg = false;

        // 使いまわす
        // Assets / Resources / SE / Title / PlayClick.wav
        //AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        //_audioManager.PlayClickSE(audio);
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_START);

        _gameManager.StaySpotArea = false;
    }

    public void SelectButtonClicked()
    {
        if (_gameManager.GameClearFlg)
        {
            switch (_stageManager.SelectStageLevel)
            {
                // Next Stage
                case (int)StageManager.StageLevel.EASY:
                    _nextSceneName = (int)FadeManager.NextScene.SCENE_ESCAPE_TO_THE_SPOT;
                    //_fadeManager.SceneName = "RunToTheSpot";
                    _stageManager.SelectStageLevel += 1;
                    break;
                // Next Stage
                case (int)StageManager.StageLevel.NORMAL:
                    _nextSceneName = (int)FadeManager.NextScene.SCENE_ESCAPE_TO_THE_SPOT;
                    //_fadeManager.SceneName = "RunToTheSpot";
                    _stageManager.SelectStageLevel += 1;
                    break;
                // Top
                case (int)StageManager.StageLevel.HARD:
                    _nextSceneName = (int)FadeManager.NextScene.SCENE_TITLE;
                    //_fadeManager.SceneName = "TitleScene";
                    break;
                default:
                    break;
            }

            _stageManager.SetStageInfo();
        }
        else
        {
            _nextSceneName = (int)FadeManager.NextScene.SCENE_STAGE_SELECT;
            //_fadeManager.SceneName = "StageSelect";
        }

        _stageManager.SetStageInfo();

        //_fadeManager.FadeOutFlg = true;
        _fadeManager.CallFadeOutFlgEvent(_nextSceneName);

        // gameoverせずにクリアした場合は処理スキップ
        // gameoverの場合はフラグを再設定
        if (_gameManager.GameOverFlg == true)
        {
            _gameManager.CallGameOverFlgEvent();
        }

        // gameclearした場合はフラグを再設定
        if (_gameManager.GameClearFlg == true)
        {
            _gameManager.CallGameClearFlgEvent();
        }
        
        _gameManager.StaySpotArea = false;

        // 使いまわす
        // Assets / Resources / SE / Title / PlayClick.wav
        //AudioClip audio = Resources.Load("SE/Title/PlayClick") as AudioClip;
        //_audioManager.PlayClickSE(audio);
        _audioManager.PlayMusicSE((int)AudioManager.PlaySE.CLICK_START);

        // Titleまたはステージ選択画面の場合
        //if ("RunToTheSpot" != _fadeManager.SceneName)
        if (_fadeManager.SceneName != "RunToTheSpot")
        {
            // Title用のBGMに変更
            //_audioManager.TitleBGM();
            _audioManager.PlayMusicBGM((int)AudioManager.PlayBGM.BGM_TITLE_SCENE);
        }
    }

    // Awakeで登録したイベントを削除する
    private void OnDisable()
    {
        _gameManager.RemoveGameOverFlgEvent();
        _gameManager.RemoveGameClearFlgEvent();
        _fadeManager.RemoveFadeOutFlgEvent();

        //Debug.Log("OnDisable ResultManager.cs");
    }
}
