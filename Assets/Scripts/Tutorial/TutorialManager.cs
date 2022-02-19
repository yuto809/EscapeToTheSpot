using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// ゲーム上のチュートリアルを管理するマネージャクラス
/// </summary>
public class TutorialManager : MonoBehaviour
{
    const int FADE_COUNT = 3;

    public enum TutorialTitle
    {
        TUTORIAL_INTRODUCE = 0,
        TUTORIAL_MOVEMENT,
        TUTORIAL_CAMERA,
        TUTORIAL_GAME_CLEAR,
    }

    [SerializeField]
    private Text _tutorialTitle;

    [SerializeField]
    private Text _tutorialText;

    [SerializeField]
    private GameObject _focusCursolPanel;

    [SerializeField]
    private GameObject _focusAnglePanel;

    [SerializeField]
    private float _fadeSpeed; // 透明度が変わるスピードを管理

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private GameObject _spotLight;
    
    /// <summary>
    /// パネルのフェード処理中フラグ
    /// </summary>
    public bool ActiveFadePanelFlg
    {
        get
        {
            return _activeFadePanelFlg;
        }
    }

    /// <summary>
    /// パネルのフェード処理開始フラグの状態
    /// </summary>
    public bool GetPanelEnabledChangeFlg
    {
        get
        {
            return _panelEnabledChangeFlg;
        }
    }

    /// <summary>
    /// スポットライトが有無を取得
    /// </summary>
    public bool GetSpotLightEnable
    {
        get
        {
            return _spotLight.activeSelf;
        }
    }

    /// <summary>
    /// 現在のチュートリアル状態を返す
    /// </summary>
    public int GetCurrentTutorialStatus
    {
        get
        {
            return _currentTutorialStatus;
        }
    }

    public static TutorialManager instance;

    private UnityEvent _focusPanelFlgEvent = new UnityEvent();

    // チュートリアルタスク
    private ITutorialTask _currentTask;
    private List<ITutorialTask> _tutorialTask;
    private bool[] _tutorialTaskCompleteStatus;
    private int _tutorialTaskIndex;

    // チュートリアル表示フラグ
    private bool _isEnabled;

    // チュートリアルタスクの条件を満たした際の遷移用フラグ
    private bool _taskExecuted = false;

    // チュートリアル表示時のUI移動距離
    private float fade_pos_x = 350;

    // フェード用の画素情報
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;


    private bool _activeFadePanelFlg;
    //private bool _anglePanelEnabled;

    // テキスト取得フラグ
    private bool _updateTextFlg;

    // カーソルとアングルのパネルの表示/非表示を行う
    private bool _panelEnabledChangeFlg;
    private int _repeatFadeCnt;

    // 今何のチュートリアルを実行しているのかを表す変数
    private int _currentTutorialStatus;

    // instance作成
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _tutorialTask = new List<ITutorialTask>();

        // 導入チュートリアル定義
        _tutorialTask.Add(new Introducetask());
        // 操作移動チュートリアル定義
        _tutorialTask.Add(new MovementTask());
        // カメラアングル切り替えチュートリアル定義
        _tutorialTask.Add(new CameraAngleTask());
        // クリア条件確認チュートリアル定義
        _tutorialTask.Add(new GameClearTask());
        // ゲームオーバー条件確認チュートリアル定義


        // 各チュートリアルの完了状態を保持する配列を定義
        _tutorialTaskCompleteStatus = new bool[_tutorialTask.Count]; //new bool[_tutorialTask.Count - 1];

        // フラグ初期化
        for (var i = 0; i < _tutorialTaskCompleteStatus.Length; i++)
        {
            _tutorialTaskCompleteStatus[i] = false;
        }


        // 最初のチュートリアルを設定
        StartCoroutine(SetCurrentTask(_tutorialTask.First()));


        /**************************パネル関連********************************/
        // 色情報取得(パネルはどれも同一色)
        _red = _focusCursolPanel.GetComponent<Image>().color.r;
        _green = _focusCursolPanel.GetComponent<Image>().color.g;
        _blue = _focusCursolPanel.GetComponent<Image>().color.b;
        _alpha = _focusCursolPanel.GetComponent<Image>().color.a;

        // フォーカスパネルを透明にする
        _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        _repeatFadeCnt = 0;
        _activeFadePanelFlg = false;
        _panelEnabledChangeFlg = false;
        
        /**************************スポットライト関連************************/
        //_spotLight = GameObject.Find("SpotLight");
        // GameClearTutorial後に操作できるようにスポットライトは非表示にする
        _spotLight.SetActive(false);

        /**************************スポットライト関連************************/
        // CameraTutorial後に操作できるようにMainCameraは非表示にする
        _mainCamera.enabled = false;

