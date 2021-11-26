using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    private FadeManager fadeManager;

    [SerializeField]
    private float logoTime;

    void Start()
    {
        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        Invoke("Next", logoTime);
    }

    void Next()
    {
        fadeManager.FadeOutFlg = true;
        fadeManager.SceneName = "TitleScene";
//        SceneManager.LoadScene("TitleScene");
    }
}
