using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// Panelに使用する
public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    /// <summary>
    /// 次に遷移するシーン名
    /// </summary>
    public enum NextScene
    {
        SCENE_TITLE = 0,
        SCENE_STAGE_SELECT,
        SCENE_ESCAPE_TO_THE_SPOT,
        SCENE_GAME_RESULT
    }

    [SerializeField]
    private float _fadeSpeed;        // 透明度が変わるスピードを管理

    [SerializeField]
    private float _fadeTime = 3.0f;  // 指定した時間でフェードアウト/インする(未使用)

    [SerializeField]
    private GameObject _fade;        //インスペクタからPrefab化したCanvasを入れる


    /// <summary>
    /// フェードアウト開始フラグ
    /// </summary>
    public bool FadeOutFlg
    {
        get
        {
            return _fadeOutFlg;
        }
    }

    /// <summary>
    /// フェードイン開始フラグ
    /// </summary>
    public bool FadeInFlg
    {
        get
        {
            return _fadeInFlg;
        }
    }

    // 遷移先シーン名
    //public string SceneName { set; get; }

    public string SceneName
    {
        get
        {
            return _nextSceneName;
        }
    }

    // フェード用のパネルをアタッチ
    private Image _fadeImage = null; 
    
    // フェード用の画素情報
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;

    private bool _fadeOutFlg;
    private bool _fadeInFlg;
    private string _nextSceneName;

    private UnityEvent _fadeOutFlgEvent = new UnityEvent();

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        // インスタンスが見つからない場合は破棄
        if (this != FadeManager.Instance)
        {
            Destroy(this.gameObject);
            Destroy(_fade);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(_fade);
        }
    }

    private void Start()
    {
        //SceneName = "StageSelect";
        _nextSceneName = "StageSelect";

        _fadeImage = _fade.GetComponentInChildren<Image>();

        // フェードアウト/フェードイン用のパネルを無効にする
        _fadeImage.enabled = false;

        _fadeOutFlg = false;
        _fadeInFlg = false;

        // オブジェクト自身(0, 0, 0, 0)とする
        _red = _fadeImage.color.r;
        _green = _fadeImage.color.g;
        _blue = _fadeImage.color.b;
        _alpha = _fadeImage.color.a;
    }

    private void Update()
    {
        // フェードアウト
        if (FadeOutFlg)
        {
            FadeOutStart();
        }

        // フェードイン
        if (FadeInFlg)
        {
            FadeInStart();
        }
    }

    // フェードアウト開始
    private void FadeOutStart()
    {
        // フェードアウト/フェードイン用のパネルを有効にする
        _fadeImage.enabled = true;
        // 指定したスピードで1フレームずつ透明度を足していく
        _alpha += _fadeSpeed * Time.deltaTime;
        // 指定した時間で1フレームずつ透明度を足していく
        // alpha += Time.deltaTime / fadeTime;
        SetAlpha();

        // 設定値がMAXの場合
        if (_alpha >= 1.0f)
        {
            // フェードアウト終了して透明度をリセットする
            _fadeOutFlg = false;

            // 次シーンへ
            // シーン読み込み完了時にFadeInStartをコールする(イベント登録)
            SceneManager.sceneLoaded += SetFadeInStartFlg;
            SceneManager.LoadScene(SceneName);
        }
    }

    // フェードイン開始
    private void FadeInStart()
    {
        // 指定したスピードで1フレームずつ透明度を引いていく
        _alpha -= _fadeSpeed * Time.deltaTime;
        // 1フレームずつ透明度を引いていく
        //alpha -= Time.deltaTime / fadeTime;
        SetAlpha();

        // 設定値がMINの場合
        if (_alpha <= 0.0f)
        {
            // フェードイン終了
            _fadeInFlg = false;

            // フェードアウト/フェードイン用のパネルを無効にする
            _fadeImage.enabled = false;
        }
    }

    // フェードインフラグをONにしてUpdateでフェードイン処理を行う
    private void SetFadeInStartFlg(Scene scene, LoadSceneMode mode)
    {
        _fadeInFlg = true;

        // シーン読み込み完了時の処理が完了したらイベント削除
        SceneManager.sceneLoaded -= SetFadeInStartFlg;
    }

    // PanelのImage画素を設定する
    private void SetAlpha()
    {
        _fadeImage.color = new Color(_red, _green, _blue, _alpha);
    }

    // FadeOutFlgイベント関連処理
    public void SetFadeOutFlgEvent()
    {
        _fadeOutFlgEvent.AddListener(SetFadeOutFlg);
    }
    // FadeOutFlgイベント関連処理
    public void CallFadeOutFlgEvent(int nextSceneName)
    {
        switch(nextSceneName)
        {
            case (int)FadeManager.NextScene.SCENE_TITLE:
                _nextSceneName = "TitleScene";
                break;
            case (int)FadeManager.NextScene.SCENE_STAGE_SELECT:
                _nextSceneName = "StageSelect";
                break;
            case (int)FadeManager.NextScene.SCENE_ESCAPE_TO_THE_SPOT:
                _nextSceneName = "RunToTheSpot";
                break;
            case (int)FadeManager.NextScene.SCENE_GAME_RESULT:
                _nextSceneName = "GameResult";
                break;
            default:
                break;
        }

        _fadeOutFlgEvent.Invoke();
    }
    
    // FadeOutFlgイベント関連処理
    //public void CallFadeOutFlgEvent()
    //{
    //    _fadeOutFlgEvent.Invoke();
    //}

    // FadeOutFlgイベント関連処理
    public void RemoveFadeOutFlgEvent()
    {
        _fadeOutFlgEvent.RemoveListener(SetFadeOutFlg);
    }

    // フェードアウト開始
    private void SetFadeOutFlg()
    {
        _fadeOutFlg = true;
    }
}
