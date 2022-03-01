using System.Collections.Generic;
using UnityEngine;

public class MovementTask : ITutorialTask
{
    public enum TriggerMessage
    {
        TRIGGER_MESSAGE_1 = 1,
        TRIGGER_MESSAGE_2,
    }

    // Unitychanの初期位置(X:0, Y:0.5, Z:-5)

    // 特定距離移動したかどうか判断定数(微調整必要)
    const float MOVE_POS_UP = -3.5f;
    const float MOVE_POS_RIGHT = 1.5f;
    const float MOVE_POS_DOWN = -7.0f;
    const float MOVE_POS_LEFT = -1.0f;

    private int textIndex;
    private List<string> textMessages = new List<string>();
    private string[] _textSentence;
    private TutorialManager _tutorialManager;
    private TutorialUnityChanController _unityChan;

    private bool _moveUpFlg;
    private bool _moveRightFlg;
    private bool _moveDownFlg;
    private bool _moveLeftFlg;

    private bool _isCalled;

    private bool _showMessageComplete;
    private bool _tutorialMovementComplete;
    private bool _tutorialAllComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;


    public void OnTaskSetting()
    {
        // TutorialManager取得
        //_tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _tutorialManager = TutorialManager.instance;
        _unityChan = GameObject.Find("TutorialUnityChan").GetComponent<TutorialUnityChanController>();
        
        textIndex = 0;
        _moveUpFlg = false;
        _moveRightFlg = false;
        _moveDownFlg = false;
        _moveLeftFlg = false;

        _isCalled = false;

        // 0origin
        textMessages.Add("まずは、キャラクターの移動方法です。");
        textMessages.Add("WSADキー、またはカーソルキーで上下左右に移動できます。");
        textMessages.Add("では、実際にキャラクターを移動させてみましょう。");
        textMessages.Add("問題ないですね。それでは、次の操作説明です。");

        _textSentence = new string[]
        {
            "まずは、キャラクターの移動方法です。",
            "WSADキー、またはカーソルキーで上下左右に移動できます。",
            "では、実際にキャラクターを移動させてみましょう。",
            "問題ないですね。それでは、次の操作説明です。",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialAllComplete = false;
        // イベント登録
        _tutorialManager.SetPanelEnabledChangeFlgEvent();
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_MOVEMENT;
    }

    public string GetText()
    {
        if (textIndex >= textMessages.Count)
        {
            return textMessages[textMessages.Count - 1];
        }
        else
        {
            return _currentSenetnce;
        }
    }

    // TutorialMangerのUpdate処理
    public bool CheckTask()
    {
        // 現在表示されるべきメッセージ内容がすべて表示されていない場合
        if (!_showMessageComplete)
        {

            if (CheckSentence())
            {
                //Debug.Log(_textSentence[_currentSenetenceIndex][_currentCharIndex]);
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

            // 特定メッセージを読み込んだら、フォーカス解除するイベントをTutorial側に伝える
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_1)
            {
                _tutorialManager.CallPanelEnabledChangeFlgEvent();
                _isCalled = true;
            }
        }
        // 現在表示されるべきメッセージ内容がすべて表示できている場合
        else
        {
            if (_currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_2)
            {
                // 事前に決められた距離分移動したら移動チュートリアルは終了と判断する
                if (CheckTutorialMove())
                {
                    // メッセージを初期化して、次のメッセージ内容へ
                    SetNextSentenceInfo();
                }
            }
            else if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap操作
            {
                // 現在のチュートリアルですべてのメッセージが表示出来たらチュートリアル終了
                if (_tutorialAllComplete)//_currentSenetenceIndex == _currentSenetnce.Length)
                {
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
        return _tutorialMovementComplete;
    }

    // 移動距離の合計値にする

    // 事前に決められた距離分移動したら移動チュートリアルは終了と判断する
    private bool CheckTutorialMove()
    {
        //Debug.Log("移動Check");
        if (_unityChan.transform.position.x > MOVE_POS_RIGHT)
        {
            _moveRightFlg = true;
        }

        if (_unityChan.transform.position.x < MOVE_POS_LEFT)
        {
            _moveLeftFlg = true;
        }

        if (_unityChan.transform.position.z > MOVE_POS_UP)
        {
            _moveUpFlg = true;
        }

        if (_unityChan.transform.position.z < MOVE_POS_DOWN)
        {
            _moveDownFlg = true;
        }

        // 移動操作が完了したかどうか？
        if ((_moveUpFlg & _moveRightFlg & _moveDownFlg & _moveLeftFlg))
        {
            Debug.Log("移動チュートリアル完了");

            //textIndex++;

            _tutorialMovementComplete = true;
            // 次のチュートリアル実行までパネルを有効に戻すか？
            return true;
        }

        return false;
    }

    private bool CheckSentence()
    {
        // すべてのメッセージを表示している場合は処理をスキップ
        if (_currentSenetenceIndex >= _textSentence.Length)
        {
            Debug.Log("move tutorial complete set");
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
