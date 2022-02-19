using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTask : ITutorialTask
{
    public enum TriggerMessage
    {
        TRIGGER_MESSAGE_1 = 1,
        TRIGGER_MESSAGE_2,
    }

    // Unitychan�̏����ʒu(X:0, Y:0.5, Z:-5)

    // ���苗���ړ��������ǂ������f�萔(�������K�v)
    const float MOVE_POS_UP = -3.5f;
    const float MOVE_POS_RIGHT = 1.5f;
    const float MOVE_POS_DOWN = -7.0f;
    const float MOVE_POS_LEFT = -1.0f;

    private int textIndex;
    private List<string> textMessages = new List<string>();
    private string[] _textSentence;
    private TutorialManager _tutorialManager;
    private TutorialUnityChanController _unityChan;

    private bool _moveUpFlg;
    private bool _moveRightFlg;
    private bool _moveDownFlg;
    private bool _moveLeftFlg;

    private bool _isCalled;

    private bool _showMessageComplete;
    private bool _tutorialMovementComplete;
    private bool _tutorialAllComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;


    public void OnTaskSetting()
    {
        // TutorialManager�擾
        //_tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _tutorialManager = TutorialManager.instance;
        _unityChan = GameObject.Find("TutorialUnityChan").GetComponent<TutorialUnityChanController>();
        
        textIndex = 0;
        _moveUpFlg = false;
        _moveRightFlg = false;
        _moveDownFlg = false;
        _moveLeftFlg = false;

        _isCalled = false;

        // 0origin
        textMessages.Add("�܂��́A�L�����N�^�[�̈ړ����@�ł��B");
        textMessages.Add("WSAD�L�[�A�܂��̓J�[�\���L�[�ŏ㉺���E�Ɉړ��ł��܂��B");
        textMessages.Add("�ł́A���ۂɃL�����N�^�[���ړ������Ă݂܂��傤�B");
        textMessages.Add("���Ȃ��ł��ˁB����ł́A���̑�������ł��B");

        _textSentence = new string[]
        {
            "�܂��́A�L�����N�^�[�̈ړ����@�ł��B",
            "WSAD�L�[�A�܂��̓J�[�\���L�[�ŏ㉺���E�Ɉړ��ł��܂��B",
            "�ł́A���ۂɃL�����N�^�[���ړ������Ă݂܂��傤�B",
            "���Ȃ��ł��ˁB����ł́A���̑�������ł��B",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialAllComplete = false;
        // �C�x���g�o�^
        _tutorialManager.SetPanelEnabledChangeFlgEvent();
    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_MOVEMENT;
    }

    public string GetText()
    {
        if (textIndex >= textMessages.Count)
        {
            return textMessages[textMessages.Count - 1];
        }
        else
        {
            return _currentSenetnce; // textMessages[textIndex];
        }
    }

    // TutorialManger��Update����
    public bool CheckTask()
    {
        // �p�l���̃t�F�[�h�������͊m�F�����̓X�L�b�v����
        //if (_tutorialManager.ActiveFadePanelFlg)
        //{
        //    //Debug.Log("SKIPPPPPPPPPPPP");
        //    return false;
        //}

        // ���ݕ\�������ׂ����b�Z�[�W���e�����ׂĕ\���ł��������f
        //if (CheckSentence() && !_showMessageComplete)
        //{
        //    // �����t���O��ݒ�
        //    _showMessageComplete = true;
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

            //Debug.Log(_currentSenetnce);
            //Debug.Log(_currentSenetnce.Length);
            //Debug.Log(_textSentence.Length);

            // ���胁�b�Z�[�W��ǂݍ��񂾂�A�t�H�[�J�X��������C�x���g��Tutorial���ɓ`����
            if (!_isCalled && _currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_1)
            {
                _tutorialManager.CallPanelEnabledChangeFlgEvent();
                _isCalled = true;
                Debug.Log("CallPanelEnabledChangeFlgEvent");
            }
        }
        // ���ݕ\�������ׂ����b�Z�[�W���e�����ׂĕ\���ł��Ă���ꍇ
        else
        {
            if (_currentSenetenceIndex == (int)TriggerMessage.TRIGGER_MESSAGE_2)
            {
                // ���O�Ɍ��߂�ꂽ�������ړ�������ړ��`���[�g���A���͏I���Ɣ��f����
                if (CheckTutorialMove())
                {
                    // ���b�Z�[�W�����������āA���̃��b�Z�[�W���e��
                    SetNextSentenceInfo();
                }
            }
            else if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap����
            {
                // ���݂̃`���[�g���A���ł��ׂẴ��b�Z�[�W���\���o������`���[�g���A���I��
                if (_tutorialAllComplete)//_currentSenetenceIndex == _currentSenetnce.Length)
                {
                    //Debug.Log("ZXXXXXXXX");
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











        ///////////////////////////////////////////// old


        //// ���O�ɗp�ӂ��Ă���e�L�X�g���b�Z�[�W�����ׂĕ\������
        //// ���O�Ɍ��߂�ꂽ�������ړ�������ړ��`���[�g���A���͏I���Ɣ��f����
        //// textMessage.Count�͌����4�Œ�(���b�Z�[�W��)
        //if (CheckTutorialMove() && (textIndex >= (textMessages.Count))) //!_tutorialManager.CursolPanelEnabledFlg)
        //{
        //    // �`���[�g���A���������3
        //    textIndex++;
        //    //Debug.Log(textIndex);
        //    // �C�x���g�폜
        //    _tutorialManager.RemovePanelEnabledChangeFlgEvent();
        //    return true;
        //}

        //// ���O�ƃ��A���������index���X�V����Ȃ����烁�b�Z�[�W���ǂݍ��܂�Ȃ�

        //// �ړ��`���[�g���A�������Ă���ꍇ�͏������Ȃ�
        //if (Input.GetMouseButtonDown(0) && textIndex != 2)// (Input.touchCount == 1) tap����
        //{
        //    textIndex++;
        //    //Debug.Log(textIndex);
        //}

        //// ���胁�b�Z�[�W��ǂݍ��񂾂�A�t�H�[�J�X��������C�x���g��Tutorial���ɓ`����
        //// �ړ��`���[�g���A�����炾�Ƃ킩��悤�ɁA�񋓌^�Ƃ��ŊǗ�����H
        //else if (!_isCalled && textIndex == 1)
        //{
        //    _tutorialManager.CallPanelEnabledChangeFlgEvent();
        //    _isCalled = true;
        //    Debug.Log("CallPanelEnabledChangeFlgEvent");
        //    //textIndex++;
        //}



        //Debug.Log(_unityChan.transform.position.x);
        //Debug.Log(_unityChan.transform.position.z);


        return false;
    }

    public float GetTransitionTime()
    {
        return 0f;
    }

    public bool IsTutorialComplete()
    {
        return _tutorialMovementComplete;
    }

    // �ړ������̍��v�l�ɂ���

    // ���O�Ɍ��߂�ꂽ�������ړ�������ړ��`���[�g���A���͏I���Ɣ��f����
    private bool CheckTutorialMove()
    {
        //Debug.Log("�ړ�Check");
        if (_unityChan.transform.position.x > MOVE_POS_RIGHT)
        {
            _moveRightFlg = true;
        }

        if (_unityChan.transform.position.x < MOVE_POS_LEFT)
        {
            _moveLeftFlg = true;
        }

        if (_unityChan.transform.position.z > MOVE_POS_UP)
        {
            _moveUpFlg = true;
        }

        if (_unityChan.transform.position.z < MOVE_POS_DOWN)
        {
            _moveDownFlg = true;
        }

        // �ړ����삪�����������ǂ����H
        if ((_moveUpFlg & _moveRightFlg & _moveDownFlg & _moveLeftFlg))
        {
            Debug.Log("�ړ��`���[�g���A������");

            //textIndex++;

            _tutorialMovementComplete = true;
            // ���̃`���[�g���A�����s�܂Ńp�l����L���ɖ߂����H
            return true;
        }

        return false;
    }

    private bool CheckSentence()
    {
        // ���ׂẴ��b�Z�[�W��\�����Ă���ꍇ�͏������X�L�b�v
        if (_currentSenetenceIndex >= _textSentence.Length)
        {
            Debug.Log("move tutorial complete set");
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


}
