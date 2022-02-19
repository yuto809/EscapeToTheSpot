using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // UnityChanをアタッチ
    [SerializeField]
    private GameObject _unityChan;
    
    // 追従用カメラの速さ
    [SerializeField]
    private float _followSpeed;

    // UnityChanと追従用カメラとの距離の差
    private Vector3 targetDiff;

    // Start is called before the first frame update
    private void Start()
    {
        // UnityChanと追従用のカメラとの距離を算出
        targetDiff = _unityChan.transform.position - transform.position;
    }

    // Update関数後のフレーム更新処理
    private void LateUpdate()
    {
        if (_unityChan != null)
        {
            // UnityChanが移動後、ゆっくりカメラが近づく
            transform.position = Vector3.Lerp(transform.position, _unityChan.transform.position - targetDiff, Time.deltaTime * _followSpeed);
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
