using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private CharacterController characterController;

    // [0]��dead
    // [1]��clear
    private AudioSource []unitySE;
    private Animator animator;
    AudioClip audioClip;
    //�@�L�����N�^�[�̑��x
    private Vector3 velocity;
    //�@�L�����N�^�[�̕����X�s�[�h
    [SerializeField]
    private float walkSpeed = 2f;
    //�@�L�����N�^�[�̑���X�s�[�h
    [SerializeField]
    private float runSpeed = 4f;
    [SerializeField]
    private float jumpSpeed = 2f;

    Vector3 input;

    // ���͕����t���O
    bool leftDirection = false;
    bool rightDirection = false;
    bool upDirection = false;
    bool downDirection = false;

    private bool deadFlg;
    private bool successFlg;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        // AudioManager�̃Q�[���I�u�W�F�N�g��T��
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        unitySE = GetComponents<AudioSource>();
        // �R���|�[�l���g�̎擾���s��
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        deadFlg = false;
        successFlg = false;
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Ghost�ɐڐG������A�E�g
        // �ڐG���Â��̂����f�ł��Ȃ����Ƃ�����������
        // Ghost����Collider�ł����f���Ă���
        if (hit.gameObject.tag == "ghost")
        {
            unityDead();
        }
    }

    // �O������̏�Q����A���̂ƏՓ˂����ꍇ
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "enemy"))
        {
            unityDead();
        }
    }

    // �{�^������
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
        // unityChan�������Ă���ꍇ
        if (false == deadFlg)
        {
            deadFlg = true;
            animator.SetBool("Dead", true);

            // ��Q���ɓ���������SE�𗬂�
            audioClip = Resources.Load("SE/Damage/Damage") as AudioClip;
            audioManager.DamageSE(audioClip);

            animator.SetTrigger("Damage");
            animator.SetTrigger("KneelDown");

            // ��ɂ������E�E�E(����)
            unitySE[0].Play();
            gameManager.GameOverFlgSet(true);
        }
    }

    // SpotArea����Ă΂��
    public void unitySuccess()
    {
        //Debug.Log("�Q�[���N���A�t���O�@�F" + gameManager.GameClearFlg);
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

    // ���͂��ꂽ�����ɍ��W��ύX����
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
        // unityChan�����񂾁A�������̓Q�[���N���A�̏ꍇ
        if ((true == deadFlg) || (true == successFlg))
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        // �n�ʂƐڒn���Ă���ꍇ
        if (characterController.isGrounded)
        {
            // ���x�̏�����
            velocity = Vector3.zero;

            // ���[���h���W(�O���[�o�����W)
            // x���ɍ��E�L�[�̒l�Az���ɏc���L�[�̒l��ݒ肷��
            input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // �ړ�
            ToUp();
            ToRight();
            ToDown();
            ToLeft();

            // 3�����x�N�g���̒���(x^2 + y^2 + z^2 �̃��[�g)
            if (input.magnitude > 0.1f)
            {
                // ���[���h���W�����Č�����ς��Ă���
                transform.rotation = Quaternion.LookRotation(input);

                // ���݂̈ʒu�ɓ��͂��������𑫂��āA���̕�������������
                //transform.LookAt(transform.position + input.normalized);

                // �A�j���[�^�N��
                animator.SetFloat("Speed", input.magnitude);

                if (input.magnitude > 0.5f)
                {
                    // ��
                    // FPS�Ƃ��Ɍ����Ă������
                    // �x�N�g�������[�J�����W����O���[�o�����W�֕ϊ�
                    // ���[���h���W����ꂽ�Ƃ��Ă��A�V�X�e�����̓��[�J�����W�ƔF������
                    // TransformDirection�͎����̃��[�J�����W�����[���h���W�Ƃ��Ĉ���
                    //float mX = Input.GetAxis("Mouse X");      //�}�E�X�̍��E�ړ���(-1.0~1.0)
                    //input = transform.TransformDirection(input);
                    //gameObject.transform.Rotate(new Vector3(0, 30.0f * mX, 0));

                    // transform.forward�̓I�u�W�F�N�g�������Ă�������̃x�N�g��(z��)
                    // ���x�͂ǂ̌����ɂǂꂮ�炢������ (velocity += transform.forward * runSpeed;)�ł��\
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

        // �W�����v�@�\�͂Ȃ��ɂ���
        //if (Input.GetButtonDown("Jump"))
        //{
        //    velocity.y = jumpSpeed;
        //    animator.SetTrigger("Jump");
        //}

        // CharactorController�͏d�͂��l������K�v������
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
