using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introducetask : ITutorialTask
{
    private int textIndex;
    private List<string> textMessages = new List<string>();
    private string[] _textSentence;

    private bool _showMessageComplete;
    private bool _tutorialComplete;

    private int _currentSentenceNumber;
    private int _currentCharIndex;
    private int _currentSenetenceIndex;
    private string _currentSenetnce;

    public void OnTaskSetting()
    {
        textIndex = 0;
        
        textMessages.Add("���߂܂���!!");
        textMessages.Add("Escape To The Run�ւ悤����");
        textMessages.Add("���̃Q�[���́A�w�L�����N�^�[�𑀍삵�āA�X�e�[�W��ɏo������X�|�b�g�G���A�Ɍ������ē�����x��ړI�Ƃ����A�A�N�V�����Q�[���ƂȂ��Ă��܂��B");
        textMessages.Add("�����ł́A�G���o�����邱�Ƃ�����̂ŁA��肭�����Ȃ���X�|�b�g�֓����Ă��������c!!");
        textMessages.Add("����ł́A�Q�[���̑�����@��������Ă����܂�");

        _textSentence = new string[]
        {
            "���߂܂���!!",
            "Escape To The Run�ւ悤����!!",
            "���̃Q�[���́A�w�L�����N�^�[�𑀍삵�āA�X�e�[�W��ɏo������X�|�b�g�G���A�Ɍ������ē�����x��ړI�Ƃ����A�A�N�V�����Q�[���ƂȂ��Ă��܂��B",
            "�����ł́A�G���o�����邱�Ƃ�����̂ŁA��肭�����Ȃ���X�|�b�g�֓����Ă��������c!!",
            "����ł́A�Q�[���̑�����@��������Ă����܂��B",
        };

        _currentSentenceNumber = _textSentence.Length;
        _currentCharIndex = 0;
        _currentSenetenceIndex = 0;
        _showMessageComplete = false;
        _currentSenetnce = "";

        _tutorialComplete = false;

    }

    public int GetTitleIndex()
    {
        return (int)TutorialManager.TutorialTitle.TUTORIAL_INTRODUCE;
    }

    public string GetText()
    {
        if (textIndex >= textMessages.Count)
        {
            return textMessages[textMessages.Count - 1];
        }
        else
        {
            return _currentSenetnce; //textMessages[textIndex];
        }
    }


    public bool CheckTask()
    {
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
        }
        // ���ݕ\�������ׂ����b�Z�[�W���e�����ׂĕ\���ł��Ă���ꍇ
        else
        {
            if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1) tap����
            {
                // ���݂̃`���[�g���A���ł��ׂẴ��b�Z�[�W���\���o������`���[�g���A���I��
                if (_tutorialComplete)
                {
                    Debug.Log("Introduce����");
                    return true;
                }
                else
                {
                    // ���b�Z�[�W�����������āA���̃��b�Z�[�W���e��
                    SetNextSentenceInfo();
                }
            }
        }

        


        //// ���O�ɗp�ӂ��Ă���e�L�X�g���b�Z�[�W�����ׂĕ\����������
        //// ���̃`���[�g���A����
        //if (textIndex >= textMessages.Count)
        //{
        //    return true;
        //}


        //if (Input.GetMouseButtonDown(0))// (Input.touchCount == 1)
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
            Debug.Log("tutorial complete set");
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

}
