using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    const float FADE_TIME = 2.5f;
    /// <summary>
    /// UnityChanがSpotAreaに留まっているフラグ
    /// </summary>
    public bool StaySpotArea { set; get; }

    /// <summary>
    /// UnityChanが経過時間内SpotAreaにいたかの判断フラグ
    /// </summary>
    public bool GameClearFlg
    {
        get
        {
            return _gameClearFlg;
        }
    }

    /// <summary>
    /// UnityChanが敵と接触、ステージ場外に落ちたときの判断フラグ
    /// </summary>
    public bool GameOverFlg
    {
        get
        {
            return _gameOverFlg;
        }
    }

    /// <summary>
    /// リザルト画面で文字を表示する際の判断フラグ
    /// </summary>
    //public bool ResultFlg
    //{
    //    get
    //    {
    //        return _resultFlg;
    //    }
    //}

    //public bool ResultFlg
    //{
    //    get; set;
    //}

    private bool _gameOverFlg;
    private bool _gameClearFlg;
    private bool _resultFlg;
    private FadeManager _fadeManager;
    private StageManager _stageManager;

    private UnityEvent _gameOverFlgEvent = new UnityEvent();
    private UnityEvent _gameClearFlgEvent = new UnityEvent();

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        _fadeManager = FadeManager.Instance;
        _fadeManager.SetFadeOutFlgEvent();

        // インスタンスが見つからない場合は破棄
        if (this != GameManager.Instance)
        {
            //Debug.Log("Destroy");
            Destroy(this.gameObject);
        }
        else
        {
            //Debug.Log("DontDestroyOnLoad");
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        // ゴールフラグ初期化
        _gameClearFlg = false;
        _gameOverFlg = false;
        _resultFlg = false;

        StaySpotArea = false;

        _stageManager = StageManager.Instance;
    }

    // GameOverFlgイベント関連処理
    public void SetGameOverFlgEvent()
    {
        _gameOverFlgEvent.AddListener(SetGameOverFlg);
    }
    // GameOverFlgイベント関連処理
    public void CallGameOverFlgEvent()
    {
        _gameOverFlgEvent.Invoke();
    }
    // GameOverFlgイベント関連処理
    public void RemoveGameOverFlgEvent()
    {
        _gameOverFlgEvent.RemoveListener(SetGameOverFlg);
    }

    // GameClearFlgイベント関連処理
    public void SetGameClearFlgEvent()
    {
        _gameClearFlgEvent.AddListener(SetGameClearFlg);
    }
    // GameClearFlgイベント関連処理
    public void CallGameClearFlgEvent()
    {
        _gameClearFlgEvent.Invoke();
    }
    // GameClearFlgイベント関連処理
    public void RemoveGameClearFlgEvent()
    {
        _gameClearFlgEvent.RemoveListener(SetGameClearFlg);
    }

    private void SetGameClearFlg()
    {
        //Debug.Log("SetGameClearFlg before : " + _gameClearFlg);

        // ゲームクリアフラグを更新
        if (_gameClearFlg)
        {
            _gameClearFlg = false;
        }
        else
        {
            _gameClearFlg = true;
        }


        if (GameClearFlg)
        {
            // 次シーンへ
            //_fadeManager.SceneName = "GameResult";

            // FadeOut開始待機時間
            Invoke("SetFedeFlg", FADE_TIME);
        }

        //Debug.Log("SetGameClearFlg after : " + _gameClearFlg);
    }

    private void SetGameOverFlg()
    {
        //Debug.Log("SetGameOverFlg before : " + _gameOverFlg);

        // ゲームオーバーフラグを更新
        if (_gameOverFlg)
        {
            _gameOverFlg = false;
        }
        else
        {
            _gameOverFlg = true;
        }

        // ゲームオーバー
        if (GameOverFlg)
        {
            // 次シーンへ
            //_fadeManager.SceneName = "GameResult";

            // FadeOut開始待機時間
            Invoke("SetFedeFlg", FADE_TIME);
        }

        //Debug.Log("SetGameOverFlg after : " + _gameOverFlg);
    }

    // UnityChanのモーション後にフェードする
    private void SetFedeFlg()
    {
        // 指定したシーンへフェードアウトする
        _fadeManager.CallFadeOutFlgEvent((int)FadeManager.NextScene.SCENE_GAME_RESULT);
        //Debug.Log("SetFadeFlg GameManager.cs");
    }
}

