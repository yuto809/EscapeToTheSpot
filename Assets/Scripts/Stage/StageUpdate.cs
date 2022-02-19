using UnityEngine;

public class StageUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject _stage;
    private StageManager _stageManager;

    // Start is called before the first frame update
    private void Start()
    {
        //_stage = GameObject.Find("Stage");
        _stageManager = StageManager.Instance;
        //_stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 1度だけステージのスケールを変更する(Hardのみだけ)
        if (_stageManager.UpdateStageScale)
        {
            _stage.transform.localScale = new Vector3((_stageManager.StageScaleX), 1.0f, (_stageManager.StageScaleZ));
            _stageManager.UpdateStageScale = false;
        }
    }
}
