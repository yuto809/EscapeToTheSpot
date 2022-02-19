using UnityEngine;

public class Logo : MonoBehaviour
{
    private FadeManager _fadeManager;

    [SerializeField]
    private float _logoTime = 1.5f;

    private void Awake()
    {
        _fadeManager = FadeManager.Instance;
        _fadeManager.SetFadeOutFlgEvent();
    }

    private void Start()
    {
        Invoke("Next", _logoTime);
    }

    private void Next()
    {
        _fadeManager.CallFadeOutFlgEvent((int)FadeManager.NextScene.SCENE_TITLE);
        //_fadeManager.SceneName = "TitleScene";
    }


    private void OnDisable()
    {
        _fadeManager.RemoveFadeOutFlgEvent();
        //Debug.Log("OnDisable logo.cs");
    }
}
