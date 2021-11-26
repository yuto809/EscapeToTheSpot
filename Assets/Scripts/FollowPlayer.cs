using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // UnityChanをアタッチ
    [SerializeField]
    private GameObject unityChan;
    
    // 追従用カメラの速さ
    [SerializeField]
    private float followSpeed;

    // UnityChanと追従用カメラとの距離の差
    Vector3 targetDiff;

    // Start is called before the first frame update
    void Start()
    {
        // UnityChanと追従用のカメラとの距離を算出
        targetDiff = unityChan.transform.position - transform.position;
    }

    // Update関数後のフレーム更新処理
    void LateUpdate()
    {
        if (unityChan != null)
        {
            // UnityChanが移動後、ゆっくりカメラが近づく
            transform.position = Vector3.Lerp(transform.position, unityChan.transform.position - targetDiff, Time.deltaTime * followSpeed);
        }
        //// マウスの右クリックを押している間
        //if (Input.GetMouseButton(1))
        //{
        //    // マウスの移動量
        //    float mouseInputX = Input.GetAxis("Mouse X");
        //    float mouseInputY = Input.GetAxis("Mouse Y");
        //    // targetの位置のY軸を中心に、回転（公転）する
        //    transform.RotateAround(unityChan.transform.position, Vector3.up, mouseInputX * Time.deltaTime * 200f);
        //    // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
        //    //transform.RotateAround(unityChan.transform.position, transform.right, mouseInputY * Time.deltaTime * 200f);
        //}
    }
}
