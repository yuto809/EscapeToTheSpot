using UnityEngine;

public class SpotCreator : MonoBehaviour
{
    const float SPOT_ANGLE = 12.0f;
    const float SPOT_RANGE_OFFSET = 0.5f;
    // spotの高さは固定とする
    const float SPOT_HEIGHT = 5.0f;

    private GameManager _gameManager;
    private StageManager _stageManager;
    private Light _spotLight;

    private Vector3 _stageScale;
    private Vector3 _spotPosition;
    private float _spotPositionX;
    private float _spotPositionZ;
    private float _orgSpotLightAngle;
    
    // フェード用の画素情報
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;

    private void Start()
    {
        // GameManagerインスタンス取得
        _gameManager = GameManager.Instance;
        _stageManager = StageManager.Instance;

        // Lightのコンポーネント取得
        _spotLight = GameObject.Find("SpotLight").GetComponent<Light>();
        _orgSpotLightAngle = _spotLight.spotAngle;

        _red = _spotLight.color.r;
        _green = _spotLight.color.g;
        _blue = _spotLight.color.b;
        _alpha = _spotLight.color.a;

        // SpotAreaの自動配置を行う
        CreateSpotArea();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// SpotAreaを再配置する
    /// </summary>
    public void ReCreateSpotArea()
    {
        _spotLight.spotAngle = 1;
        CreateSpotArea();
        _spotLight.spotAngle = _orgSpotLightAngle;
    }

    /// <summary>
    /// SpotAreaを自動配置する
    /// </summary>
    private void CreateSpotArea()
    {
        // ステージのスケールを取得して、スポットエリアの生成範囲を決める
        _spotPositionX = (_stageManager.StageScaleX / 2) - SPOT_RANGE_OFFSET;
        _spotPositionZ = (_stageManager.StageScaleZ / 2) - SPOT_RANGE_OFFSET;

        transform.position = new Vector3(Random.Range((-1) * _spotPositionX, _spotPositionX),
                                         SPOT_HEIGHT,
                                         Random.Range((-1) * _spotPositionZ, _spotPositionZ));
    }

}
