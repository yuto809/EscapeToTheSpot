using System.Collections.Generic;
using UnityEngine;

public class CameraAngleTask : ITutorialTask
{
    public enum TriggerCameraMessage
    {
        TRIGGER_MESSAGE_3 = 3,
        TRIGGER_MESSAGE_4,
    }

    private int textIndex;
    private List<string> textMessages = new List<string>();
    private string[] _textSentence;
    private TutorialManager _tutorialManager;
    private Camera _mainCamera;

    private bool _angleChangeFlg;
    private bool _angleDefaultFlg;

    private bool _showMessageComplete;
    private bool _tutorialComplete;

    private bool _isCalled;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;

    public void OnTaskSetting()
    {
        // TutorialManager取得
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        _mainCamera.enabled = false;

        textIndex = 0;

        _textSentence = new string[]
        {
            "次は、カメラアングルの切り替えです。",
            "ゲームを進めていくと、広いステージが出現することがあります。",
            "そのため、今のカメラアングルでスポットを映し出せない場合があります。",
            "そのときは、カメラボタンを押下することで、カメラアングルの切り替えが行えます。",
            "では、カメラアングルを切り替えてみましょう。",
            "問題ないですね。それでは、次の操作説明です。",
        };

        _angleChangeFlg = false;
        _angleDefaultFlg = false;

        _isCalled = false;

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialComplete = false;

        // イベント登録
        _tutorialManager.SetPanelEnabledChangeFlgEvent();
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_CAMERA;
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

            // 特定メッセージを読み込んだら、フォーカス解除するイベントをTutorial側に伝える
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerCameraMessage.TRIGGER_MESSAGE_3)
            {
                _tutorialManager.CallPanelEnabledChangeFlgEvent();
                _isCalled = true;
            }
        }
        // 現在表示されるべきメッセージ内容がすべて表示できている場合
        else
        {
            if (_currentSenetenceIndex == (int)TriggerCameraMessage.TRIGGER_MESSAGE_4)
            {
                // 事前に決められた距離分移動したら移動チュートリアルは終了と判断する
                if (CheckTutorialAngle())
                {
                    // メッセージを初期化して、次のメッセージ内容へ
                    SetNextSentenceInfo();
                }
            }
            else if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap操作
            {
                // 現在のチュートリアルですべてのメッセージが表示出来たらチュートリアル終了
                if (_tutorialComplete)
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
        return _tutorialComplete;
    }

    private bool CheckSentence()
    {
        // すべてのメッセージを表示している場合は処理をスキップ
        if (_currentSenetenceIndex >= _textSentence.Length)
        {
            _tutorialComplete = true;
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

    // カメラアングルの切り替え(ON/OFF)が完了したらチュートリアルは終了と判断する
    private bool CheckTutorialAngle()
    {
        //Debug.Log("カメラCheck");
        if (_mainCamera.enabled)
        {
            _angleChangeFlg = true;
        }

        // 1度カメラアングルを切り替えている且つ
        // defaultに戻した場合
        if (_angleChangeFlg && !_mainCamera.enabled)
        {
            _angleDefaultFlg = true;
        }

        // 移動操作が完了したかどうか？
        if ((_angleChangeFlg & _angleDefaultFlg))
        {
            // 次のチュートリアル実行までパネルを有効に戻すか？
            return true;
        }

        return false;
    }
}
