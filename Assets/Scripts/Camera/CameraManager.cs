using UnityEngine;

public class CameraManager : MonoBehaviour
{
    const float MAIN_POS_Y = 8.0f;
    const float MAIN_POS_Z = -15.0f;
    const float MAIN_POS_Y_HARD = 20.0f;
    const float MAIN_POS_Z_HARD = -32.0f;

    [SerializeField]
    private Camera _viewCamera;

    private StageManager _stageManager;

    private void Start()
    {
        _stageManager = StageManager.Instance;

        if ((int)StageManager.StageLevel.HARD == _stageManager.SelectStageLevel)
        {
            _viewCamera.transform.position = new Vector3(0.0f, MAIN_POS_Y_HARD, MAIN_POS_Z_HARD);
        }
        else
        {
            _viewCamera.transform.position = new Vector3(0.0f, MAIN_POS_Y, MAIN_POS_Z);
        }

        // 最初はviewカメラはOFFにする
        _viewCamera.enabled = false;
    }

    public void CameraChange()
    {
        // ViewカメラがOFFの場合
        if (_viewCamera.enabled == false)
        {
            // Viewカメラ起動
            _viewCamera.enabled = true;
        }
        else
        {
            _viewCamera.enabled = false;
        }
    }

    private void Update()
    {

    }
}
