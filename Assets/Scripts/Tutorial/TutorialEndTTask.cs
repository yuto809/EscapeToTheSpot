using UnityEngine;

public class TutorialEndTask : ITutorialTask
{
    public enum TriggerMessage
    {
        TRIGGER_MESSAGE_1 = 1,
    }

    private TutorialManager _tutorialManager;
    private TutorialUnityChanController _unityChan;
    private string[] _textSentence;

    private bool _showMessageComplete;
    private bool _tutorialComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;

    private bool _isCalled;

    public void OnTaskSetting()
    {
        _tutorialManager = TutorialManager.instance;
        _unityChan = GameObject.Find("TutorialUnityChan").GetComponent<TutorialUnityChanController>();

        _textSentence = new string[]
        {
            "これでゲームの操作方法の説明は以上になります。",
            "ぜひゲームクリアを目指してください!!",
            "遊んでみた感想もお待ちしております…!!",
            "それではEscape To The Runをお楽しみください。",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialComplete = false;
        _isCalled = false;
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TURORIAL_END;
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
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_1)
            {
                _unityChan.Reborn();
                _isCalled = true;
                Debug.Log("UnityChan Reborn");
            }
        }
        // 現在表示されるべきメッセージ内容がすべて表示できている場合
        else
        {
            if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap操作
            {
                // 現在のチュートリアルですべてのメッセージが表示出来たらチュートリアル終了
                if (_tutorialComplete)
                {
                    Debug.Log("チュートリアル完了");
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
}
