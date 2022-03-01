using UnityEngine;

public class StageManager : SingletonMonoBehaviour<StageManager>
{
    public enum StageLevel
    {
        EASY = 0,
        NORMAL,
        HARD,
        TUTORIAL,
    }

    // ステージのスケール初期値
    const float orgStageScaleX = 20.0f;
    const float orgStageScaleZ = 20.0f;

    // ステージレベル
    public int SelectStageLevel { set; get; }

    // 現在のステージのスケール情報(x方向)
    // 初期値のステージスケール情報をセット
    public float StageScaleX { set; get; }

    // 現在のステージのスケール情報(z方向)
    public float StageScaleZ { set; get; }

    public bool UpdateStageScale { get; set; }

    // シングルトンでSceneを跨いでもオブジェクトは残すようにする
    private void Awake()
    {
        // インスタンスが見つからない場合は破棄
        if (this != StageManager.Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        SetStageInfo();
    }

    // 各レベルに応じたステージ情報を設定
    public void SetStageInfo()
    {
        if ((int)StageManager.StageLevel.HARD == SelectStageLevel)
        {
            StageScaleX = orgStageScaleX * 2;
            StageScaleZ = orgStageScaleZ * 2;
        }
        else
        {
            StageScaleX = orgStageScaleX;
            StageScaleZ = orgStageScaleZ;
        }

        UpdateStageScale = true;
    }
}
