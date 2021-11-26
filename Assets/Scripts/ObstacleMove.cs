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
        // 一定の速度を超えたら処理しない
        if (rb.velocity.magnitude < 20)
        {
            // 一定の速度に近づけるようにする
            //指定したスピードから現在の速度を引いて加速力を求める
            float currentSpeed = speed - rb.velocity.magnitude;
            //調整された加速力で力を加える
            rb.AddForce(new Vector3(0, 0, -currentSpeed));
        }
    }
}
