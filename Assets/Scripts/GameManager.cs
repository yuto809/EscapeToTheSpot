using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //�V���O���g���ݒ肱������
    static public GameManager instance;

    // UnityChan��SpotArea�ɗ��܂��Ă���t���O
    public bool StaySpotArea { set; get; }

    // UnityChan���o�ߎ��ԓ�SpotArea�ɂ������̔��f�t���O
    public bool GameClearFlg { set; get; }

    public bool GameOverFlg { set; get; }

    public bool ResultFlg { set; get; }
    
    [SerializeField]
    private ObstacleGenerator objGen;

    private FadeManager fadeManager;
    private StageManager stageManager;

    // �V���O���g����Scene���ׂ��ł��I�u�W�F�N�g�͎c���悤�ɂ���
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        // �S�[���t���O������
        GameClearFlg = false;
        GameOverFlg = false;
        ResultFlg = false;
        StaySpotArea = false;

        fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    public void GameClearFlgSet(bool val)
    {
        // �Q�[���N���A�t���O���X�V
        GameClearFlg = val;

        if (GameClearFlg)
        {
            // ���V�[����
            fadeManager.SceneName = "GameResult";

            // ResultManager�֓n��
            ResultFlg = true;

            // FadeOut�J�n�ҋ@����
            Invoke("SetFedeFlg", 2.5f);
        }
    }

    public void GameOverFlgSet(bool val)
    {
        // �Q�[���I�[�o�[�t���O���X�V
        GameOverFlg = val;

        // �Q�[���I�[�o�[
        if (GameOverFlg)
        {
            // ���V�[����
            fadeManager.SceneName = "GameResult";

            // FadeOut�J�n�ҋ@����
            Invoke("SetFedeFlg", 2.5f);
        }
    }

    // UnityChan�̃��[�V������Ƀt�F�[�h����
    void SetFedeFlg()
    {
        fadeManager.FadeOutFlg = true;
    }
}
