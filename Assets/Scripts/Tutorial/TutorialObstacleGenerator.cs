using UnityEngine;

public class TutorialObstacleGenerator : MonoBehaviour
{
    const float POS_OFFSET = 0.1f;
    const float DROP_OBSTACLE_OFFSET_Y = 10.0f;

    // 障害物リスト
    [SerializeField]
    private GameObject _obstacle;

    [SerializeField]
    private TutorialUnityChanController _unityChan;

    private StageManager _stageManager;
    private GameManager _gameManager;
    private float _unityChanPosX;
    private float _unityChanPosY;
    private float _unityChanPosZ;
    private bool _isCalled;

    private void Start()
    {
        // GameManagerインスタンス取得
        _gameManager = GameManager.Instance;
        _unityChan = GameObject.Find("TutorialUnityChan").GetComponent<TutorialUnityChanController>();

        _unityChanPosX = _unityChan.transform.position.x;
        _unityChanPosY = _unityChan.transform.position.y;
        _unityChanPosZ = _unityChan.transform.position.z;
    }

    private void Update()
    {
        _unityChanPosX = _unityChan.transform.position.x;
        _unityChanPosY = _unityChan.transform.position.y;
        _unityChanPosZ = _unityChan.transform.position.z;
    }

    // ステージ上にランダムで障害物を作成する
    public void CreateTutorialObstacle()
    {
        // 障害物を自動生成
        GameObject obj = Instantiate(_obstacle,
                                     new Vector3(_unityChanPosX - POS_OFFSET, _unityChanPosY + DROP_OBSTACLE_OFFSET_Y, _unityChanPosZ - POS_OFFSET),
                                     Quaternion.identity);
        _isCalled = true;
    }
}
