using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    [SerializeField]
    private GameObject[] activeEnemy;

    private StageManager stageManager;
    private GameManager gameManager;
    const float enemyPosOffset = 2.0f;
    private float enemyRange_X = 0.0f;
    private float enemyRange_Z = 0.0f;
    
    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetActiveObject();
        SetEnemyPosition();
    }

    void SetEnemyPosition()
    {
        for (int i = 0; i < activeEnemy.Length; i++)
        {
            // ��Q���𐶐����邽�߂͈̔͂��擾
            enemyRange_X = (stageManager.StageScale_x / 2) - enemyPosOffset;
            enemyRange_Z = (stageManager.StageScale_z / 2) - enemyPosOffset;
            
            // ��Q���̍��W�����߂�
            activeEnemy[i].transform.position = new Vector3(Random.Range(-enemyRange_X, enemyRange_X),
                                                            1.0f,
                                                            Random.Range(-enemyRange_Z, enemyRange_Z));
        }
    }

    void SetActiveObject()
    {
        // Normal�̏ꍇ�́A���݂̂̂�G�Ƃ��Ĉ���
        if (1 == stageManager.SelectStageLevel)
        {
            activeEnemy[0].SetActive(true);
            activeEnemy[1].SetActive(false);
        }
        // Hard�̏ꍇ�́A���̂�Ghost��G�Ƃ��Ĉ���
        else if (2 == stageManager.SelectStageLevel)
        {
            activeEnemy[0].SetActive(true);
            activeEnemy[1].SetActive(true);
        }
        // Easy�̏ꍇ�́A�G�͕s�v(��Q���̂�)
        else
        {
            activeEnemy[0].SetActive(false);
            activeEnemy[1].SetActive(false);
        }
    }
}
