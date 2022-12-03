using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    // 障害物のY座標/Z座標とステージからはみ出させないオフセット
    const float OBSTACLE_DROP_HIGHT = 5.0f;
    const float OBSTACLE_DROP_Z = 12.5f;

    const float OBSTACLE_POS_OFFSET = 2.0f;

    // 障害物を生成するインターバル
    const float CREATE_TIME = 2.0f;

    [SerializeField]
    // 前方から飛んでくる障害物
    private GameObject _frontObstacle;

    // 障害物リスト
    [SerializeField]
    private GameObject[] _obstacles;

    // 障害物の最大数(Easy/Normal)
    // Hardは2倍にする
    [SerializeField]
    private int _obstacleMax = 10;

    private int _obstacleNum;
    private GameManager _gameManager;
    private Vector3 _vector;
    private float _obsRangeX;
    private float _obsRangeZ;
    private float _obsDropRangeZ;
    private int _createCount;
    private float _elapsedTime;
    private float _counter;
    private Coroutine _createCoroutine;
    private StageManager _stageManager;
    private List<GameObject> _objList;

    private void Start()
    {
        // GameManagerインスタンス取得
        _gameManager = GameManager.Instance;
        _stageManager = StageManager.Instance;

        // 選択しているステージがHardの場合
        if ((int)StageManager.StageLevel.HARD == _stageManager.SelectStageLevel)
        {
            _obstacleNum = _obstacleMax * 3;
            _obsDropRangeZ = OBSTACLE_DROP_Z * 1.75f;
        }
        else
        {
            _obstacleNum = _obstacleMax;
            _obsDropRangeZ = OBSTACLE_DROP_Z;
        }

        // 障害物を生成するための範囲を取得
        _obsRangeX = (_stageManager.StageScaleX / 2) - OBSTACLE_POS_OFFSET;
        _obsRangeZ = (_stageManager.StageScaleZ / 2) - OBSTACLE_POS_OFFSET;

        _objList = new List<GameObject>();
    }

    private void Update()
    {
        if (false == _gameManager.GameClearFlg)
        {
            // ステージ上に障害物を作成
            CreateObstacle();
            CreateFrontObstacle();
        }
        else
        {
            // Listに格納したオブジェクトはすべて削除
            // クリア後に障害物に当たってゲームオーバーとなるのを防ぐため
            foreach (GameObject gameObj in _objList)
            {
                Destroy(gameObj);
            }
        }
    }
    
    // ステージ上にランダムで障害物を作成する
    private void CreateObstacle()
    {
        // すでに障害物が最大数分作り出されている場合は終了
        if (_obstacleNum <= _createCount)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;

        // 2秒おきに障害物を増やす
        if (_elapsedTime > CREATE_TIME)
        {
            CreateRandomObstacle();
            _elapsedTime = 0.0f;
        }
    }

    private void CreateFrontObstacle()
    {
        _counter += Time.deltaTime;

        // 4秒おきに障害物を増やす
        if (_counter > CREATE_TIME)
        {
            // ステージのxスケールが10のため、x軸は-5~5の範囲で生成する 他は調整する
            GameObject frontObj = Instantiate(_frontObstacle, new Vector3(Random.Range(-_obsRangeX, _obsRangeX), OBSTACLE_DROP_HIGHT, _obsDropRangeZ), Quaternion.identity);

            /* Listにオブジェクト保持して、一定時間経過後に解放もあり */
            _objList.Add(frontObj);

            _counter = 0.0f;
        }
    }
    
    // ランダムに障害物を作り出す
    private void CreateRandomObstacle()
    {
        // ランダムの障害物のindex取得
        var index = Random.Range(0, _obstacles.Length);

        // 生成した障害物が最大数を超えていない場合
        if (_obstacleNum >= _createCount)
        {
            // 障害物を自動生成
            GameObject obj = Instantiate(_obstacles[index], GetRandomPosition(index), Quaternion.identity);

            // 生成した障害物をカウント
            _createCount++;
        }
    }

    // 現在のステージのスケール情報を基に、障害物の生成場所を決める
    private Vector3 GetRandomPosition(int index)
    {
        return new Vector3(Random.Range((-1) * _obsRangeX, _obsRangeX),
                                        OBSTACLE_DROP_HIGHT,
                                        Random.Range((-1) * _obsRangeZ, _obsRangeZ));
    }
}
