using UnityEngine;

public class Logo : MonoBehaviour
{
    private FadeManager _fadeManager;

    [SerializeField]
    private float _logoTime;

    void Start()
    {
        _fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        Invoke("Next", _logoTime);
    }

    void Next()
    {
        _fadeManager.FadeOutFlg = true;
        _fadeManager.SceneName = "TitleScene";
//        SceneManager.LoadScene("TitleScene");
    }
}
