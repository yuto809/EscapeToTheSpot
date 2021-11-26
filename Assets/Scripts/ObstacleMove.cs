using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }

    void FixedUpdate()
    {
        // ���̑��x�𒴂����珈�����Ȃ�
        if (rb.velocity.magnitude < 20)
        {
            // ���̑��x�ɋ߂Â���悤�ɂ���
            //�w�肵���X�s�[�h���猻�݂̑��x�������ĉ����͂����߂�
            float currentSpeed = speed - rb.velocity.magnitude;
            //�������ꂽ�����͂ŗ͂�������
            rb.AddForce(new Vector3(0, 0, -currentSpeed));
        }
    }
}
