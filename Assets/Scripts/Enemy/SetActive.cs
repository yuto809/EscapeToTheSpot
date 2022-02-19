using UnityEngine;

public class SetActive : MonoBehaviour
{
    //[SerializeField]
    //private GameObject[] _activeEnemy;

    [SerializeField]
    private GameObject _enemy;

    private StageManager _stageManager;

    const float _enemyPosOffset = 2.0f;
    private float _enemyRangeX = 0.0f;
    private float _enemyRangeZ = 0.0f;

    private void Start()
    {
        _stageManager = StageManager.Instance;

        //SetActiveObject();
        SetActiveEnemy();
        SetEnemyPosition();
    }

    private void SetEnemyPosition()
    {
        //for (var i = 0; i < _activeEnemy.Length; i++)
        //{
        //    // 障害物を生成するための範囲を取得
        //    _enemyRangeX = (_stageManager.StageScaleX / 2) - _enemyPosOffset;
        //    _enemyRangeZ = (_stageManager.StageScaleZ / 2) - _enemyPosOffset;

        //    // 障害物の座標を決める
        //    _activeEnemy[i].transform.position = new Vector3(Random.Range(-_enemyRangeX, _enemyRangeX),
        //                                                    1.0f,
        //                                                    Random.Range(-_enemyRangeZ, _enemyRangeZ));
        //}

        // 障害物を生成するための範囲を取得
        _enemyRangeX = (_stageManager.StageScaleX / 2) - _enemyPosOffset;
        _enemyRangeZ = (_stageManager.StageScaleZ / 2) - _enemyPosOffset;

        _enemy.transform.position = new Vector3(Random.Range(-_enemyRangeX, _enemyRangeX),
                                                1.0f,
                                                Random.Range(-_enemyRangeZ, _enemyRangeZ));
    }

    //void SetActiveObject()
    //{
    //    // Normalの場合は、球体のみを敵として扱う
    //    if (1 == _stageManager.SelectStageLevel)
    //    {
    //        _activeEnemy[0].SetActive(true);
    //        _activeEnemy[1].SetActive(false);
    //    }
    //    // Hardの場合は、球体とGhostを敵として扱う
    //    else if (2 == _stageManager.SelectStageLevel)
    //    {
    //        _activeEnemy[0].SetActive(true);
    //        _activeEnemy[1].SetActive(true);
    //    }
    //    // Easyの場合は、敵は不要(障害物のみ)
    //    else
    //    {
    //        _activeEnemy[0].SetActive(false);
    //        _activeEnemy[1].SetActive(false);
    //    }
    //}

    private void SetActiveEnemy()
    {
        // Normalの場合は、敵(球体)のみを表示
        if ((int)StageManager.StageLevel.NORMAL == _stageManager.SelectStageLevel)
        {
            if (_enemy.CompareTag("enemy"))
            {
                _enemy.SetActive(true);
            }
            else
            {
                _enemy.SetActive(false);
            }
        }
        // Hardの場合は、敵は全て表示させる
        else if ((int)StageManager.StageLevel.HARD == _stageManager.SelectStageLevel)
        {
            _enemy.SetActive(true);
        }
        // Easyの場合は、敵は不要(障害物のみ)
        else
        {
            _enemy.SetActive(false);
        }
    }
}
