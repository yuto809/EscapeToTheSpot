public interface ITutorialTask
{
    /// <summary>
    /// チュートリアルのタイトル(index)を取得する
    /// </summary>
    /// <returns></returns>
    int GetTitleIndex();

    /// <summary>
    /// 説明文を取得する
    /// </summary>
    /// <returns></returns>
    string GetText();

    /// <summary>
    /// チュートリアルタスクが設定された際に実行される
    /// </summary>
    void OnTaskSetting();

    /// <summary>
    /// チュートリアルが達成されたか判定する
    /// </summary>
    /// <returns></returns>
    bool CheckTask();

    /// <summary>
    /// 達成後に次のタスクへ遷移するまでの時間(秒)
    /// </summary>
    /// <returns></returns>
    float GetTransitionTime();

    bool IsTutorialComplete();
}

