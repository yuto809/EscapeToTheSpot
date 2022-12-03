using UnityEngine;

public class Disable : MonoBehaviour
{
    private GameManager _gameManager;
    private FadeManager _fadeManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _fadeManager = FadeManager.Instance;
    }

    /// <summary>
    /// シーン切り替え時に、登録したイベントを削除する
    /// </summary>
    private void OnDisable()
    {
        Debug.Log("OnDisable Disable.cs");
        _gameManager.RemoveGameOverFlgEvent();
        _gameManager.RemoveGameClearFlgEvent();
        _fadeManager.RemoveFadeOutFlgEvent();
    }
}
