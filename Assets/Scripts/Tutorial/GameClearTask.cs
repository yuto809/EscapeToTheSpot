using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearTask : ITutorialTask
{
    public enum TriggerMessage
    {
        TRIGGER_MESSAGE_7 = 7,
        TRIGGER_MESSAGE_8,
    }

    private TutorialManager _tutorialManager;
    private GameObject _spotLight;
    private TutorialUnityChanController _unityChan;
    private SpotCreator _spotCreator;

    //�@�L�����N�^�[�̑��x
    private Vector3 _unityPos;

    private string[] _textSentence;

    private bool _showMessageComplete;
    private bool _tutorialComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;

    private bool _isCalled;
    private bool _isCalled_2;

    private bool _tutorialGameClearComplete;
    private bool _tutorialAllComplete;


    public void OnTaskSetting()
    {
        // TutorialManager�擾
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _spotLight = GameObject.Find("SpotLight");
        _unityChan = GameObject.Find("TutorialUnityChan").GetComponent<TutorialUnityChanController>();
        // SpotCreator�C���X�^���X�擾
        //_spotCreator = GameObject.Find("SpotLight").GetComponent<SpotCreator>();

        _textSentence = new string[]
        {
            "���́A���̃Q�[���̃N���A�����ł��B",
            "�X�e�[�W��ɁA�X�|�b�g���C�g���Ƃ炳�ꂽ�ꏊ�A�X�|�b�g�G���A���o�����܂��B",
            "�G��A��Q��������Ȃ���A�X�|�b�g�G���A��ڎw���Ă�������!!",
            "�X�|�b�g�G���A�͎��Ԃ��o�߂���x�ɁA�������Ȃ�����Ă��܂��܂��B",
            "�X�|�b�g�G���A�������Ă��܂��O�ɁA���̒���3�b�ԗ��܂邱�Ƃ��ł�����A�X�e�[�W�N���A�ƂȂ�܂�!!",
            "�X�|�b�g�G���A�������Ă��܂��ƁA�����_���ŐV���ȃX�|�b�g�G���A���o�����܂��B",
            "���̂Ƃ��́A�J�����A���O����؂�ւ��ăX�|�b�g�G���A�������āA�f�����ړ����܂��傤!!",
            "�c!! �X�|�b�g�G���A���o�����܂���!!",
            "�ł͎��ۂɁA�X�|�b�g�G���A��ڎw���āA�ړ����Ă݂܂��傤�B",
            "�����ɃX�|�b�g�G���A�܂ŒH�蒅�����Ƃ��ł��܂�����!! ����ŃQ�[���N���A�ƂȂ�A���̃X�e�[�W�ɐi�ނ��Ƃ��ł��܂�!!",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialComplete = false;

        _isCalled = false;
        _isCalled_2 = false;

        _tutorialAllComplete = false;

        // �C�x���g�o�^
        _tutorialManager.SetPanelEnabledChangeFlgEvent();
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_GAME_CLEAR;
    }

    public string GetText()
    {
        return _currentSenetnce;
    }


    public bool CheckTask()
    {
        // ���ݕ\�������ׂ����b�Z�[�W���e�����ׂĕ\������Ă��Ȃ��ꍇ
        if (!_showMessageComplete)
        {
            if (CheckSentence())
            {
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

            // �X�|�b�g�G���A�o��������
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_7)
            {
                // _spotLight�͂��̎��_�Ŕ񊈐���ԂȂ̂ŁA�擾�����܂������Ă��Ȃ�
                // TutorialManager���ŕύX����K�v������

                _tutorialManager.CallPanelEnabledChangeFlgEvent();

                //_spotLight.SetActive(true);
                _isCalled = true;

                // �ŏ���1��ڂ́AunityChan�������Ă�������Ƃ͋t�ɏo��������
                //_spotCreator.ReCreateSpotArea();
            }

            //// ���胁�b�Z�[�W��ǂݍ��񂾂�A�t�H�[�J�X��������C�x���g��Tutorial���ɓ`����
            //if (!_isCalled_2 && _currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_9)
            //{
            //    //_tutorialManager.CallPanelEnabledChangeFlgEvent();
            //    _isCalled_2 = true;
            //    Debug.Log("GameClearTutorial CallPanelEnabledChangeFlgEvent");
            //}
        }
        // ���ݕ\�������ׂ����b�Z�[�W���e�����ׂĕ\���ł��Ă���ꍇ
        else
        {
            if (_currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_8)
            {
                // �X�|�b�g�G���A�ɗ��܂邱�Ƃ��ł��������f����
                if (CheckTutorialGameClear())
                {
                    // ���b�Z�[�W�����������āA���̃��b�Z�[�W���e��
                    SetNextSentenceInfo();
                }
            }
            else if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap����
            {
                // ���݂̃`���[�g���A���ł��ׂẴ��b�Z�[�W���\���o������`���[�g���A���I��
                if (_tutorialAllComplete)
                {
                    Debug.Log("GameClearTutorial����");
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

    // �X�|�b�g�G���A���ɗ��܂邱�Ƃ��ł�����`���[�g���A���͏I���Ɣ��f����
    private bool CheckTutorialGameClear()
    {
        ////Debug.Log("�ړ�Check");
        //if (_unityChan.transform.position.x > MOVE_POS_RIGHT)
        //{
        //    _moveRightFlg = true;
        //}

        //if (_unityChan.transform.position.x < MOVE_POS_LEFT)
        //{
        //    _moveLeftFlg = true;
        //}

        //if (_unityChan.transform.position.z > MOVE_POS_UP)
        //{
        //    _moveUpFlg = true;
        //}

        //if (_unityChan.transform.position.z < MOVE_POS_DOWN)
        //{
        //    _moveDownFlg = true;
        //}

        //// �ړ����삪�����������ǂ����H
        //if ((_moveUpFlg & _moveRightFlg & _moveDownFlg & _moveLeftFlg))
        //{
        //    Debug.Log("�ړ��`���[�g���A������");

        //    //textIndex++;

        //    _tutorialMovementComplete = true;
        //    // ���̃`���[�g���A�����s�܂Ńp�l����L���ɖ߂����H
        //    return true;
        //}

        //_tutorialGameClearComplete = true;

        return false;
    }


    private bool CheckSentence()
    {
        // ���ׂẴ��b�Z�[�W��\�����Ă���ꍇ�͏������X�L�b�v
        if (_currentSenetenceIndex >= _textSentence.Length)
        {
            Debug.Log("tutorial GameClear complete set");
            _tutorialAllComplete = true;

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

    private void CreateSpotArea()
    {

    }

}
