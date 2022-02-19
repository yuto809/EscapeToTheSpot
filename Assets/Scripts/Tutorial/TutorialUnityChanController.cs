using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialUnityChanController : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private TutorialManager _tutorialManager;
    private CharacterController _characterController;
    private UnityEvent _setGameOverFlgEvent;

    // [0]はdead
    // [1]はclear
    //private AudioSource[] _unitySE;
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
        // TutorialManager取得
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
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
            unityDead();
        }
    }

    // 前方からの障害物や、球体と衝突した場合
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            unityDead();
        }
    }

    // ボタン処理
    public void pushLeft()
    {
        _leftDirection = true;
    }

    public void pushRight()
    {
        _rightDirection = true;
    }

    public void pushUp()
    {
        _upDirection = true;
    }

    public void pushDown()
    {
        _downDirection = true;
    }

    public void leaveLeft()
    {
        _leftDirection = false;
    }

    public void leaveRight()
    {
        _rightDirection = false;
    }

    public void leaveUp()
    {
        _upDirection = false;
    }

    public void leaveDown()
    {
        _downDirection = false;
    }

    public void unityDead()
    {
        // unityChanが生きている場合
        if (false == _deadFlg)
        {
            _deadFlg = true;
            _animator.SetBool("Dead", true);

            // 障害物に当たったらSEを流す
            //_audioClip = Resources.Load("SE/Damage/Damage") as AudioClip;
            //_audioManager.DamageSE(_audioClip);
            _audioManager.PlayMusicSE((int)AudioManager.PlaySE.DAMAGE_UNITYCHAN);

            _animator.SetTrigger("Damage");
            _animator.SetTrigger("KneelDown");

            // ゆにぃいい・・・(負け)
            //_unitySE[0].Play();
            _audioManager.PlayMusicVoice((int)AudioManager.PlayVoice.VOICE_DEAD);
            _gameManager.CallGameOverFlgEvent();
        }
    }

    // SpotAreaから呼ばれる
    public void unitySuccess()
    {
        //Debug.Log("ゲームクリアフラグ　：" + gameManager.GameClearFlg);
        if (false == _successFlg)
        {
            if (_gameManager.GameClearFlg)
            {
                //Debug.Log("SUCCESS");
                _successFlg = true;
                _animator.SetTrigger("Success");
                //_unitySE[1].Play();
                _audioManager.PlayMusicVoice((int)AudioManager.PlayVoice.VOICE_CLEAR);

            }
        }
    }

    // 入力された方向に座標を変更する
    // 速度を変更するやり方で移動
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

                // アニメータ起動
                _animator.SetFloat("Speed", _input.magnitude);

                if (_input.magnitude > 0.5f)
                {
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

        // CharactorControllerは重力を考慮する必要がある
        _velocity.y += Physics.gravity.y * Time.deltaTime;

        //// 移動チュートリアルが完了したら操作は受け付けないようにする
        //if (_tutorialManager.GetTutorialTaskCompStatus(((int)TutorialManager.TutorialTitle.TUTORIAL_MOVEMENT)))
        //{
        //    //Debug.Log("チュートリアル中　　移動");
        //}
        //else
        //{
        //    _characterController.Move(_velocity * Time.deltaTime);
        //}

        // 移動チュートリアルが完了したら操作は受け付けないようにする
        if (_tutorialManager.GetCurrentTutorialStatus == (int)TutorialManager.TutorialTitle.TUTORIAL_MOVEMENT)
        {
            if ((_tutorialManager.GetTutorialTaskCompStatus(((int)TutorialManager.TutorialTitle.TUTORIAL_MOVEMENT))))
            {

                //Debug.Log("チュートリアル中　　移動");
            }
            else
            {
                _characterController.Move(_velocity * Time.deltaTime);
            }
        }
        else if (_tutorialManager.GetCurrentTutorialStatus == (int)TutorialManager.TutorialTitle.TUTORIAL_GAME_CLEAR)
        {
            _characterController.Move(_velocity * Time.deltaTime);
        }
        else
        {
            _characterController.Move(_velocity * Time.deltaTime);
        }

    }

}
