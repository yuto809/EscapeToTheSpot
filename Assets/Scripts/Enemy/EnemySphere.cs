using UnityEngine;

public class EnemySphere : MonoBehaviour
{
    // unityChanをアタッチ
    [SerializeField]
    private GameObject _target;

    // 速度デフォルトは20.0f
    [SerializeField]
    private float _speed;

    private float _enemySpeed;
    private Rigidbody _rigid;
    private GameManager _gameManager;
    private GameObject _enemy;
    void Start()
    {
        _enemySpeed = _speed;// Random.Range(1, speed);
        _rigid = GetComponent<Rigidbody>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // FixedUpdateは一定秒数ごとに呼ばれる
    void FixedUpdate()
    {
        if (_target == null)
        {
            return;
        }

        if (_gameManager.GameClearFlg)
        {
            Destroy(this.gameObject);
            return;
        }

        // AddForceは、物体に力を加えて動かす機能
        // 質量を考慮して継続的に力を加える
        // 向きたいターゲット位置から自分の位置を引くことで、ターゲットに向かうベクトルが求められる
        // 向きさえ知りたいから、単位ベクトルに直して、速度をかける
        _rigid.AddForce((_target.transform.position - transform.position).normalized * _enemySpeed);
    }
}
