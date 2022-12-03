using System.Collections;
using UnityEngine;

public class SpotArea : MonoBehaviour
{
    const int COUNT_CLEARTIME = 3;
    const float SPOT_ANGLE = 12.0f;

    [SerializeField]
    private string _targetTag;

    [SerializeField]
    private UnityChanController _unityChan;

    private int counter;
    private Coroutine _timerCoroutine;
    private GameManager _gameManager;
    private SpotCreator _spotCreator;
    private Light _spotLight;
    private AudioSource[] _spotSE;
    private StageManager _stageManager;
    private bool _judgeClearOnce;
    private bool _clearFlg;
    // フェード用の画素情報
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;

    private void Awake()
    {
        // GameManagerインスタンス取得
        _gameManager = GameManager.Instance;
        _gameManager.SetGameClearFlgEvent();
    }

    private void Start()
    {
        // GameManagerインスタンス取得
        _stageManager = StageManager.Instance;
        _spotSE = GetComponents<AudioSource>();

        // SpotCreatorインスタンス取得
        _spotCreator = GameObject.Find("SpotLight").GetComponent<SpotCreator>();
        _spotLight = _spotCreator.GetComponent<Light>();

        _red = _spotLight.color.r;
        _green = _spotLight.color.g;
        _blue = _spotLight.color.b;
        _alpha = _spotLight.color.a;

        _judgeClearOnce = false;
        _clearFlg = false;
    }

    private void Update()
    {
        if (_gameManager.GameOverFlg)
        {
            // コルーチンが生きている場合
            if (null != _timerCoroutine)
            {
                StopCoroutine(_timerCoroutine);
                _spotSE[0].Stop();
            }
        }

        // スポットエリアの状態を更新する
        UpdateSpotAreaStatus();

        // 3秒間スポットエリア内に留まれた場合
        if (_clearFlg)
        {
            _spotSE[0].Stop();
            _spotSE[1].Play();

            _gameManager.CallGameClearFlgEvent();
            
            _unityChan.UnitySuccess();
            _clearFlg = false;
        }
    }

    private void UpdateSpotAreaStatus()
    {
        // SPOT_ANGLEを基準に、ゲームクリア判定が可能となる色(デフォルトの黄色)を表示する
        if (_spotLight.spotAngle > SPOT_ANGLE)
        {
            _spotLight.color = new Color(_red, _green, _blue, _alpha);
        }
        else
        {
            // 赤色は、ゲームクリア判定不可とする
            _spotLight.color = new Color(1.0f, 0.0f, 0.0f, _alpha);
        }

        // 経過時間ごとにスポットエリアをある程度まで(現状は1としている)縮めていく
        if (1 < _spotLight.spotAngle)
        {
            if (false == _gameManager.GameClearFlg)
            {
                // 角度を少しずつ小さくする
                _spotLight.spotAngle -= Time.deltaTime * 2.0f;
            }
        }
        else
        {
            // スポットエリアを作り直す
            _spotCreator.ReCreateSpotArea();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // ゲームオーバーの場合はクリア判断処理は実行させない
        if (_gameManager.GameOverFlg)
        {
            //gameOverFlg = true;
            // コルーチンが生きている場合
            if (null != _timerCoroutine)
            {
                StopCoroutine(_timerCoroutine);
            }
            // エリア内に留まっているときのSEを止める
            _spotSE[0].Stop();
            return;
        }

        // 一定角度未満になったら衝突判定はしない
        if (_spotLight.spotAngle < SPOT_ANGLE)
        {
            // コルーチンが生きている場合
            if (null != _timerCoroutine)
            {
                StopCoroutine(_timerCoroutine);
            }

            // エリア内に留まっているときのSEを止める
            _spotSE[0].Stop();
            return;
        }

        // ゲームオーバーとなっていない場合 かつ、1度もエリア判定処理を行っていない場合
        if ((false == _gameManager.GameOverFlg) && (false == _judgeClearOnce))
        {
            // UnityChanがコライダー範囲内にある場合
            // クリア判断を行う
            if (other.CompareTag(_targetTag))
            {
                if (_stageManager.SelectStageLevel == (int)StageManager.StageLevel.HARD)
                {
                    _gameManager.StaySpotArea = true;
                }

                _timerCoroutine = StartCoroutine(TimeCount());
                _spotSE[0].Play();
                _judgeClearOnce = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // コライダー範囲内からUnityChanが出た場合
        if (other.CompareTag(_targetTag))
        {
            // コルーチンが生きている場合
            if (null != _timerCoroutine)
            {
                StopCoroutine(_timerCoroutine);
            }

            // エリア内に留まっているときのSEを止める
            _spotSE[0].Stop();
            _gameManager.StaySpotArea = false;
            _judgeClearOnce = false;
            _clearFlg = false;
        }
    }

    IEnumerator TimeCount()
    {
        counter = COUNT_CLEARTIME;

        // 3秒経過後にクリア判定を行う
        while (counter > 0)
        {
            yield return new WaitForSeconds(1.0f);
            counter--;
        }

        _clearFlg = true;
    }
}
