using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private CharacterController characterController;

    // [0]はdead
    // [1]はclear
    private AudioSource []unitySE;
    private Animator animator;
    AudioClip audioClip;
    //　キャラクターの速度
    private Vector3 velocity;
    //　キャラクターの歩くスピード
    [SerializeField]
    private float walkSpeed = 2f;
    //　キャラクターの走るスピード
    [SerializeField]
    private float runSpeed = 4f;
    [SerializeField]
    private float jumpSpeed = 2f;

    Vector3 input;

    // 入力方向フラグ
    bool leftDirection = false;
    bool rightDirection = false;
    bool upDirection = false;
    bool downDirection = false;

    private bool deadFlg;
    private bool successFlg;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        // AudioManagerのゲームオブジェクトを探す
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        unitySE = GetComponents<AudioSource>();
        // コンポーネントの取得を行う
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        deadFlg = false;
        successFlg = false;
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
        if ((other.gameObject.tag == "enemy"))
        {
            unityDead();
        }
    }

    // ボタン処理
    public void pushLeft()
    {
        leftDirection = true;
    }

    public void pushRight()
    {
        rightDirection = true;
    }

    public void pushUp()
    {
        upDirection = true;
    }

    public void pushDown()
    {
        downDirection = true;
    }

    public void leaveLeft()
    {
        leftDirection = false;
    }

    public void leaveRight()
    {
        rightDirection = false;
    }

    public void leaveUp()
    {
        upDirection = false;
    }

    public void leaveDown()
    {
        downDirection = false;
    }

    public void unityDead()
    {
        // unityChanが生きている場合
        if (false == deadFlg)
        {
            deadFlg = true;
            animator.SetBool("Dead", true);

            // 障害物に当たったらSEを流す
            audioClip = Resources.Load("SE/Damage/Damage") as AudioClip;
            audioManager.DamageSE(audioClip);

            animator.SetTrigger("Damage");
            animator.SetTrigger("KneelDown");

            // ゆにぃいい・・・(負け)
            unitySE[0].Play();
            gameManager.GameOverFlgSet(true);
        }
    }

    // SpotAreaから呼ばれる
    public void unitySuccess()
    {
        //Debug.Log("ゲームクリアフラグ　：" + gameManager.GameClearFlg);
        if (false == successFlg)
        {
            if (gameManager.GameClearFlg)
            {
                //Debug.Log("SUCCESS");
                successFlg = true;
                animator.SetTrigger("Success");
                unitySE[1].Play();
                return;
            }
        }
    }

    // 入力された方向に座標を変更する
    public void ToUp()
    {
        if (upDirection)
        {
            input = new Vector3(0, 0, 1.0f);
        }
    }

    public void ToRight()
    {
        if (rightDirection)
        {
            input = new Vector3(1.0f, 0, 0);
        }
    }

    public void ToDown()
    {
        if (downDirection)
        {
            input = new Vector3(0, 0, -1.0f);
        }
    }

    public void ToLeft()
    {
        if (leftDirection)
        {
            input = new Vector3(-1.0f, 0, 0);
        }
    }

    void Update()
    {
        // unityChanが死んだ、もしくはゲームクリアの場合
        if ((true == deadFlg) || (true == successFlg))
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        // 地面と接地している場合
        if (characterController.isGrounded)
        {
            // 速度の初期化
            velocity = Vector3.zero;

            // ワールド座標(グローバル座標)
            // x軸に左右キーの値、z軸に縦下キーの値を設定する
            input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // 移動
            ToUp();
            ToRight();
            ToDown();
            ToLeft();

            // 3次元ベクトルの長さ(x^2 + y^2 + z^2 のルート)
            if (input.magnitude > 0.1f)
            {
                // ワールド座標を見て向きを変えている
                transform.rotation = Quaternion.LookRotation(input);

                // 現在の位置に入力した方向を足して、その方向を向かせる
                //transform.LookAt(transform.position + input.normalized);

                // アニメータ起動
                animator.SetFloat("Speed", input.magnitude);

                if (input.magnitude > 0.5f)
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
                    velocity += input * runSpeed;
                }
                else
                {
                    velocity += input * walkSpeed;
                }
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }
        }

        // ジャンプ機能はなしにする
        //if (Input.GetButtonDown("Jump"))
        //{
        //    velocity.y = jumpSpeed;
        //    animator.SetTrigger("Jump");
        //}

        // CharactorControllerは重力を考慮する必要がある
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
