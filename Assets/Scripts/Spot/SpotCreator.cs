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
        //Ray ray = new Ray(transform.position, Vector3.down);

        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit)) // もしRayを投射して何らかのコライダーに衝突したら
        //{
        //    string name = hit.collider.gameObject.name; // 衝突した相手オブジェクトの名前を取得
        //}

        // SPOT_ANGLEを基準に、ゲームクリア判定が可能となる色(デフォルトの黄色)を表示する
        //if (_spotLight.spotAngle > SPOT_ANGLE)
        //{
        //    _spotLight.color = new Color(_red, _green, _blue, _alpha);
        //}
        //else
        //{
        //    // 赤色は、ゲームクリア判定不可とする
        //    _spotLight.color = new Color(1.0f, 0.0f, 0.0f, _alpha);
        //}

        //// 経過時間ごとにスポットエリアをある程度まで(現状は1としている)縮めていく
        //if (1 < _spotLight.spotAngle)
        //{
        //    if (false == _gameManager.GameClearFlg)
        //    {
        //        // 角度を少しずつ小さくする
        //        _spotLight.spotAngle -= Time.deltaTime * 2.0f;
        //    }
        //}
        //else
        //{
        //    // スポットエリアを作り直す
        //    _spotLight.spotAngle = 1;
        //    ReCreateSpotArea();
        //}
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
