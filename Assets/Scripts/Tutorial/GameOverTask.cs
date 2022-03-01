using UnityEngine;

public class GameOverTask : ITutorialTask
{
    public enum TriggerMessage
    {
        TRIGGER_MESSAGE_1 = 1,
        TRIGGER_MESSAGE_2 = 2,
        TRIGGER_MESSAGE_3 = 3,
    }
    
    private int textIndex;
    private string[] _textSentence;
    private GameManager _gameManager;
    private TutorialManager _tutorialManager;
    private TutorialObstacleGenerator _obsGenerator;
    private TutorialUnityChanController _unityChan;

    private bool _isCalled;

    private bool _showMessageComplete;
    private bool _tutorialGameOverComplete;
    private bool _tutorialAllComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;


    public void OnTaskSetting()
    {
        _gameManager = GameManager.Instance;
        _tutorialManager = TutorialManager.instance;
        _obsGenerator = _tutorialManager.GetComponent<TutorialObstacleGenerator>();
        _unityChan = GameObject.Find("TutorialUnityChan").GetComponent<TutorialUnityChanController>();
        
        textIndex = 0;

        _isCalled = false;

        _textSentence = new string[]
        {
            "次は、ゲームオーバーについてです。",
            "画面の上に、ハートが3つあります。このハートが全て無くなってしまうとゲームオーバーとなってしまいます。",
            "ステージ上に出現する敵、上空から落ちてくる障害物、前方から転がってくる障害物に当たってしまうとハートが減ってしまいます。",
            "…!! このようにハートが減ってしまいます…",
            "また、ステージ上から落ちてしまうと、同じようにハートが減ってしまいます。",
            "ハートが減ってしまうと、ステージ開始時の状態に戻ってしまいます。注意してスポットエリアまで目指そう…！",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialAllComplete = false;
        // イベント登録
        _tutorialManager.SetPanelEnabledChangeFlgEvent();
        _tutorialManager.SetLifeLostFlgEvent();
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_GAME_OVER;
    }

    public string GetText()
    {
        return _currentSenetnce;
    }

    // TutorialMangerのUpdate処理
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
                _tutorialManager.CallPanelEnabledChangeFlgEvent();
                _isCalled = true;
            }

            // タイミングは用件等
            // 特定メッセージを読み込んだら、頭上からブロックを落とす　//ハート消滅させる
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_2)
            {
                // unityChanの頭上からブロックを落とす
                _obsGenerator.CreateTutorialObstacle();
                _isCalled = true;
            }
        }
        // 現在表示されるべきメッセージ内容がすべて表示できている場合
        else
        {
            // ハートを消滅
            if (_currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_3)
            {
                if (!_isCalled && _gameManager.GameOverFlg)
                {
                    _tutorialManager.CallLifeLostFlgEvent();
                    _isCalled = true;
                }

                // 事前に決められた距離分移動したら移動チュートリアルは終了と判断する
                if (CheckTutorialGameOver())
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
                    _tutorialManager.RemoveLifeLostFlgEvent();
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
        return _tutorialGameOverComplete;
    }

    // 移動距離の合計値にする
    // 事前に決められた距離分移動したら移動チュートリアルは終了と判断する
    private bool CheckTutorialGameOver()
    {
        // unityChanに当たる→unityDead動作→gammanagerのゲームオーバーフラグが立つ
        if (_gameManager.GameOverFlg & !_tutorialManager.GetLifeImageEnable)
        {
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
        _isCalled = false;
    }

}
