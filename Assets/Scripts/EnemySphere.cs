using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySphere : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float speed;

    private float enemySpeed;
    private Rigidbody rigid;
    private GameManager gameManager;

    void Start()
    {
        enemySpeed = speed;// Random.Range(1, speed);
        rigid = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    // FixedUpdateは一定秒数ごとに呼ばれる
    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (gameManager.GameClearFlg)
        {
            Destroy(this.gameObject);
            return;
        }

        // AddForceは、物体に力を加えて動かす機能
        // 質量を考慮して継続的に力を加える
        // 向きたいターゲット位置から自分の位置を引くことで、ターゲットに向かうベクトルが求められる
        // 向きさえ知りたいから、単位ベクトルに直して、速度をかける
        rigid.AddForce((target.transform.position - transform.position).normalized * enemySpeed);
    }
}
