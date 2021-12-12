using UnityEngine;

public class SpotCreator : MonoBehaviour
{
    const float SPOT_ANGLE = 12.0f;
    const float spotRangeOffset = 0.5f;
    // spotの高さは固定とする
    const float spotHeight = 5.0f;

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

    void Start()
    {
        // GameManagerインスタンス取得
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

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
    void Update()
    {
        //Ray ray = new Ray(transform.position, Vector3.down);

        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit)) // もしRayを投射して何らかのコライダーに衝突したら
        //{
        //    string name = hit.collider.gameObject.name; // 衝突した相手オブジェクトの名前を取得
        //}

        // スポット角度が0以上の場合
        if (_spotLight.spotAngle > SPOT_ANGLE)
        {
            _spotLight.color = new Color(_red, _green, _blue, _alpha);
        }
        else
        {
            _spotLight.color = new Color(1.0f, 0.0f, 0.0f, _alpha);
        }

        // スポット角度が0以上の場合
        if (_spotLight.spotAngle > 1)
        {
            if (false == _gameManager.GameClearFlg)
            {
                // 角度を少しずつ小さくする
                _spotLight.spotAngle -= Time.deltaTime * 2.0f;
            }
        }
        else
        {
            _spotLight.spotAngle = 1;
            ReCreateApotArea();
        }
    }

    /// <summary>
    /// SpotAreaを再配置する
    /// </summary>
    private void ReCreateApotArea()
    {
        CreateSpotArea();
        _spotLight.spotAngle = _orgSpotLightAngle;
    }

    /// <summary>
    /// SpotAreaを自動配置する
    /// </summary>
    private void CreateSpotArea()
    {
        // ステージのスケールを取得して、スポットエリアの生成範囲を決める
        _spotPositionX = (_stageManager.StageScale_x / 2) - spotRangeOffset;
        _spotPositionZ = (_stageManager.StageScale_z / 2) - spotRangeOffset;

        transform.position = new Vector3(Random.Range((-1) * _spotPositionX, _spotPositionX),
                                         spotHeight,
                                         Random.Range((-1) * _spotPositionZ, _spotPositionZ));
    }

}
