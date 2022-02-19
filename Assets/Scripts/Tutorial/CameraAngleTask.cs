using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngleTask : ITutorialTask
{
    public enum TriggerCameraMessage
    {
        TRIGGER_MESSAGE_3 = 3,
        TRIGGER_MESSAGE_4,
    }

    private int textIndex;
    private List<string> textMessages = new List<string>();
    private string[] _textSentence;
    private TutorialManager _tutorialManager;
    private Camera _mainCamera;

    private bool _angleChangeFlg;
    private bool _angleDefaultFlg;

    private bool _showMessageComplete;
    private bool _tutorialComplete;

    private bool _isCalled;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;

    public void OnTaskSetting()
    {
        // TutorialManager�擾
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        _mainCamera.enabled = false;

        textIndex = 0;

        // 0origin
        //textMessages.Add("���́A�J�����A���O���̐؂�ւ��ł��B");
        //textMessages.Add("�Q�[����i�߂Ă����ƁA�L���X�e�[�W���o�����邱�Ƃ�����܂��B");
        //textMessages.Add("���̂��߁A���̃J�����A���O���ŃX�|�b�g���f���o���Ȃ��ꍇ������܂��B");
        //textMessages.Add("���̂Ƃ��́A�J�����{�^�����������邱�ƂŁA�J�����A���O���̐؂�ւ����s���܂��B");
        //textMessages.Add("�ł́A�J�����A���O����؂�ւ��Ă݂܂��傤�B");
        //textMessages.Add("���Ȃ��ł��ˁB����ł́A���̑�������ł��B");


        _textSentence = new string[]
        {
            "���́A�J�����A���O���̐؂�ւ��ł��B",
            "�Q�[����i�߂Ă����ƁA�L���X�e�[�W���o�����邱�Ƃ�����܂��B",
            "���̂��߁A���̃J�����A���O���ŃX�|�b�g���f���o���Ȃ��ꍇ������܂��B",
            "���̂Ƃ��́A�J�����{�^�����������邱�ƂŁA�J�����A���O���̐؂�ւ����s���܂��B",
            "�ł́A�J�����A���O����؂�ւ��Ă݂܂��傤�B",
            "���Ȃ��ł��ˁB����ł́A���̑�������ł��B",
        };

        _angleChangeFlg = false;
        _angleDefaultFlg = false;

        _isCalled = false;

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialComplete = false;

        // �C�x���g�o�^
        _tutorialManager.SetPanelEnabledChangeFlgEvent();
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_CAMERA;
    }

    public string GetText()
    {
        return _currentSenetnce;
    }


    public bool CheckTask()
    {
        //// �p�l���̃t�F�[�h�������͊m�F�����̓X�L�b�v����
        //if (_tutorialManager.ActiveFadePanelFlg)
        //{
        //    return false;
        //}



        // ���ݕ\�������ׂ����b�Z�[�W���e�����ׂĕ\������Ă��Ȃ��ꍇ
        if (!_showMessageComplete)
        {

            if (CheckSentence())
            {
                //Debug.Log(_textSentence[_currentSenetenceIndex][_currentCharIndex]);
                // �����t���O��ݒ�
                _showMessageComplete = true;
            }
            else
            {
                // �\������郁�b�Z�[�W��1�������擾���Đݒ肷��
                _currentSenetnce = _currentSenetnce + _textSentence[_currentSenetenceIndex][_currentCharIndex];

                // ����1������
                _currentCharIndex++;

            }

            Debug.Log(_currentSenetnce);
            Debug.Log(_currentSenetnce.Length);
            Debug.Log(_textSentence.Length);

            // ���胁�b�Z�[�W��ǂݍ��񂾂�A�t�H�[�J�X��������C�x���g��Tutorial���ɓ`����
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerCameraMessage.TRIGGER_MESSAGE_3)
            {
                _tutorialManager.CallPanelEnabledChangeFlgEvent();
                _isCalled = true;
                Debug.Log("CallPanelEnabledChangeFlgEvent");
            }
        }
        // ���ݕ\�������ׂ����b�Z�[�W���e�����ׂĕ\���ł��Ă���ꍇ
        else
        {
            if (_currentSenetenceIndex == (int)TriggerCameraMessage.TRIGGER_MESSAGE_4)
            {
                // ���O�Ɍ��߂�ꂽ�������ړ�������ړ��`���[�g���A���͏I���Ɣ��f����
                if (CheckTutorialAngle())
                {
                    // ���b�Z�[�W�����������āA���̃��b�Z�[�W���e��
                    SetNextSentenceInfo();
                }
            }
            else if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap����
            {
                // ���݂̃`���[�g���A���ł��ׂẴ��b�Z�[�W���\���o������`���[�g���A���I��
                if (_tutorialComplete)//_currentSenetenceIndex == _currentSenetnce.Length)
                {
                    // �C�x���g�폜
                    _tutorialManager.RemovePanelEnabledChangeFlgEvent();
                    return true;
                }
                else
                {
                    // ���b�Z�[�W�����������āA���̃��b�Z�[�W���e��
                    SetNextSentenceInfo();
                }
            }
        }


        //// ���O�ɗp�ӂ��Ă���e�L�X�g���b�Z�[�W�����ׂĕ\������
        //// ���O�Ɍ��߂�ꂽ�������ړ�������ړ��`���[�g���A���͏I���Ɣ��f����
        //if (CheckTutorialAngle() && (textIndex >= (textMessages.Count))) // - 1))) //!_tutorialManager.CursolPanelEnabledFlg)
        //{
        //    // �C�x���g�폜
        //    _tutorialManager.RemovePanelEnabledChangeFlgEvent();

        //    Debug.Log("���̃`���[�g���A�����b�Z�[�W��");
        //    return true;
        //}

        //// ���胁�b�Z�[�W��ǂݍ��񂾂�A�t�H�[�J�X��������C�x���g��Tutorial���ɓ`����
        //// �ړ��`���[�g���A�����炾�Ƃ킩��悤�ɁA�񋓌^�Ƃ��ŊǗ�����H
        //if (textIndex == 3)
        //{
        //    _tutorialManager.CallPanelEnabledChangeFlgEvent();
        //    //textIndex++;
        //}

        //// �ړ��`���[�g���A�������Ă���ꍇ�͏������Ȃ�
        //if (Input.GetMouseButtonDown(0) && textIndex != 4)// (Input.touchCount == 1) tap����
        //{
        //    textIndex++;
        //}

        return false;
    }

    public float GetTransitionTime()
    {
        return 0f;
    }

    public bool IsTutorialComplete()
    {
        return _tutorialComplete;
    }

    private bool CheckSentence()
    {
        // ���ׂẴ��b�Z�[�W��\�����Ă���ꍇ�͏������X�L�b�v
        if (_currentSenetenceIndex >= _textSentence.Length)
        {
            Debug.Log("camera tutorial complete set");
            _tutorialComplete = true;
            // SetNextSentenceInfo�ɂ���ă��b�Z�[�W������������Ă��邽��
            // �Ō�̕��͂�ݒ�
            _currentSenetnce = _textSentence[_currentSenetenceIndex - 1];
            return true;
        }

        // ���������ׂĕ\�����邱�Ƃ��ł�����I��
        if (_currentSenetnce.Length == _textSentence[_currentSenetenceIndex].Length)
        {
            return true;
        }

        return false;
    }


    private void SetNextSentenceInfo()
    {
        _currentSenetenceIndex++;
        _currentCharIndex = 0;
        _currentSenetnce = "";
        _showMessageComplete = false;
    }

    // �J�����A���O���̐؂�ւ�(ON/OFF)������������`���[�g���A���͏I���Ɣ��f����
    private bool CheckTutorialAngle()
    {
        //Debug.Log("�J����Check");
        if (_mainCamera.enabled)
        {
            _angleChangeFlg = true;
        }

        // 1�x�J�����A���O����؂�ւ��Ă��銎��
        // default�ɖ߂����ꍇ
        if (_angleChangeFlg && !_mainCamera.enabled)
        {
            _angleDefaultFlg = true;
        }

        // �ړ����삪�����������ǂ����H
        if ((_angleChangeFlg & _angleDefaultFlg))
        {
            //Debug.Log("�J�����`���[�g���A������");

            //textIndex++;
            //Debug.Log(textIndex);

            // ���̃`���[�g���A�����s�܂Ńp�l����L���ɖ߂����H
            return true;
        }

        return false;
    }
}
