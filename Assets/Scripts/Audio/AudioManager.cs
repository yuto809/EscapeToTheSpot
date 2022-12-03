using UnityEngine;
using System.IO;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public enum PlaySE
    {
        CLICK_EASY = 0,
        CLICK_NORMAL,
        CLICK_HARD,
        CLICK_BACK,
        CLICK_START,
        DAMAGE_UNITYCHAN
    }

    public enum PlayBGM
    {
        BGM_TITLE_SCENE = 0,
        BGM_PLAY_SCENE,
    }

    public enum PlayVoice
    {
        VOICE_DEAD = 0,
        VOICE_CLEAR,
        VOICE_FALL
    }

    private AudioSource[] _BGM;
    private AudioSource _clickSE;
    private AudioClip _titleSceneBGM;
    private AudioClip _playModeBGM;
    private AudioClip _audioBundle;
 
    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        // インスタンスが見つからない場合は破棄
        if (this != AudioManager.Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        // [0]はSE用のオーディオクリップを格納する
        // [1]はTitle〜StageSelectまでのBGM
        // [2]はPlay中のBGM
        _BGM = GetComponents<AudioSource>();

        // BGMは容量が大きいため先にロードしておく
        _titleSceneBGM = Resources.Load("BGM/TitleStage/SpaceGameBgm#1_Galaxy Blues_Loop") as AudioClip;
        _playModeBGM = Resources.Load("BGM/PlayMode/GhostChaser_Loop") as AudioClip; ;

        // AssetBundleファイルをロード
        // StreamingAssetsからロード
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "audio"));

        Debug.Log(Path.Combine(Application.streamingAssetsPath, "audio"));
        //audioBundle = assetBundle.LoadAsset<AudioClip>("GhostChaser_Loop.wav");
    }


    public void PlayMusicSE(int clickSE)
    {
        // [0]をSEとして使用する
        _clickSE = GetComponent<AudioSource>();
        _clickSE.clip = GetUseAudioClipSE(clickSE);

        if (_clickSE.clip != null)
        {
            _clickSE.Play();
        }
        // 流すSEに問題がある場合は停止する
        else
        {
            _clickSE.Stop();
        }
    }

    private AudioClip GetUseAudioClipSE(int clickSE)
    {
        AudioClip retClip = null;

        switch(clickSE)
        {
            case (int)PlaySE.CLICK_EASY:
                retClip = Resources.Load("SE/StageSelect/EasyClick") as AudioClip;
                break;
            case (int)PlaySE.CLICK_NORMAL:
                retClip = Resources.Load("SE/StageSelect/NormalClick") as AudioClip;
                break;
            case (int)PlaySE.CLICK_HARD:
                retClip = Resources.Load("SE/StageSelect/HardClick") as AudioClip;
                break;
            case (int)PlaySE.CLICK_BACK:
                retClip = Resources.Load("SE/StageSelect/BackClick") as AudioClip;
                break;
            case (int)PlaySE.DAMAGE_UNITYCHAN:
                retClip = Resources.Load("SE/Damage/Damage") as AudioClip;
                break;
            case (int)PlaySE.CLICK_START:
                retClip = Resources.Load("SE/Title/PlayClick") as AudioClip;
                break;
            default:
                retClip = null;
                break;
        }


        return retClip;
    }

    public void PlayMusicBGM(int playBGM)
    {
        // 流すBGMが決まるまでは停止する
        _BGM[1].Stop();
        _BGM[1].clip = GetUseAudioClipBGM(playBGM);

        if (_BGM[1].clip != null)
        {
            Debug.Log("PlayMUSIC");
            Invoke("StartBGM", 1.0f);
        }
        // 流すSEに問題がある場合は停止する
        else
        {
            _BGM[1].Stop();
        }

    }

    private AudioClip GetUseAudioClipBGM(int playBGM)
    {
        AudioClip retClip = null;

        switch (playBGM)
        {
            case (int)PlayBGM.BGM_TITLE_SCENE:
                retClip = _titleSceneBGM;
                break;
            case (int)PlayBGM.BGM_PLAY_SCENE:
                retClip = _playModeBGM;
                break;
            default:
                retClip = null;
                break;
        }

        return retClip;
    }

    private void StartBGM()
    {
        _BGM[1].Play();
    }

    public void PlayMusicVoice(int playVoice)
    {
        // [2]をUnityChanのボイスとして使用する

        // 流すBGMが決まるまでは停止する
        _BGM[2].Stop();
        _BGM[2].clip = GetUseAudioClipVoice(playVoice);

        if (_BGM[2].clip != null)
        {
            _BGM[2].Play();
        }
        // 流すSEに問題がある場合は停止する
        else
        {
            _BGM[2].Stop();
        }

    }

    private AudioClip GetUseAudioClipVoice(int playVoice)
    {
        AudioClip retClip = null;

        switch (playVoice)
        {
            case (int)PlayVoice.VOICE_DEAD:
                retClip = Resources.Load("SE/Damage/damage_uni1520") as AudioClip;
                break;
            case (int)PlayVoice.VOICE_CLEAR:
                retClip = Resources.Load("SE/Clear/Clear_uni1518") as AudioClip;
                break;
            case (int)PlayVoice.VOICE_FALL:
                retClip = Resources.Load("SE/Damage/fall_uni1519") as AudioClip;
                break;
            default:
                retClip = null;
                break;
        }

        return retClip;
    }
}
