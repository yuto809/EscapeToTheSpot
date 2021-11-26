using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotArea : MonoBehaviour
{
    const int countTime = 3;
    const float SPOT_ANGLE = 12.0f;

    [SerializeField]
    private string targetTag;

    [SerializeField]
    private UnityChanController unityChan;

    int counter = 0;
    Coroutine timerCoroutine;
    GameManager gameManager;
    SpotCreator spotCreator;
    Light spotLight;
    private AudioSource[] spotSE;
    private StageManager stageManager;
    private bool spotJudge;

    void Start()
    {
        // GameManager�C���X�^���X�擾
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        spotSE = GetComponents<AudioSource>();

        // SpotCreator�C���X�^���X�擾
        spotCreator = GameObject.Find("SpotLight").GetComponent<SpotCreator>();
        spotLight = spotCreator.GetComponent<Light>();

        spotJudge = false;
    }
    
    void Update()
    {
        if (gameManager.GameOverFlg)
        {
            // �R���[�`���������Ă���ꍇ
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
                spotSE[0].Stop();
                //gameManager.GameClearFlg = false;
                gameManager.GameOverFlgSet(false);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        // �Q�[���I�[�o�[�̏ꍇ�̓N���A���f�����͎��s�����Ȃ�
        if (gameManager.GameOverFlg)
        {
            //gameOverFlg = true;
            // �R���[�`���������Ă���ꍇ
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
            }
            // �G���A���ɗ��܂��Ă���Ƃ���SE���~�߂�
            spotSE[0].Stop();
            return;
        }

        // ���p�x�����ɂȂ�����Փ˔���͂��Ȃ�
        if (spotLight.spotAngle < SPOT_ANGLE)
        {
            // �R���[�`���������Ă���ꍇ
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
            }

            // �G���A���ɗ��܂��Ă���Ƃ���SE���~�߂�
            spotSE[0].Stop();
            return;
        }

        // �Q�[���I�[�o�[�ƂȂ��Ă��Ȃ��ꍇ ���A1�x���G���A���菈�����s���Ă��Ȃ��ꍇ
        if ((false == gameManager.GameOverFlg) && (false == spotJudge))
        {
            // UnityChan���R���C�_�[�͈͓��ɂ���ꍇ
            // �N���A���f���s��
            if (other.gameObject.tag == targetTag)
            {
                if (stageManager.SelectStageLevel == 2)
                {
                    gameManager.StaySpotArea = true;
                }

                timerCoroutine = StartCoroutine(TimeCount());
                spotSE[0].Play();
                spotJudge = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �R���C�_�[�͈͓�����UnityChan���o���ꍇ
        if (other.gameObject.tag == targetTag)
        {
            // �R���[�`���������Ă���ꍇ
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
            }

            // �G���A���ɗ��܂��Ă���Ƃ���SE���~�߂�
            spotSE[0].Stop();
            gameManager.GameOverFlgSet(false);
            gameManager.StaySpotArea = false;
            spotJudge = false;
        }
    }

    IEnumerator TimeCount()
    {
        counter = countTime;

        // 5�b�o�ߌ�ɃN���A������s��
        while (counter > 0)
        {
            yield return new WaitForSeconds(1.0f);
            //Debug.Log("Enter Collider Capsule");
            counter--;
        }

        spotSE[0].Stop();
        spotSE[1].Play();

        // GameManager�ɃN���A�񍐂�n��
        // SendMessage�͕����I�u�W�F�N�g�œ����X�N���v�g���g�p���Ă���ꍇ�ɖ𗧂�
        gameManager.GameClearFlgSet(true);
        unityChan.unitySuccess();
    }
}
