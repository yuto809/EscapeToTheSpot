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

    // ���N���X����͂����点�Ȃ�
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
 
        // �ŏ��͒Ǐ]�p�J������OFF�ɂ���
        mainCamera.enabled = false;
    }

    public void CameraChange()
    {
        // �Ǐ]�p�J������OFF�̏ꍇ
        if (mainCamera.enabled == false)
        {
            // �Ǐ]�p�J������ON�A���C���J������OFF
            mainCamera.enabled = true;
        }
        else
        {
            // �Ǐ]�p�J������ON�A���C���J������OFF
            mainCamera.enabled = false;
        }
    }

    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    // �Ǐ]�p�J������OFF�̏ꍇ
        //    if (mainCamera.enabled == false)
        //    {
        //        // �Ǐ]�p�J������ON�A���C���J������OFF
        //        mainCamera.enabled = true;
        //        followCamera.enabled = false;
        //    }
        //    else
        //    {
        //        // �Ǐ]�p�J������ON�A���C���J������OFF
        //        mainCamera.enabled = false;
        //        followCamera.enabled = true;
        //    }
        //}
    }
}
