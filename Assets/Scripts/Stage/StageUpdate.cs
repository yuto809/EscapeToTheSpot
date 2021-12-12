using UnityEngine;

public class StageUpdate : MonoBehaviour
{
    private GameObject _stage;
    private StageManager _stageManager;

    // Start is called before the first frame update
    void Start()
    {
        _stage = GameObject.Find("Stage");
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1度だけステージのスケールを変更する(Hardのみだけ)
        if (_stageManager.UpdateStageScale)
        {
            _stage.transform.localScale = new Vector3((_stageManager.StageScale_x), 1.0f, (_stageManager.StageScale_z));
            _stageManager.UpdateStageScale = false;
        }
    }
}
