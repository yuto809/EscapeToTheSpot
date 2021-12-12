using UnityEngine;
using UnityEngine.AI;

public class EnemyGhost : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _animator;
    private NavMeshAgent _myAgent;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _unityChan;

    [SerializeField]
    private UnityChanController _unityChanController;

    //　キャラクターの速度
    private Vector3 _velocity;
    //　キャラクターの歩くスピード
    [SerializeField]
    private float _walkSpeed = 2.0f;

    [SerializeField]
    private float _runSpeed = 4.0f;

    private Vector3 _vector3;

    void Start()
    {
        // コンポーネントの取得を行う
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _myAgent = GetComponent<NavMeshAgent>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (_unityChan == null)
        {
            _velocity = Vector3.zero;
            return;
        }

        if (_gameManager.GameClearFlg)
        {
            Destroy(this.gameObject);
            return;
        }

        // 地面と接地している場合
        if (_characterController.isGrounded)
        {
            // 速度の初期化
            _velocity = Vector3.zero;
            _vector3 = (_unityChan.transform.position - transform.position);

            // 3次元ベクトルの長さ(x^2 + y^2 + z^2 のルート)
            if (_vector3.magnitude > 0.1f)
            {
                // 向きを変える
                transform.rotation = Quaternion.LookRotation(_vector3.normalized);

                // unityChanがスポットエリア内に留まった場合は速度を上げる
                // ステージレベルHardのみの仕様
                if (_gameManager.StaySpotArea)
                {
                    _animator.SetBool("Run", true);
                    _animator.SetBool("Walk", false);

                    _velocity += transform.forward * _runSpeed;
                }
                else
                {
                    // アニメータ起動
                    _animator.SetBool("Run", false);
                    _animator.SetBool("Walk", true);

                    _velocity += transform.forward * _walkSpeed;
                }
            }
        }

        //myAgent.SetDestination(unityChan.transform.position);
        // Physics.gravity.yはプロジェクト設定より-9.81
        _velocity.y += Physics.gravity.y * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "unityChan")
        {
            if (false == _gameManager.GameClearFlg)
            {
                _unityChanController.unityDead();
            }

            Destroy(this.gameObject);
        }
    }
}
