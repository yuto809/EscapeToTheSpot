using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGhost : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private NavMeshAgent myAgent;
    private GameManager gameManager;

    [SerializeField]
    private GameObject unityChan;

    [SerializeField]
    private UnityChanController unityChanController;

    //　キャラクターの速度
    private Vector3 velocity;
    //　キャラクターの歩くスピード
    [SerializeField]
    private float walkSpeed = 2.0f;

    [SerializeField]
    private float runSpeed = 4.0f;

    private Vector3 vector3;

    void Start()
    {
        // コンポーネントの取得を行う
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (unityChan == null)
        {
            velocity = Vector3.zero;
            return;
        }

        if (gameManager.GameClearFlg)
        {
            Destroy(this.gameObject);
            return;
        }

        // 地面と接地している場合
        if (characterController.isGrounded)
        {
            // 速度の初期化
            velocity = Vector3.zero;
            vector3 = (unityChan.transform.position - transform.position);

            // 3次元ベクトルの長さ(x^2 + y^2 + z^2 のルート)
            if (vector3.magnitude > 0.1f)
            {
                // 向きを変える
                transform.rotation = Quaternion.LookRotation(vector3.normalized);

                // unityChanがスポットエリア内に留まった場合は速度を上げる
                // ステージレベルHardのみの仕様
                if (gameManager.StaySpotArea)
                {
                    animator.SetBool("Run", true);
                    animator.SetBool("Walk", false);

                    velocity += transform.forward * runSpeed;
                }
                else
                {
                    // アニメータ起動
                    animator.SetBool("Run", false);
                    animator.SetBool("Walk", true);

                    velocity += transform.forward * walkSpeed;
                }
            }
        }

        //myAgent.SetDestination(unityChan.transform.position);
        // Physics.gravity.yはプロジェクト設定より-9.81
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "unityChan")
        {
            if (false == gameManager.GameClearFlg)
            { 
                unityChanController.unityDead();
            }

            Destroy(this.gameObject);
        }
    }
}
