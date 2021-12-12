using UnityEngine;

public class CameraManager : MonoBehaviour
{
    const int levelHard = 2;
    const float mainPosY = 8.0f;
    const float mainPosZ = -15.0f;
    const float mainPosYHard = 20.0f;
    const float mainPosZHard = -32.0f;

    // 他クラスからはいじらせない
    [SerializeField]
    private Camera _followCamera;

    [SerializeField]
    private Camera _mainCamera;

    private StageManager _stageManager;

    void Start()
    {
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        if (levelHard == _stageManager.SelectStageLevel)
        {
            _mainCamera.transform.position = new Vector3(0.0f, mainPosYHard, mainPosZHard);
        }
        else
        {
            _mainCamera.transform.position = new Vector3(0.0f, mainPosY, mainPosZ);
        }

        // 最初は追従用カメラはOFFにする
        _mainCamera.enabled = false;
    }

    public void CameraChange()
    {
        // 追従用カメラがOFFの場合
        if (_mainCamera.enabled == false)
        {
            // 追従用カメラをON、メインカメラをOFF
            _mainCamera.enabled = true;
        }
        else
        {
            // 追従用カメラをON、メインカメラをOFF
            _mainCamera.enabled = false;
        }
    }

    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    // 追従用カメラがOFFの場合
        //    if (mainCamera.enabled == false)
        //    {
        //        // 追従用カメラをON、メインカメラをOFF
        //        mainCamera.enabled = true;
        //        followCamera.enabled = false;
        //    }
        //    else
        //    {
        //        // 追従用カメラをON、メインカメラをOFF
        //        mainCamera.enabled = false;
        //        followCamera.enabled = true;
        //    }
        //}
    }
}
