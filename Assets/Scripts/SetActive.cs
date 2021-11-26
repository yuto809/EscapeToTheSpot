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
            // 障害物を生成するための範囲を取得
            enemyRange_X = (stageManager.StageScale_x / 2) - enemyPosOffset;
            enemyRange_Z = (stageManager.StageScale_z / 2) - enemyPosOffset;
            
            // 障害物の座標を決める
            activeEnemy[i].transform.position = new Vector3(Random.Range(-enemyRange_X, enemyRange_X),
                                                            1.0f,
                                                            Random.Range(-enemyRange_Z, enemyRange_Z));
        }
    }

    void SetActiveObject()
    {
        // Normalの場合は、球体のみを敵として扱う
        if (1 == stageManager.SelectStageLevel)
        {
            activeEnemy[0].SetActive(true);
            activeEnemy[1].SetActive(false);
        }
        // Hardの場合は、球体とGhostを敵として扱う
        else if (2 == stageManager.SelectStageLevel)
        {
            activeEnemy[0].SetActive(true);
            activeEnemy[1].SetActive(true);
        }
        // Easyの場合は、敵は不要(障害物のみ)
        else
        {
            activeEnemy[0].SetActive(false);
            activeEnemy[1].SetActive(false);
        }
    }
}
