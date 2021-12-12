using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class AudioManager : MonoBehaviour
{
    //シングルトン設定ここから
    static public AudioManager Instance;

    private AudioSource[] _BGM;
    private AudioSource _clickSE;
    private AudioClip _audioBundle;
 
    public string CurrentSceneName { set; get; }

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        // [0]はSE用のオーディオクリップを格納する
        // [1]はTitle〜StageSelectまでのBGM
        // [2]はPlay中のBGM
        _BGM = GetComponents<AudioSource>();
        CurrentSceneName = SceneManager.GetActiveScene().name;

        // AssetBundleファイルをロード
        // StreamingAssetsからロード
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "audio"));

        Debug.Log(Path.Combine(Application.streamingAssetsPath, "audio"));
        //audioBundle = assetBundle.LoadAsset<AudioClip>("GhostChaser_Loop.wav");
    }

    private void ClickPlaySE(AudioClip audioClip)
    {
        _clickSE = GetComponent<AudioSource>();
        _clickSE.clip = audioClip;
        _clickSE.Play();
    }
    // 全部1つにまとめられる
    public void playClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void easyClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void normalClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void hardClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void backClickSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    public void DamageSE(AudioClip audioClip)
    {
        ClickPlaySE(audioClip);
    }

    // シーンが切り替わった時に呼ばれるメソッド　
    public void OnActiveSceneChanged(string nextSceneName)//(Scene nextScene)
    {
        // ステージセレクト画面からステージ画面に遷移するとき、BGMを切り替える
        if (CurrentSceneName == "StageSelect" && nextSceneName == "RunToTheSpot")
        {
            _BGM[1].Stop();
            Invoke("PlayModeBGM", 1.0f);
        }
    }

    void PlayModeBGM()
    {
        //BGM[2].clip = audioBundle;
        _BGM[2].Play();

//        BGM[2].Play();
    }

    public void TitleBGM()
    {
        _BGM[2].Stop();
        Invoke("PlayTitleBGM", 1.0f);
    }

    private void PlayTitleBGM()
    {
        _BGM[1].Play();
    }
}
