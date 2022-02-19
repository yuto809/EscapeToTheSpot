using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// �Q�[����̃`���[�g���A�����Ǘ�����}�l�[�W���N���X
/// </summary>
public class TutorialManager : MonoBehaviour
{
    const int FADE_COUNT = 3;

    public enum TutorialTitle
    {
        TUTORIAL_INTRODUCE = 0,
        TUTORIAL_MOVEMENT,
        TUTORIAL_CAMERA,
        TUTORIAL_GAME_CLEAR,
    }

    [SerializeField]
    private Text _tutorialTitle;

    [SerializeField]
    private Text _tutorialText;

    [SerializeField]
    private GameObject _focusCursolPanel;

    [SerializeField]
    private GameObject _focusAnglePanel;

    [SerializeField]
    private float _fadeSpeed; // �����x���ς��X�s�[�h���Ǘ�

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private GameObject _spotLight;
    
    /// <summary>
    /// �p�l���̃t�F�[�h�������t���O
    /// </summary>
    public bool ActiveFadePanelFlg
    {
        get
        {
            return _activeFadePanelFlg;
        }
    }

    /// <summary>
    /// �p�l���̃t�F�[�h�����J�n�t���O�̏��
    /// </summary>
    public bool GetPanelEnabledChangeFlg
    {
        get
        {
            return _panelEnabledChangeFlg;
        }
    }

    /// <summary>
    /// �X�|�b�g���C�g���L�����擾
    /// </summary>
    public bool GetSpotLightEnable
    {
        get
        {
            return _spotLight.activeSelf;
        }
    }

    /// <summary>
    /// ���݂̃`���[�g���A����Ԃ�Ԃ�
    /// </summary>
    public int GetCurrentTutorialStatus
    {
        get
        {
            return _currentTutorialStatus;
        }
    }

    public static TutorialManager instance;

    private UnityEvent _focusPanelFlgEvent = new UnityEvent();

    // �`���[�g���A���^�X�N
    private ITutorialTask _currentTask;
    private List<ITutorialTask> _tutorialTask;
    private bool[] _tutorialTaskCompleteStatus;
    private int _tutorialTaskIndex;

    // �`���[�g���A���\���t���O
    private bool _isEnabled;

    // �`���[�g���A���^�X�N�̏����𖞂������ۂ̑J�ڗp�t���O
    private bool _taskExecuted = false;

    // �`���[�g���A���\������UI�ړ�����
    private float fade_pos_x = 350;

    // �t�F�[�h�p�̉�f���
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;


    private bool _activeFadePanelFlg;
    //private bool _anglePanelEnabled;

    // �e�L�X�g�擾�t���O
    private bool _updateTextFlg;

    // �J�[�\���ƃA���O���̃p�l���̕\��/��\�����s��
    private bool _panelEnabledChangeFlg;
    private int _repeatFadeCnt;

    // �����̃`���[�g���A�������s���Ă���̂���\���ϐ�
    private int _currentTutorialStatus;

    // instance�쐬
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        _tutorialTask = new List<ITutorialTask>();

        // �����`���[�g���A����`
        _tutorialTask.Add(new Introducetask());
        // ����ړ��`���[�g���A����`
        _tutorialTask.Add(new MovementTask());
        // �J�����A���O���؂�ւ��`���[�g���A����`
        _tutorialTask.Add(new CameraAngleTask());
        // �N���A�����m�F�`���[�g���A����`
        _tutorialTask.Add(new GameClearTask());
        // �Q�[���I�[�o�[�����m�F�`���[�g���A����`


        // �e�`���[�g���A���̊�����Ԃ�ێ�����z����`
        _tutorialTaskCompleteStatus = new bool[_tutorialTask.Count]; //new bool[_tutorialTask.Count - 1];

        // �t���O������
        for (var i = 0; i < _tutorialTaskCompleteStatus.Length; i++)
        {
            _tutorialTaskCompleteStatus[i] = false;
        }


        // �ŏ��̃`���[�g���A����ݒ�
        StartCoroutine(SetCurrentTask(_tutorialTask.First()));


