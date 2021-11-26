using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Panel�Ɏg�p����
public class FadeManager : MonoBehaviour
{
    //�V���O���g���ݒ肱������
    static public FadeManager instance;

    [SerializeField]
    private float fadeSpeed; // �����x���ς��X�s�[�h���Ǘ�

    [SerializeField]
    private float fadeTime = 3.0f;  // �w�肵�����ԂŃt�F�[�h�A�E�g/�C������(���g�p)

    [SerializeField]
    private GameObject fade;//�C���X�y�N�^����Prefab������Canvas������

    // �t�F�[�h�A�E�g/�C���J�n�t���O
    public bool FadeOutFlg { set; get; }
    public bool FadeInFlg { set; get; }

    // �J�ڐ�V�[����
    public string SceneName { set; get; }

    // �t�F�[�h�p�̃p�l�����A�^�b�`
    private Image fadeImage = null; 
    
    // �t�F�[�h�p�̉�f���
    private float red;
    private float green;
    private float blue;
    private float alpha;

    // �V���O���g����Scene���ׂ��ł��I�u�W�F�N�g�͎c���悤�ɂ���
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

        // �t�F�[�h�A�E�g/�t�F�[�h�C���p�̃p�l���𖳌��ɂ���
        fadeImage.enabled = false;
        FadeOutFlg = false;
        
        // �I�u�W�F�N�g���g(0, 0, 0, 0)�Ƃ���
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alpha = fadeImage.color.a;
    }

    private void Update()
    {
        // �t�F�[�h�A�E�g����ꍇ
        if (FadeOutFlg)
        {
            // �t�F�[�h�A�E�g/�t�F�[�h�C���p�̃p�l����L���ɂ���
            fadeImage.enabled = true;
            // �w�肵���X�s�[�h��1�t���[���������x�𑫂��Ă���
            alpha += fadeSpeed * Time.deltaTime;
            // �w�肵�����Ԃ�1�t���[���������x�𑫂��Ă���
            // alpha += Time.deltaTime / fadeTime;
            SetAlpha();

            // �ݒ�l��MAX�̏ꍇ
            if (alpha >= 1.0f)
            {
                //Debug.Log("fade out end");
                // �t�F�[�h�A�E�g�I�����ē����x�����Z�b�g����
                FadeOutFlg = false;

                // ���V�[����
                // �V�[���ǂݍ��݊�������FadeInStart���R�[������(�C�x���g�o�^)
                SceneManager.sceneLoaded += FadeInStart;
                SceneManager.LoadScene(SceneName);
            }
        }

        // �t�F�[�h�C������ꍇ
        if (FadeInFlg)
        {
            // 1�t���[���������x�������Ă���
            // �w�肵�����Ԃ�1�t���[���������x�������Ă���
            alpha -= fadeSpeed * Time.deltaTime;
            //alpha -= Time.deltaTime / fadeTime;
            SetAlpha();

            // �ݒ�l��MIN�̏ꍇ
            if (alpha <= 0.0f)
            {
                //Debug.Log("fade in end");
                // �t�F�[�h�C���I��
                FadeInFlg = false;
                // �t�F�[�h�A�E�g/�t�F�[�h�C���p�̃p�l���𖳌��ɂ���
                fadeImage.enabled = false;
            }
        }
    }

    // �t�F�[�h�C���t���O��ON�ɂ���Update�Ńt�F�[�h�C���������s��
    void FadeInStart(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Fade IN Start");
        FadeInFlg = true;

        // �V�[���ǂݍ��݊������̏���������������C�x���g�폜
        SceneManager.sceneLoaded -= FadeInStart;
    }

    // Panel��Image��f��ݒ肷��
    void SetAlpha()
    {
        fadeImage.color = new Color(red, green, blue, alpha);
    }

}
