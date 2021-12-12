using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    //ステージレベル
    enum StageLevel
    {
        EASY = 0,
        NORMAL,
        HARD
    }

    // 障害物のY座標/Z座標とステージからはみ出させないオフセット
    const float obstaclDropHight = 5.0f;
    const float obstaclDropZ = 12.5f;

    const float obsPosOffset = 2.0f;

    // 障害物を生成するインターバル
    const float createTime = 2.0f;

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

    private int _obstacleNum = 0;
    private GameManager _gameManager;
    private Vector3 _vector;
    private float _obsRangeX = 0.0f;
    private float _obsRangeZ = 0.0f;
    private float _obsDropRangeZ = 0.0f;
    private int _createCount = 0;
    private float _elapsedTime = 0.0f;
    private float _counter = 0.0f;
    private Coroutine _createCoroutine;
    private StageManager _stageManager;
    private List<GameObject> _objList;

    void Start()
    {
        // GameManagerインスタンス取得
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        // 選択しているステージがHardの場合
        if ((int)StageLevel.HARD == _stageManager.SelectStageLevel)
        {
            _obstacleNum = _obstacleMax * 3;
            _obsDropRangeZ = obstaclDropZ * 1.75f;
        }
        else
        {
            _obstacleNum = _obstacleMax;
            _obsDropRangeZ = obstaclDropZ;
        }

        // 障害物を生成するための範囲を取得
        _obsRangeX = (_stageManager.StageScale_x / 2) - obsPosOffset;
        _obsRangeZ = (_stageManager.StageScale_z / 2) - obsPosOffset;

        _objList = new List<GameObject>();
    }

    void Update()
    {
        if (false == _gameManager.GameClearFlg)
        {
            // ステージ上に障害物を作成
            CreateObstacle();
            CreateFrontObstacle();
        }
        else
        {
            Debug.Log(_objList.Count);

            // 前方からの障害物が1つでも存在する場合
            if (0 != _objList.Count)
            {
                // Listに格納したオブジェクトはすべて削除
                // クリア後に障害物に当たってゲームオーバーとなるのを防ぐため
                foreach (GameObject gameObj in _objList)
                {
                    Destroy(gameObj);
                }
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
        if (_elapsedTime > createTime)
        {
            CreateRandomObstacle();
            _elapsedTime = 0.0f;
        }
    }

    private void CreateFrontObstacle()
    {
        _counter += Time.deltaTime;

        // 4秒おきに障害物を増やす
        if (_counter > createTime)
        {
            // ステージのxスケールが10のため、x軸は-5~5の範囲で生成する 他は調整する
            GameObject frontOnj = Instantiate(_frontObstacle, new Vector3(Random.Range(-_obsRangeX, _obsRangeX), obstaclDropHight, _obsDropRangeZ), Quaternion.identity);

            /* Listにオブジェクト保持して、一定時間経過後に解放もあり */
            _objList.Add(frontOnj);

            _counter = 0.0f;
        }
    }
    
    // ランダムに障害物を作り出す
    private void CreateRandomObstacle()
    {
        // ランダムの障害物のindex取得
        int index = Random.Range(0, _obstacles.Length);

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
        //// 障害物を生成するための範囲を取得
        //obsRange_X = (stageManager.StageScale_x / 2) - obsPosOffset;
        //obsRange_Z = (stageManager.StageScale_z / 2) - obsPosOffset;

        // 障害物の座標を決める
        _obstacles[index].transform.position = new Vector3(Random.Range((-1) * _obsRangeX, _obsRangeX),
                                                          obstaclDropHight,
                                                          Random.Range((-1) * _obsRangeZ, _obsRangeZ));

        return _obstacles[index].transform.position;
    }
}
