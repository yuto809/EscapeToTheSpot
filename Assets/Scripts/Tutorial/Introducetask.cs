using UnityEngine;

public class Introducetask : ITutorialTask
{
    private int textIndex;
    private string[] _textSentence;

    private bool _showMessageComplete;
    private bool _tutorialComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;

    public void OnTaskSetting()
    {
        textIndex = 0;

        _textSentence = new string[]
        {
            "初めまして!!",
            "Escape To The Runへようこそ!!",
            "このゲームは、『キャラクターを操作して、ステージ上に出現するスポットエリアに向かって逃げる』を目的とした、アクションゲームとなっています。",
            "道中では、敵が出現することもあるので、上手く避けながらスポットへ逃げてください…!!",
            "それでは、ゲームの操作方法を説明していきます。",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialComplete = false;

    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_INTRODUCE;
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
        }
        // 現在表示されるべきメッセージ内容がすべて表示できている場合
        else
        {
            if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap操作
            {
                // 現在のチュートリアルですべてのメッセージが表示出来たらチュートリアル終了
                if (_tutorialComplete)
                {
                    Debug.Log("Introduce完了");
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
            Debug.Log("tutorial complete set");
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
