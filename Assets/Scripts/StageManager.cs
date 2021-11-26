using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    const int levelHard = 2;

    public enum StageLevel
    {
        Level1 = 0,
        Level2,
        Level3
    }

    // �X�e�[�W�̃X�P�[�������l
    const float orgStageScaleX = 20.0f;
    const float orgStageScaleZ = 20.0f;

    //�V���O���g���ݒ肱������
    static public StageManager instance;

    // �X�e�[�W���x��
    public int SelectStageLevel { set; get; }

    // ���݂̃X�e�[�W�̃X�P�[�����(x����)
    // �����l�̃X�e�[�W�X�P�[�������Z�b�g
    public float StageScale_x { set; get; }

    // ���݂̃X�e�[�W�̃X�P�[�����(z����)
    public float StageScale_z { set; get; }

    public bool UpdateStageScale { get; set; }

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

    private void Start()
    {
        SetStageInfo();
    }

    // �e���x���ɉ������X�e�[�W����ݒ�
    public void SetStageInfo()
    {
        if (levelHard == SelectStageLevel)
        {
            Debug.Log("Select Hard");
            StageScale_x = orgStageScaleX * 2;
            StageScale_z = orgStageScaleZ * 2;
        }
        else
        {
            StageScale_x = orgStageScaleX;
            StageScale_z = orgStageScaleZ;
        }

        UpdateStageScale = true;
    }
}
