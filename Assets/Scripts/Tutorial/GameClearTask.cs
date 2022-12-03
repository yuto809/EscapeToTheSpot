using UnityEngine;

public class GameClearTask : ITutorialTask
{
    public enum TriggerMessage
    {
        TRIGGER_MESSAGE_7 = 7,
        TRIGGER_MESSAGE_8,
    }

    private TutorialManager _tutorialManager;
    private TutorialUnityChanController _unityChan;
    private TutorialSpotCreator _spotCreator;
    //private Light _spotLight;
    private TutorialSpotArea _spotArea;

    private string[] _textSentence;

    private bool _showMessageComplete;
    private bool _tutorialComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;

    private bool _isCalled;
    private bool _isCalled_2;

    private bool _tutorialGameClearComplete;
    private bool _tutorialAllComplete;


    public void OnTaskSetting()
    {
        // TutorialManager取得
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _unityChan = GameObject.Find("TutorialUnityChan").GetComponent<TutorialUnityChanController>();

        _textSentence = new string[]
        {
            "次は、このゲームのクリア条件です。",
            "ステージ上に、スポットライトが照らされた場所、スポットエリアが出現します。",
            "敵や、障害物を避けながら、スポットエリアを目指してください!!",
            "スポットエリアは時間が経過する度に、小さくなり消えてしまいます。",
            "スポットエリアが消えてしまう前に、その中に3秒間留まることができたら、ステージクリアとなります!!",
            "スポットエリアが消えてしまうと、ランダムで新たなスポットエリアが出現します。",
            "そのときは、カメラアングルを切り替えてスポットエリアを見つけて、素早く移動しましょう!!",
            "…!! スポットエリアが出現しました!!",
            "では実際に、スポットエリアを目指して、移動してみましょう。",
            "無事にスポットエリアまで辿り着くことができましたね!! これでゲームクリアとなり、次のステージに進むことができます!!",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialComplete = false;

        _isCalled = false;
        _isCalled_2 = false;

        _tutorialAllComplete = false;

        // イベント登録
        _tutorialManager.SetPanelEnabledChangeFlgEvent();
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_GAME_CLEAR;
    }

    public string GetText()
    {
        return _currentSenetnce;
    }


    public bool CheckTask()
    {
        // 現在表示されるべきメッセージ内容がすべて表示されていない場合
        if (!_showMessageComplete)
        {
            if (CheckSentence())
            {
                // 完了フラグを設定
                _showMessageComplete = true;
            }
            else
            {
                // 表示されるメッセージを1文字ずつ取得して設定する
                _currentSenetnce = _currentSenetnce + _textSentence[_currentSenetenceIndex][_currentCharIndex];

                // 次の1文字へ
                _currentCharIndex++;

            }

            // スポットエリア出現させる
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_7)
            {
                // _spotLightはこの時点で非活性状態なので、取得がうまくいっていない
                // TutorialManager側で変更する必要がある
                _tutorialManager.CallPanelEnabledChangeFlgEvent();

                _tutorialManager.SetSpotLightActive();
                // SpotCreatorインスタンス取得
                _spotArea = GameObject.Find("TutorialSpotArea").GetComponent<TutorialSpotArea>();

                //_spotLight.SetActive(true);
                _isCalled = true;
            }
        }
        // 現在表示されるべきメッセージ内容がすべて表示できている場合
        else
        {
            if (_currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_8)
            {
                // スポットエリアに留まることができたか判断する
                if (CheckTutorialGameClear())
                {
                    // メッセージを初期化して、次のメッセージ内容へ
                    SetNextSentenceInfo();
                }
            }
            else if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap操作
            {
                // 現在のチュートリアルですべてのメッセージが表示出来たらチュートリアル終了
                if (_tutorialAllComplete)
                {
                    Debug.Log("GameClearTutorial完了");
                    // イベント削除
                    _tutorialManager.RemovePanelEnabledChangeFlgEvent();
                    return true;
                }
                else
                {
                    // メッセージを初期化して、次のメッセージ内容へ
                    SetNextSentenceInfo();
                }
            }
        }
        return false;
    }

    public float GetTransitionTime()
    {
        return 0f;
    }

    public bool IsTutorialComplete()
    {
        return _tutorialComplete;
    }

    // スポットエリア内に留まることができたらチュートリアルは終了と判断する
    private bool CheckTutorialGameClear()
    {
        // 制限時間内にunityChanがエリアに留まれた場合
        if (_spotArea.GetJudgeClearFlg)
        {
            //_unityChan.UnitySuccess();
            _tutorialGameClearComplete = true;
            return true;
        }

        return false;
    }

    private bool CheckSentence()
    {
        // すべてのメッセージを表示している場合は処理をスキップ
        if (_currentSenetenceIndex >= _textSentence.Length)
        {
            Debug.Log("tutorial GameClear complete set");
            _tutorialAllComplete = true;

            // SetNextSentenceInfoによってメッセージが初期化されているため
            // 最後の文章を設定
            _currentSenetnce = _textSentence[_currentSenetenceIndex - 1];
            return true;
        }

        // 文字をすべて表示することができたら終了
        if (_currentSenetnce.Length == _textSentence[_currentSenetenceIndex].Length)
        {
            return true;
        }

        return false;
    }


    private void SetNextSentenceInfo()
    {
        _currentSenetenceIndex++;
        _currentCharIndex = 0;
        _currentSenetnce = "";
        _showMessageComplete = false;
    }
}
