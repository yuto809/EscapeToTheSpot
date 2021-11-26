using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    //�X�e�[�W���x��
    enum StageLevel
    {
        EASY = 0,
        NORMAL,
        HARD
    }

    // ��Q����Y���W/Z���W�ƃX�e�[�W����͂ݏo�����Ȃ��I�t�Z�b�g
    const float obstaclDropHight = 5.0f;
    const float obstaclDropZ = 12.5f;

    const float obsPosOffset = 2.0f;

    // ��Q���𐶐�����C���^�[�o��
    const float createTime = 2.0f;

    [SerializeField]
    // �O��������ł����Q��
    private GameObject frontObstacle;

    // ��Q�����X�g
    [SerializeField]
    private GameObject[] obstacles;

    // ��Q���̍ő吔(Easy/Normal)
    // Hard��2�{�ɂ���
    [SerializeField]
    private int obstacleMax = 10;

    private int obstacleNum = 0;
    private GameManager gameManager;
    private Vector3 vector;
    private float obsRange_X = 0.0f;
    private float obsRange_Z = 0.0f;
    private float obsDropRange_Z = 0.0f;
    private int createCount = 0;
    private float elapsedTime = 0.0f;
    private float counter = 0.0f;
    private Coroutine createCoroutine;
    private StageManager stageManager;
    private List<GameObject> objList;

    void Start()
    {
        // GameManager�C���X�^���X�擾
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        // �I�����Ă���X�e�[�W��Hard�̏ꍇ
        if ((int)StageLevel.HARD == stageManager.SelectStageLevel)
        {
            obstacleNum = obstacleMax * 3;
            obsDropRange_Z = obstaclDropZ * 1.75f;
        }
        else
        {
            obstacleNum = obstacleMax;
            obsDropRange_Z = obstaclDropZ;
        }

        // ��Q���𐶐����邽�߂͈̔͂��擾
        obsRange_X = (stageManager.StageScale_x / 2) - obsPosOffset;
        obsRange_Z = (stageManager.StageScale_z / 2) - obsPosOffset;

        objList = new List<GameObject>();
    }

    void Update()
    {
        if (false == gameManager.GameClearFlg)
        {
            // �X�e�[�W��ɏ�Q�����쐬
            CreateObstacle();
            CreateFrontObstacle();
        }
        else
        {
            Debug.Log(objList.Count);

            // �O������̏�Q����1�ł����݂���ꍇ
            if (0 != objList.Count)
            {
                // List�Ɋi�[�����I�u�W�F�N�g�͂��ׂč폜
                // �N���A��ɏ�Q���ɓ������ăQ�[���I�[�o�[�ƂȂ�̂�h������
                foreach (GameObject gameObj in objList)
                {
                    Destroy(gameObj);
                }
            }
        }
    }
    
    // �X�e�[�W��Ƀ����_���ŏ�Q�����쐬����
    private void CreateObstacle()
    {
        // ���łɏ�Q�����ő吔�����o����Ă���ꍇ�͏I��
        if (obstacleNum <= createCount)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        // 2�b�����ɏ�Q���𑝂₷
        if (elapsedTime > createTime)
        {
            CreateRandomObstacle();
            elapsedTime = 0.0f;
        }
    }

    private void CreateFrontObstacle()
    {
        counter += Time.deltaTime;

        // 4�b�����ɏ�Q���𑝂₷
        if (counter > createTime)
        {
            // �X�e�[�W��x�X�P�[����10�̂��߁Ax����-5~5�͈̔͂Ő������� ���͒�������
            GameObject frontOnj = Instantiate(frontObstacle, new Vector3(Random.Range(-obsRange_X, obsRange_X), obstaclDropHight, obsDropRange_Z), Quaternion.identity);

            /* List�ɃI�u�W�F�N�g�ێ����āA��莞�Ԍo�ߌ�ɉ�������� */
            objList.Add(frontOnj);

            counter = 0.0f;
        }
    }
    
    // �����_���ɏ�Q�������o��
    private void CreateRandomObstacle()
    {
        // �����_���̏�Q����index�擾
        int index = Random.Range(0, obstacles.Length);

        // ����������Q�����ő吔�𒴂��Ă��Ȃ��ꍇ
        if (obstacleNum >= createCount)
        {
            // ��Q������������
            GameObject obj = Instantiate(obstacles[index], GetRandomPosition(index), Quaternion.identity);

            // ����������Q�����J�E���g
            createCount++;
        }
    }

    // ���݂̃X�e�[�W�̃X�P�[��������ɁA��Q���̐����ꏊ�����߂�
    private Vector3 GetRandomPosition(int index)
    {
        //// ��Q���𐶐����邽�߂͈̔͂��擾
        //obsRange_X = (stageManager.StageScale_x / 2) - obsPosOffset;
        //obsRange_Z = (stageManager.StageScale_z / 2) - obsPosOffset;

        // ��Q���̍��W�����߂�
        obstacles[index].transform.position = new Vector3(Random.Range((-1) * obsRange_X, obsRange_X),
                                                          obstaclDropHight,
                                                          Random.Range((-1) * obsRange_Z, obsRange_Z));

        return obstacles[index].transform.position;
    }
}
