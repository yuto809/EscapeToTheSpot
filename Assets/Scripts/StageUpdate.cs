using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUpdate : MonoBehaviour
{
    private GameObject stage;
    private StageManager stageManager;

    // Start is called before the first frame update
    void Start()
    {
        stage = GameObject.Find("Stage");
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1�x�����X�e�[�W�̃X�P�[����ύX����(Hard�݂̂���)
        if (stageManager.UpdateStageScale)
        {
            stage.transform.localScale = new Vector3((stageManager.StageScale_x), 1.0f, (stageManager.StageScale_z));
            stageManager.UpdateStageScale = false;
        }
    }
}