        /**************************�p�l���֘A********************************/
        // �F���擾(�p�l���͂ǂ������F)
        _red = _focusCursolPanel.GetComponent<Image>().color.r;
        _green = _focusCursolPanel.GetComponent<Image>().color.g;
        _blue = _focusCursolPanel.GetComponent<Image>().color.b;
        _alpha = _focusCursolPanel.GetComponent<Image>().color.a;

        // �t�H�[�J�X�p�l���𓧖��ɂ���
        _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        _repeatFadeCnt = 0;
        _activeFadePanelFlg = false;
        _panelEnabledChangeFlg = false;
        
        /**************************�X�|�b�g���C�g�֘A************************/
        //_spotLight = GameObject.Find("SpotLight");
        // GameClearTutorial��ɑ���ł���悤�ɃX�|�b�g���C�g�͔�\���ɂ���
        _spotLight.SetActive(false);

        /**************************�X�|�b�g���C�g�֘A************************/
        // CameraTutorial��ɑ���ł���悤��MainCamera�͔�\���ɂ���
        _mainCamera.enabled = false;

        //// 0��`����
        //_tutorialTaskIndex = -1;
        _currentTutorialStatus = -1;
        _isEnabled = true;


    }

    /// <summary>
    /// �V�����`���[�g���A���^�X�N��ݒ肷��
    /// </summary>
    /// <param name="task"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator SetCurrentTask(ITutorialTask task, float time = 0)
    {
        // time���w�肳��Ă���ꍇ�͑ҋ@
        yield return new WaitForSeconds(time);

        // ���݂̃`���[�g���A����ݒ�
        _currentTask = task;
        // ���݂̃`���[�g���A�������t���O
        _taskExecuted = false;

        // �`���[�g���A���^�X�N�ݒ莞�p�̊֐������s
        task.OnTaskSetting();

        // UI�Ƀ^�C�g���Ɛ�������ݒ�
        _tutorialTitle.text = GetTitle(task.GetTitleIndex());
        _tutorialText.text = task.GetText();
        _updateTextFlg = true;
        _currentTutorialStatus++;
        //_tutorialTaskIndex++;

    }
    
    void Update()
    {
        // ���s���̃`���[�g���A���̐��������擾
        // TransitionTime��1�ȏゾ�ƃG���[
        if (_updateTextFlg)
        {
            _tutorialText.text = _currentTask.GetText();
        }

        // panel�̕\��/��\������
        if (_panelEnabledChangeFlg)
        {
            FadeFocusPanel();
        }

        // ���݂̃`���[�g���A����Ƃ������������_��
        // ��x����͎󂯕t���Ȃ��悤�Ƀp�l����\��������(�����x1)
        if (_currentTask != null && _currentTask.IsTutorialComplete())
        {
            if (!_tutorialTaskCompleteStatus[_currentTask.GetTitleIndex()]) //[_tutorialTaskIndex])
            {
                //Debug.Log(_tutorialTitle.text);
                //Debug.Log("�`���[�g���A���C���f�b�N�X�F");
                //Debug.Log(_tutorialTaskIndex);
                _tutorialTaskCompleteStatus[_currentTask.GetTitleIndex()] = true; //[_tutorialTaskIndex] = true;
            }

            ResetPanel();
        }

        // �`���[�g���A�������݂����s����Ă��Ȃ��ꍇ�ɏ���
        if (_currentTask != null && !_taskExecuted)
        {
            // ���݂̃`���[�g���A�������s���ꂽ������
            if (_currentTask.CheckTask())
            {
                _taskExecuted = true;

                // ���ݐݒ肳��Ă���`���[�g���A�����폜
                _tutorialTask.RemoveAt(0);

                Debug.Log("delete current tutorial");

                var nextTask = _tutorialTask.FirstOrDefault();

                // �`���[�g���A�����܂��c���Ă���Ȃ玟�̃`���[�g���A�������s
                if (nextTask != null)
                {
                    _updateTextFlg = false;
                    StartCoroutine(SetCurrentTask(nextTask, 1f));
                }
            }
        }
    }

    private string GetTitle(int titleIndex)
    {
        string retTitle = "";

        switch (titleIndex)
        {
            case (int)TutorialTitle.TUTORIAL_INTRODUCE:
                retTitle = "�Q�[������ �T�v";
                break;
            case (int)TutorialTitle.TUTORIAL_MOVEMENT:
                retTitle = "����m�F �ړ�";
                break;
            case (int)TutorialTitle.TUTORIAL_CAMERA:
                retTitle = "����m�F �J����";
                break;
            case (int)TutorialTitle.TUTORIAL_GAME_CLEAR:
                retTitle = "����m�F �Q�[���N���A";
                break;
        }

        return retTitle;
    }



    // �C�x���g�֘A����(�o�^)
    public void SetPanelEnabledChangeFlgEvent()
    {
        _focusPanelFlgEvent.AddListener(SetFocusPanelFlg);
    }

    // �C�x���g�֘A����(���s)
    public void CallPanelEnabledChangeFlgEvent()
    {
        _focusPanelFlgEvent.Invoke();
    }

    // �C�x���g�֘A����(�폜)
    public void RemovePanelEnabledChangeFlgEvent()
    {
        _focusPanelFlgEvent.RemoveListener(SetFocusPanelFlg);
    }

    private void SetFocusPanelFlg()
    {
        if (_panelEnabledChangeFlg)
        {
            _panelEnabledChangeFlg = false;
        }
        else
        {
            _panelEnabledChangeFlg = true;
        }
    }


    // �t�F�[�h�J�n
    private void FadeFocusPanel()
    {
        // �t�F�[�h������
        _activeFadePanelFlg = true;

        // �w�肵���X�s�[�h��1�t���[���������x�𑫂��Ă���
        _alpha += _fadeSpeed * Time.deltaTime;

        SetAlpha();

        // �ݒ�l��MAX�̏ꍇ(�ő�l��1.0)
        if (_alpha >= 0.5f)
        {
            _alpha = 0;
            _repeatFadeCnt++;

            ChangeFocusPanelEnabled();
        }

    }

    private void ChangeFocusPanelEnabled()
    {
        // ���񐔃t�F�[�h�������Ă��Ȃ��Ȃ�I��
        if (_repeatFadeCnt < FADE_COUNT)
        {
            return;
        }

        // �t�F�[�h�A�E�g�I�����ē����x�����Z�b�g����
        _panelEnabledChangeFlg = false;

        if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_MOVEMENT)
        {
            _focusCursolPanel.SetActive(false);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_CAMERA)
        {
            _focusAnglePanel.SetActive(false);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_GAME_CLEAR)
        {
            _focusCursolPanel.SetActive(false);
            _focusAnglePanel.SetActive(false);
            _spotLight.SetActive(true);
        }

        // �t�F�[�h���������񐔂������߁A���`���[�g���A�������̂��߂ɏ�����
        _repeatFadeCnt = 0;
        _activeFadePanelFlg = false;
        Debug.Log("activePanel false");
        Debug.Log(_activeFadePanelFlg);
    }

    private void ResetPanel()
    {
        Debug.Log("ResetPanel");
        // �ړ��`���[�g���A����
        if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_MOVEMENT)
        {
            _focusCursolPanel.SetActive(true);
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        }
        // �J�����`���[�g���A����
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_CAMERA)
        {
            _focusAnglePanel.SetActive(true);
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        }
        // �Q�[���N���A�m�F�`���[�g���A����
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_GAME_CLEAR)
        {
            _focusCursolPanel.SetActive(true);
            _focusAnglePanel.SetActive(true);
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, 0);
        }
    }
        // Panel��Image��f��ݒ肷��
    private void SetAlpha()
    {
        if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_MOVEMENT)
        {
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_CAMERA)
        {
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
        }
        else if (_currentTask.GetTitleIndex() == (int)TutorialTitle.TUTORIAL_GAME_CLEAR)
        {
            _focusCursolPanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
            _focusAnglePanel.GetComponent<Image>().color = new Color(_red, _green, _blue, _alpha);
        }
    }

    public bool GetTutorialTaskCompStatus(int index)
    {
        //Debug.Log("�z��"+ _tutorialTaskCompleteStatus.Length);
        return _tutorialTaskCompleteStatus[index];
    }
}
