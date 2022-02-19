public interface ITutorialTask
{
    /// <summary>
    /// �`���[�g���A���̃^�C�g��(index)���擾����
    /// </summary>
    /// <returns></returns>
    int GetTitleIndex();

    /// <summary>
    /// ���������擾����
    /// </summary>
    /// <returns></returns>
    string GetText();

    /// <summary>
    /// �`���[�g���A���^�X�N���ݒ肳�ꂽ�ۂɎ��s�����
    /// </summary>
    void OnTaskSetting();

    /// <summary>
    /// �`���[�g���A�����B�����ꂽ�����肷��
    /// </summary>
    /// <returns></returns>
    bool CheckTask();

    /// <summary>
    /// �B����Ɏ��̃^�X�N�֑J�ڂ���܂ł̎���(�b)
    /// </summary>
    /// <returns></returns>
    float GetTransitionTime();

    bool IsTutorialComplete();
}

