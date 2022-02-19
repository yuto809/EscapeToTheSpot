using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        _gameManager.RemoveGameOverFlgEvent();
        _gameManager.RemoveGameClearFlgEvent();
        _fadeManager.RemoveFadeOutFlgEvent();

        //Debug.Log("OnDisable Disable.cs");
    }
}
