using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityChanController : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private CharacterController _characterController;
    private UnityEvent _setGameOverFlgEvent;

    private Animator _animator;
    private AudioClip _audioClip;
    //　キャラクターの速度
    private Vector3 _velocity;
    //　キャラクターの歩くスピード
    [SerializeField]
    private float _walkSpeed = 2f;
    //　キャラクターの走るスピード
    [SerializeField]
    private float _runSpeed = 4f;
    [SerializeField]
    private float _jumpSpeed = 2f;

    private Vector3 _input;

    // 入力方向フラグ
    private bool _leftDirection = false;
    private bool _rightDirection = false;
    private bool _upDirection = false;
    private bool _downDirection = false;

    private bool _deadFlg;
    private bool _successFlg;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _deadFlg = false;
        _successFlg = false;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Ghostに接触したらアウト
        // 接触が甘いのか判断できないことがあったため
        // Ghost側のColliderでも判断している
        if (hit.gameObject.tag == "ghost")
        {
            UnityDead();
        }
    }

    // 前方からの障害物や、球体と衝突した場合
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            UnityDead();
        }
    }

    // ボタン処理
    public void PushLeft()
    {
        _leftDirection = true;
    }

    public void PushRight()
    {
        _rightDirection = true;
    }

    public void PushUp()
    {
        _upDirection = true;
    }

    public void PushDown()
    {
        _downDirection = true;
    }

    public void LeaveLeft()
    {
        _leftDirection = false;
    }

    public void LeaveRight()
    {
        _rightDirection = false;
    }

    public void LeaveUp()
    {
        _upDirection = false;
    }

    public void LeaveDown()
    {
        _downDirection = false;
    }

    public void UnityDead()
    {
        // unityChanが生きている場合
        if (false == _deadFlg)
        {
            _deadFlg = true;
            _animator.SetBool("Dead", true);

            // 障害物に当たったらSEを流す
            _audioManager.PlayMusicSE((int)AudioManager.PlaySE.DAMAGE_UNITYCHAN);

            _animator.SetTrigger("Damage");
            _animator.SetTrigger("KneelDown");

            // ゆにぃいい・・・(負け)
            _audioManager.PlayMusicVoice((int)AudioManager.PlayVoice.VOICE_DEAD);
            _gameManager.CallGameOverFlgEvent();
        }
    }

    // SpotAreaから呼ばれる
    public void UnitySuccess()
    {
        //Debug.Log("ゲームクリアフラグ　：" + gameManager.GameClearFlg);
        if (false == _successFlg)
        {
            if (_gameManager.GameClearFlg)
            {
                _successFlg = true;
                _animator.SetTrigger("Success");
                _audioManager.PlayMusicVoice((int)AudioManager.PlayVoice.VOICE_CLEAR);
            }
        }
    }

    // 入力された方向に座標を変更する
    public void ToUp()
    {
        if (_upDirection)
        {
            _input = new Vector3(0, 0, 1.0f);
        }
    }

    public void ToRight()
    {
        if (_rightDirection)
        {
            _input = new Vector3(1.0f, 0, 0);
        }
    }

    public void ToDown()
    {
        if (_downDirection)
        {
            _input = new Vector3(0, 0, -1.0f);
        }
    }

    public void ToLeft()
    {
        if (_leftDirection)
        {
            _input = new Vector3(-1.0f, 0, 0);
        }
    }

    private void Update()
    {
        // unityChanが死んだ、もしくはゲームクリアの場合
        if (_deadFlg || _successFlg)
        {
            _animator.SetFloat("Speed", 0f);
            return;
        }

        // 地面と接地している場合
        if (_characterController.isGrounded)
        {
            // 速度の初期化
            _velocity = Vector3.zero;

            // ワールド座標(グローバル座標)
            // x軸に左右キーの値、z軸に縦下キーの値を設定する
            _input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // 移動
            ToUp();
            ToRight();
            ToDown();
            ToLeft();

            // 3次元ベクトルの長さ(x^2 + y^2 + z^2 のルート)
            if (_input.magnitude > 0.1f)
            {
                // ワールド座標を見て向きを変えている
                transform.rotation = Quaternion.LookRotation(_input);

                // 現在の位置に入力した方向を足して、その方向を向かせる
                //transform.LookAt(transform.position + input.normalized);

                // アニメータ起動
                _animator.SetFloat("Speed", _input.magnitude);

                if (_input.magnitude > 0.5f)
                {
                    // ★
                    // FPSとかに向いているやり方
                    // ベクトルをローカル座標からグローバル座標へ変換
                    // ワールド座標を入れたとしても、システム側はローカル座標と認識する
                    // TransformDirectionは自分のローカル座標をワールド座標として扱う
                    //float mX = Input.GetAxis("Mouse X");      //マウスの左右移動量(-1.0~1.0)
                    //input = transform.TransformDirection(input);
                    //gameObject.transform.Rotate(new Vector3(0, 30.0f * mX, 0));

                    // transform.forwardはオブジェクトが向いている方向のベクトル(z軸)
                    // 速度はどの向きにどれぐらい動くか (velocity += transform.forward * runSpeed;)でも可能
                    _velocity += _input * _runSpeed;
                }
                else
                {
                    _velocity += _input * _walkSpeed;
                }
            }
            else
            {
                _animator.SetFloat("Speed", 0f);
            }
        }

        // ジャンプ機能はなしにする
        //if (Input.GetButtonDown("Jump"))
        //{
        //    velocity.y = jumpSpeed;
        //    animator.SetTrigger("Jump");
        //}

        // CharactorControllerは重力を考慮する必要がある
        _velocity.y += Physics.gravity.y * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
