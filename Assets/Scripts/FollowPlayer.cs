using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // UnityChan���A�^�b�`
    [SerializeField]
    private GameObject unityChan;
    
    // �Ǐ]�p�J�����̑���
    [SerializeField]
    private float followSpeed;

    // UnityChan�ƒǏ]�p�J�����Ƃ̋����̍�
    Vector3 targetDiff;

    // Start is called before the first frame update
    void Start()
    {
        // UnityChan�ƒǏ]�p�̃J�����Ƃ̋������Z�o
        targetDiff = unityChan.transform.position - transform.position;
    }

    // Update�֐���̃t���[���X�V����
    void LateUpdate()
    {
        if (unityChan != null)
        {
            // UnityChan���ړ���A�������J�������߂Â�
            transform.position = Vector3.Lerp(transform.position, unityChan.transform.position - targetDiff, Time.deltaTime * followSpeed);
        }
        //// �}�E�X�̉E�N���b�N�������Ă����
        //if (Input.GetMouseButton(1))
        //{
        //    // �}�E�X�̈ړ���
        //    float mouseInputX = Input.GetAxis("Mouse X");
        //    float mouseInputY = Input.GetAxis("Mouse Y");
        //    // target�̈ʒu��Y���𒆐S�ɁA��]�i���]�j����
        //    transform.RotateAround(unityChan.transform.position, Vector3.up, mouseInputX * Time.deltaTime * 200f);
        //    // �J�����̐����ړ��i���p�x�����Ȃ��A�K�v��������΃R�����g�A�E�g�j
        //    //transform.RotateAround(unityChan.transform.position, transform.right, mouseInputY * Time.deltaTime * 200f);
        //}
    }
}
