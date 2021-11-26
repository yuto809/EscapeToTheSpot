using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    const int levelHard = 2;

    public enum StageLevel
    {
        Level1 = 0,
        Level2,
        Level3
    }

    // ステージのスケール初期値
    const float orgStageScaleX = 20.0f;
    const float orgStageScaleZ = 20.0f;

    //シングルトン設定ここから
    static public StageManager instance;

    // ステージレベル
    public int SelectStageLevel { set; get; }

    // 現在のステージのスケール情報(x方向)
    // 初期値のステージスケール情報をセット
    public float StageScale_x { set; get; }

    // 現在のステージのスケール情報(z方向)
    public float StageScale_z { set; get; }

    public bool UpdateStageScale { get; set; }

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SetStageInfo();
    }

    // 各レベルに応じたステージ情報を設定
    public void SetStageInfo()
    {
        if (levelHard == SelectStageLevel)
        {
            Debug.Log("Select Hard");
            StageScale_x = orgStageScaleX * 2;
            StageScale_z = orgStageScaleZ * 2;
        }
        else
        {
            StageScale_x = orgStageScaleX;
            StageScale_z = orgStageScaleZ;
        }

        UpdateStageScale = true;
    }
}
