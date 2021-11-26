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

    //�@�L�����N�^�[�̑��x
    private Vector3 velocity;
    //�@�L�����N�^�[�̕����X�s�[�h
    [SerializeField]
    private float walkSpeed = 2.0f;

    [SerializeField]
    private float runSpeed = 4.0f;

    private Vector3 vector3;

    void Start()
    {
        // �R���|�[�l���g�̎擾���s��
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

        // �n�ʂƐڒn���Ă���ꍇ
        if (characterController.isGrounded)
        {
            // ���x�̏�����
            velocity = Vector3.zero;
            vector3 = (unityChan.transform.position - transform.position);

            // 3�����x�N�g���̒���(x^2 + y^2 + z^2 �̃��[�g)
            if (vector3.magnitude > 0.1f)
            {
                // ������ς���
                transform.rotation = Quaternion.LookRotation(vector3.normalized);

                // unityChan���X�|�b�g�G���A���ɗ��܂����ꍇ�͑��x���グ��
                // �X�e�[�W���x��Hard�݂̂̎d�l
                if (gameManager.StaySpotArea)
                {
                    animator.SetBool("Run", true);
                    animator.SetBool("Walk", false);

                    velocity += transform.forward * runSpeed;
                }
                else
                {
                    // �A�j���[�^�N��
                    animator.SetBool("Run", false);
                    animator.SetBool("Walk", true);

                    velocity += transform.forward * walkSpeed;
                }
            }
        }

        //myAgent.SetDestination(unityChan.transform.position);
        // Physics.gravity.y�̓v���W�F�N�g�ݒ���-9.81
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
