using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private Rigidbody _rb;
    private float _speed = 30f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();        
    }

    void FixedUpdate()
    {
        // 一定の速度を超えたら処理しない
        if (_rb.velocity.magnitude < 20)
        {
            // 一定の速度に近づけるようにする
            //指定したスピードから現在の速度を引いて加速力を求める
            float currentSpeed = _speed - _rb.velocity.magnitude;
            //調整された加速力で力を加える
            _rb.AddForce(new Vector3(0, 0, -currentSpeed));
        }
    }
}
