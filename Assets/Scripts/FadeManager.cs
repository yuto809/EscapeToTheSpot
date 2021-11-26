using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Panelに使用する
public class FadeManager : MonoBehaviour
{
    //シングルトン設定ここから
    static public FadeManager instance;

    [SerializeField]
    private float fadeSpeed; // 透明度が変わるスピードを管理

    [SerializeField]
    private float fadeTime = 3.0f;  // 指定した時間でフェードアウト/インする(未使用)

    [SerializeField]
    private GameObject fade;//インスペクタからPrefab化したCanvasを入れる

    // フェードアウト/イン開始フラグ
    public bool FadeOutFlg { set; get; }
    public bool FadeInFlg { set; get; }

    // 遷移先シーン名
    public string SceneName { set; get; }

    // フェード用のパネルをアタッチ
    private Image fadeImage = null; 
    
    // フェード用の画素情報
    private float red;
    private float green;
    private float blue;
    private float alpha;

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(fade);
        }
        else
        {
            Destroy(this.gameObject);
            Destroy(fade);
        }
    }

    void Start()
    {
        SceneName = "StageSelect";
        fadeImage = fade.GetComponentInChildren<Image>();

        // フェードアウト/フェードイン用のパネルを無効にする
        fadeImage.enabled = false;
        FadeOutFlg = false;
        
        // オブジェクト自身(0, 0, 0, 0)とする
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alpha = fadeImage.color.a;
    }

    private void Update()
    {
        // フェードアウトする場合
        if (FadeOutFlg)
        {
            // フェードアウト/フェードイン用のパネルを有効にする
            fadeImage.enabled = true;
            // 指定したスピードで1フレームずつ透明度を足していく
            alpha += fadeSpeed * Time.deltaTime;
            // 指定した時間で1フレームずつ透明度を足していく
            // alpha += Time.deltaTime / fadeTime;
            SetAlpha();

            // 設定値がMAXの場合
            if (alpha >= 1.0f)
            {
                //Debug.Log("fade out end");
                // フェードアウト終了して透明度をリセットする
                FadeOutFlg = false;

                // 次シーンへ
                // シーン読み込み完了時にFadeInStartをコールする(イベント登録)
                SceneManager.sceneLoaded += FadeInStart;
                SceneManager.LoadScene(SceneName);
            }
        }

        // フェードインする場合
        if (FadeInFlg)
        {
            // 1フレームずつ透明度を引いていく
            // 指定した時間で1フレームずつ透明度を引いていく
            alpha -= fadeSpeed * Time.deltaTime;
            //alpha -= Time.deltaTime / fadeTime;
            SetAlpha();

            // 設定値がMINの場合
            if (alpha <= 0.0f)
            {
                //Debug.Log("fade in end");
                // フェードイン終了
                FadeInFlg = false;
                // フェードアウト/フェードイン用のパネルを無効にする
                fadeImage.enabled = false;
            }
        }
    }

    // フェードインフラグをONにしてUpdateでフェードイン処理を行う
    void FadeInStart(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Fade IN Start");
        FadeInFlg = true;

        // シーン読み込み完了時の処理が完了したらイベント削除
        SceneManager.sceneLoaded -= FadeInStart;
    }

    // PanelのImage画素を設定する
    void SetAlpha()
    {
        fadeImage.color = new Color(red, green, blue, alpha);
    }

}
