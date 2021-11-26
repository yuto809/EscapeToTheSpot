using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySphere : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float speed;

    private float enemySpeed;
    private Rigidbody rigid;
    private GameManager gameManager;

    void Start()
    {
        enemySpeed = speed;// Random.Range(1, speed);
        rigid = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // FixedUpdate�͈��b�����ƂɌĂ΂��
    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (gameManager.GameClearFlg)
        {
            Destroy(this.gameObject);
            return;
        }

        // AddForce�́A���̂ɗ͂������ē������@�\
        // ���ʂ��l�����Čp���I�ɗ͂�������
        // ���������^�[�Q�b�g�ʒu���玩���̈ʒu���������ƂŁA�^�[�Q�b�g�Ɍ������x�N�g�������߂���
        // ���������m�肽������A�P�ʃx�N�g���ɒ����āA���x��������
        rigid.AddForce((target.transform.position - transform.position).normalized * enemySpeed);
    }
}
