using System.Collections;
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
    private GameObject frontObstacle;

    // 障害物リスト
    [SerializeField]
    private GameObject[] obstacles;

    // 障害物の最大数(Easy/Normal)
    // Hardは2倍にする
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
        // GameManagerインスタンス取得
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        // 選択しているステージがHardの場合
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

        // 障害物を生成するための範囲を取得
        obsRange_X = (stageManager.StageScale_x / 2) - obsPosOffset;
        obsRange_Z = (stageManager.StageScale_z / 2) - obsPosOffset;

        objList = new List<GameObject>();
    }

    void Update()
    {
        if (false == gameManager.GameClearFlg)
        {
            // ステージ上に障害物を作成
            CreateObstacle();
            CreateFrontObstacle();
        }
        else
        {
            Debug.Log(objList.Count);

            // 前方からの障害物が1つでも存在する場合
            if (0 != objList.Count)
            {
                // Listに格納したオブジェクトはすべて削除
                // クリア後に障害物に当たってゲームオーバーとなるのを防ぐため
                foreach (GameObject gameObj in objList)
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
        if (obstacleNum <= createCount)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        // 2秒おきに障害物を増やす
        if (elapsedTime > createTime)
        {
            CreateRandomObstacle();
            elapsedTime = 0.0f;
        }
    }

    private void CreateFrontObstacle()
    {
        counter += Time.deltaTime;

        // 4秒おきに障害物を増やす
        if (counter > createTime)
        {
            // ステージのxスケールが10のため、x軸は-5~5の範囲で生成する 他は調整する
            GameObject frontOnj = Instantiate(frontObstacle, new Vector3(Random.Range(-obsRange_X, obsRange_X), obstaclDropHight, obsDropRange_Z), Quaternion.identity);

            /* Listにオブジェクト保持して、一定時間経過後に解放もあり */
            objList.Add(frontOnj);

            counter = 0.0f;
        }
    }
    
    // ランダムに障害物を作り出す
    private void CreateRandomObstacle()
    {
        // ランダムの障害物のindex取得
        int index = Random.Range(0, obstacles.Length);

        // 生成した障害物が最大数を超えていない場合
        if (obstacleNum >= createCount)
        {
            // 障害物を自動生成
            GameObject obj = Instantiate(obstacles[index], GetRandomPosition(index), Quaternion.identity);

            // 生成した障害物をカウント
            createCount++;
        }
    }

    // 現在のステージのスケール情報を基に、障害物の生成場所を決める
    private Vector3 GetRandomPosition(int index)
    {
        //// 障害物を生成するための範囲を取得
        //obsRange_X = (stageManager.StageScale_x / 2) - obsPosOffset;
        //obsRange_Z = (stageManager.StageScale_z / 2) - obsPosOffset;

        // 障害物の座標を決める
        obstacles[index].transform.position = new Vector3(Random.Range((-1) * obsRange_X, obsRange_X),
                                                          obstaclDropHight,
                                                          Random.Range((-1) * obsRange_Z, obsRange_Z));

        return obstacles[index].transform.position;
    }
}
