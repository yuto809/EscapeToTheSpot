using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //シングルトン設定ここから
    static public GameManager instance;

    // UnityChanがSpotAreaに留まっているフラグ
    public bool StaySpotArea { set; get; }

    // UnityChanが経過時間内SpotAreaにいたかの判断フラグ
    public bool GameClearFlg { set; get; }

    public bool GameOverFlg { set; get; }

    public bool ResultFlg { set; get; }
    
    [SerializeField]
    private ObstacleGenerator objGen;

    private FadeManager fadeManager;
    private StageManager stageManager;

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
        // ゴールフラグ初期化
        GameClearFlg = false;
        GameOverFlg = false;
        ResultFlg = false;
        StaySpotArea = false;

        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    public void GameClearFlgSet(bool val)
    {
        // ゲームクリアフラグを更新
        GameClearFlg = val;

        if (GameClearFlg)
        {
            // 次シーンへ
            fadeManager.SceneName = "GameResult";

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
            fadeManager.SceneName = "GameResult";

            // FadeOut開始待機時間
            Invoke("SetFedeFlg", 2.5f);
        }
    }

    // UnityChanのモーション後にフェードする
    void SetFedeFlg()
    {
        fadeManager.FadeOutFlg = true;
    }
}