        //// 0定義だと
        //_tutorialTaskIndex = -1;
        _currentTutorialStatus = -1;
        _isEnabled = true;


    }

    /// <summary>
    /// 新しいチュートリアルタスクを設定する
    /// </summary>
    /// <param name="task"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator SetCurrentTask(ITutorialTask task, float time = 0)
    {
        // timeが指定されている場合は待機
        yield return new WaitForSeconds(time);

        // 現在のチュートリアルを設定
        _currentTask = task;
        // 現在のチュートリアル完了フラグ
        _taskExecuted = false;

        // チュートリアルタスク設定時用の関数を実行
        task.OnTaskSetting();

        // UIにタイトルと説明文を設定
        _tutorialTitle.text = GetTitle(task.GetTitleIndex());
        _tutorialText.text = task.GetText();
        _updateTextFlg = true;
        _currentTutorialStatus++;
        //_tutorialTaskIndex++;

    }
    
    void Update()
    {
        // 実行中のチュートリアルの説明文を取得
        // TransitionTimeが1以上だとエラー
        if (_updateTextFlg)
        {
            _tutorialText.text = _currentTask.GetText();
        }

        // panelの表示/非表示処理
        if (_panelEnabledChangeFlg)
        {
            FadeFocusPanel();
        }

        // 現在のチュートリアル作業が完了した時点で
        // 一度操作は受け付けないようにパネルを表示させる(透明度1)
        if (_currentTask != null && _currentTask.IsTutorialComplete())
        {
            if (!_tutorialTaskCompleteStatus[_currentTask.GetTitleIndex()]) //[_tutorialTaskIndex])
            {
                //Debug.Log(_tutorialTitle.text);
                //Debug.Log("チュートリアルインデックス：");
                //Debug.Log(_tutorialTaskIndex);
                _tutorialTaskCompleteStatus[_currentTask.GetTitleIndex()] = true; //[_tutorialTaskIndex] = true;
            }

            ResetPanel();
        }

        // チュートリアルが存在し実行されていない場合に処理
        if (_currentTask != null && !_taskExecuted)
        {
            // 現在のチュートリアルが実行されたか判定
            if (_currentTask.CheckTask())
            {
                _taskExecuted = true;

                // 現在設定されているチュートリアルを削除
                _tutorialTask.RemoveAt(0);

                Debug.Log("delete current tutorial");

                var nextTask = _tutorialTask.FirstOrDefault();

                // チュートリアルがまだ残っているなら次のチュートリアルを実行
                if (nextTask != null)
                {
                    _updateTextFlg = false;
                    StartCoroutine(SetCurrentTask(nextTask, 1f));
                }
            }
        }
    }

    private string GetTitle(int titleIndex)
    {
        string retTitle = "";

        switch (titleIndex)
        {
            case (int)TutorialTitle.TUTORIAL_INTRODUCE:
                retTitle = "ゲーム導入 概要";
                break;
            case (int)TutorialTitle.TUTORIAL_MOVEMENT:
                retTitle = "操作確認 移動";
                break;
            case (int)TutorialTitle.TUTORIAL_CAMERA:
                retTitle = "操作確認 カメラ";
                break;
            case (int)TutorialTitle.TUTORIAL_GAME_CLEAR:
                retTitle = "操作確認 ゲームクリア";
                break;
        }

        return retTitle;
    }



    // イベント関連処理(登録)
    public void SetPanelEnabledChangeFlgEvent()
    {
        _focusPanelFlgEvent.AddListener(SetFocusPanelFlg);
    }

    // イベント関連処理(実行)
    public void CallPanelEnabledChangeFlgEvent()
    {
        _focusPanelFlgEvent.Invoke();
    }

    // イベント関連処理(削除)
    public void RemovePanelEnabledChangeFlgEvent()
    {
        _focusPanelFlgEvent.RemoveListener(SetFocusPanelFlg);
    }

    private void SetFocusPanelFlg()
    {
        if (_panelEnabledChangeFlg)
        {
            _panelEnabledChangeFlg = false;
        }
        else
        {
            _panelEnabledChangeFlg = true;
        }
    }


    // フェード開始
    private void FadeFocusPanel()
    {
        // フェード処理中
        _activeFadePanelFlg = true;

        // 指定したスピードで1フレームずつ透明度を足していく
        _alpha += _fadeSpeed * Time.deltaTime;

        SetAlpha();

        // 設定値がMAXの場合(最大値は1.0)
        if (_alpha >= 0.5f)
        {
            _alpha = 0;
            _repeatFadeCnt++;

            ChangeFocusPanelEnabled();
        }

    }

    private void ChangeFocusPanelEnabled()
    {
        // 一定回数フェード処理していないなら終了
        if (_repeatFadeCnt < FADE_COUNT)
        {
            return;
        }

        // フェードアウト終了して透明度をリセットする
        _panelEnabledChangeFlg = false;

        if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_MOVEMENT)
        {
            _focusCursolPanel.SetActive(false);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_CAMERA)
        {
            _focusAnglePanel.SetActive(false);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_GAME_CLEAR)
        {
            _focusCursolPanel.SetActive(false);
            _focusAnglePanel.SetActive(false);
            _spotLight.SetActive(true);
        }

        // フェード処理を一定回数したため、次チュートリアル処理のために初期化
        _repeatFadeCnt = 0;
        _activeFadePanelFlg = false;
        Debug.Log("activePanel false");
        Debug.Log(_activeFadePanelFlg);
    }

    private void ResetPanel()
    {
        Debug.Log("ResetPanel");
        // 移動チュートリアル中
        if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_MOVEMENT)
        {
            _focusCursolPanel.SetActive(true);
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        }
        // カメラチュートリアル中
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_CAMERA)
        {
            _focusAnglePanel.SetActive(true);
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        }
        // ゲームクリア確認チュートリアル中
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_GAME_CLEAR)
        {
            _focusCursolPanel.SetActive(true);
            _focusAnglePanel.SetActive(true);
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        }
    }
        // PanelのImage画素を設定する
    private void SetAlpha()
    {
        if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_MOVEMENT)
        {
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_CAMERA)
        {
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_GAME_CLEAR)
        {
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
        }
    }

    public bool GetTutorialTaskCompStatus(int index)
    {
        //Debug.Log("配列数"+ _tutorialTaskCompleteStatus.Length);
        return _tutorialTaskCompleteStatus[index];
    }
}
