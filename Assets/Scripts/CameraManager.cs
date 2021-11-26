using System.Collections;
using System.Collections.Generic;
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
    private Camera followCamera;

    [SerializeField]
    private Camera mainCamera;

    private StageManager stageManager;

    void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        if (levelHard == stageManager.SelectStageLevel)
        {
            mainCamera.transform.position = new Vector3(0.0f, mainPosYHard, mainPosZHard);
        }
        else
        {
            mainCamera.transform.position = new Vector3(0.0f, mainPosY, mainPosZ);
        }
 
        // 最初は追従用カメラはOFFにする
        mainCamera.enabled = false;
    }

    public void CameraChange()
    {
        // 追従用カメラがOFFの場合
        if (mainCamera.enabled == false)
        {
            // 追従用カメラをON、メインカメラをOFF
            mainCamera.enabled = true;
        }
        else
        {
            // 追従用カメラをON、メインカメラをOFF
            mainCamera.enabled = false;
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
