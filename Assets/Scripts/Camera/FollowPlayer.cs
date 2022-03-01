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
    }
}
