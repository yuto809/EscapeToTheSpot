using UnityEngine;

public class GameManager : MonoBehaviour
{
    //シングルトン設定ここから
    static public GameManager Instance;

    // UnityChanがSpotAreaに留まっているフラグ
    public bool StaySpotArea { set; get; }

    // UnityChanが経過時間内SpotAreaにいたかの判断フラグ
    public bool GameClearFlg { set; get; }

    public bool GameOverFlg { set; get; }

    public bool ResultFlg { set; get; }
    
    [SerializeField]
    private ObstacleGenerator _objGen;

    private FadeManager _fadeManager;
    private StageManager _stageManager;

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        // ゴールフラグ初期化
        GameClearFlg = false;
        GameOverFlg = false;
        ResultFlg = false;
        StaySpotArea = false;

        _fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    public void GameClearFlgSet(bool val)
    {
        // ゲームクリアフラグを更新
        GameClearFlg = val;

        if (GameClearFlg)
        {
            // 次シーンへ
            _fadeManager.SceneName = "GameResult";

            // ResultManagerへ渡す
            ResultFlg = true;

            // FadeOut開始待機時間
            Invoke("SetFedeFlg", 2.5f);
        }
    }

    public void GameOverFlgSet(bool val)
    {
        // ゲームオーバーフラグを更新
        GameOverFlg = val;

        // ゲームオーバー
        if (GameOverFlg)
        {
            // 次シーンへ
            _fadeManager.SceneName = "GameResult";

            // FadeOut開始待機時間
            Invoke("SetFedeFlg", 2.5f);
        }
    }

    // UnityChanのモーション後にフェードする
    void SetFedeFlg()
    {
        _fadeManager.FadeOutFlg = true;
    }
}
