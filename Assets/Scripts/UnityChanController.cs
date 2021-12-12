using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private CharacterController _characterController;

    // [0]��dead
    // [1]��clear
    private AudioSource[] _unitySE;
    private Animator _animator;
    AudioClip _audioClip;
    //�@�L�����N�^�[�̑��x
    private Vector3 _velocity;
    //�@�L�����N�^�[�̕����X�s�[�h
    [SerializeField]
    private float _walkSpeed = 2f;
    //�@�L�����N�^�[�̑���X�s�[�h
    [SerializeField]
    private float _runSpeed = 4f;
    [SerializeField]
    private float _jumpSpeed = 2f;

    Vector3 _input;

    // ���͕����t���O
    bool _leftDirection = false;
    bool _rightDirection = false;
    bool _upDirection = false;
    bool _downDirection = false;

    private bool _deadFlg;
    private bool _successFlg;
    
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // AudioManager�̃Q�[���I�u�W�F�N�g��T��
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        _unitySE = GetComponents<AudioSource>();
        // �R���|�[�l���g�̎擾���s��
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _deadFlg = false;
        _successFlg = false;
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
        // unityChan�������Ă���ꍇ
        if (false == _deadFlg)
        {
            _deadFlg = true;
            _animator.SetBool("Dead", true);

            // ��Q���ɓ���������SE�𗬂�
            _audioClip = Resources.Load("SE/Damage/Damage") as AudioClip;
            _audioManager.DamageSE(_audioClip);

            _animator.SetTrigger("Damage");
            _animator.SetTrigger("KneelDown");

            // ��ɂ������E�E�E(����)
            _unitySE[0].Play();
            _gameManager.GameOverFlgSet(true);
        }
    }

    // SpotArea����Ă΂��
    public void unitySuccess()
    {
        //Debug.Log("�Q�[���N���A�t���O�@�F" + gameManager.GameClearFlg);
        if (false == _successFlg)
        {
            if (_gameManager.GameClearFlg)
            {
                //Debug.Log("SUCCESS");
                _successFlg = true;
                _animator.SetTrigger("Success");
                _unitySE[1].Play();
                return;
            }
        }
    }

    // ���͂��ꂽ�����ɍ��W��ύX����
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

    void Update()
    {
        // unityChan�����񂾁A�������̓Q�[���N���A�̏ꍇ
        if ((true == _deadFlg) || (true == _successFlg))
        {
            _animator.SetFloat("Speed", 0f);
            return;
        }

        // �n�ʂƐڒn���Ă���ꍇ
        if (_characterController.isGrounded)
        {
            // ���x�̏�����
            _velocity = Vector3.zero;

            // ���[���h���W(�O���[�o�����W)
            // x���ɍ��E�L�[�̒l�Az���ɏc���L�[�̒l��ݒ肷��
            _input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // �ړ�
            ToUp();
            ToRight();
            ToDown();
            ToLeft();

            // 3�����x�N�g���̒���(x^2 + y^2 + z^2 �̃��[�g)
            if (_input.magnitude > 0.1f)
            {
                // ���[���h���W�����Č�����ς��Ă���
                transform.rotation = Quaternion.LookRotation(_input);

                // ���݂̈ʒu�ɓ��͂��������𑫂��āA���̕�������������
                //transform.LookAt(transform.position + input.normalized);

                // �A�j���[�^�N��
                _animator.SetFloat("Speed", _input.magnitude);

                if (_input.magnitude > 0.5f)
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

        // �W�����v�@�\�͂Ȃ��ɂ���
        //if (Input.GetButtonDown("Jump"))
        //{
        //    velocity.y = jumpSpeed;
        //    animator.SetTrigger("Jump");
        //}

        // CharactorController�͏d�͂��l������K�v������
        _velocity.y += Physics.gravity.y * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
