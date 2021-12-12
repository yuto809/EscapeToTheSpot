using UnityEngine;

public class SetActive : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _activeEnemy;

    private StageManager _stageManager;
    private GameManager _gameManager;
    const float _enemyPosOffset = 2.0f;
    private float _enemyRangeX = 0.0f;
    private float _enemyRangeZ = 0.0f;
    
    void Start()
    {
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetActiveObject();
        SetEnemyPosition();
    }

    void SetEnemyPosition()
    {
        for (int i = 0; i < _activeEnemy.Length; i++)
        {
            // 障害物を生成するための範囲を取得
            _enemyRangeX = (_stageManager.StageScale_x / 2) - _enemyPosOffset;
            _enemyRangeZ = (_stageManager.StageScale_z / 2) - _enemyPosOffset;

            // 障害物の座標を決める
            _activeEnemy[i].transform.position = new Vector3(Random.Range(-_enemyRangeX, _enemyRangeX),
                                                            1.0f,
                                                            Random.Range(-_enemyRangeZ, _enemyRangeZ));
        }
    }

    void SetActiveObject()
    {
        // Normalの場合は、球体のみを敵として扱う
        if (1 == _stageManager.SelectStageLevel)
        {
            _activeEnemy[0].SetActive(true);
            _activeEnemy[1].SetActive(false);
        }
        // Hardの場合は、球体とGhostを敵として扱う
        else if (2 == _stageManager.SelectStageLevel)
        {
            _activeEnemy[0].SetActive(true);
            _activeEnemy[1].SetActive(true);
        }
        // Easyの場合は、敵は不要(障害物のみ)
        else
        {
            _activeEnemy[0].SetActive(false);
            _activeEnemy[1].SetActive(false);
        }
    }
}
