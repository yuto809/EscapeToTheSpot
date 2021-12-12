using System.Collections;
using UnityEngine;

public class SpotArea : MonoBehaviour
{
    const int countTime = 3;
    const float SPOT_ANGLE = 12.0f;

    [SerializeField]
    private string _targetTag;

    [SerializeField]
    private UnityChanController _unityChan;

    private int counter = 0;
    private Coroutine _timerCoroutine;
    private GameManager _gameManager;
    private SpotCreator _spotCreator;
    private Light _spotLight;
    private AudioSource[] _spotSE;
    private StageManager _stageManager;
    private bool _spotJudge;

    void Start()
    {
        // GameManagerインスタンス取得
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        _spotSE = GetComponents<AudioSource>();

        // SpotCreatorインスタンス取得
        _spotCreator = GameObject.Find("SpotLight").GetComponent<SpotCreator>();
        _spotLight = _spotCreator.GetComponent<Light>();

        _spotJudge = false;
    }
    
    void Update()
    {
        if (_gameManager.GameOverFlg)
        {
            // コルーチンが生きている場合
            if (null != _timerCoroutine)
            {
                StopCoroutine(_timerCoroutine);
                _spotSE[0].Stop();
                //gameManager.GameClearFlg = false;
                _gameManager.GameOverFlgSet(false);
            }
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
        if ((false == _gameManager.GameOverFlg) && (false == _spotJudge))
        {
            // UnityChanがコライダー範囲内にある場合
            // クリア判断を行う
            if (other.gameObject.tag == _targetTag)
            {
                if (_stageManager.SelectStageLevel == 2)
                {
                    _gameManager.StaySpotArea = true;
                }

                _timerCoroutine = StartCoroutine(TimeCount());
                _spotSE[0].Play();
                _spotJudge = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // コライダー範囲内からUnityChanが出た場合
        if (other.gameObject.tag == _targetTag)
        {
            // コルーチンが生きている場合
            if (null != _timerCoroutine)
            {
                StopCoroutine(_timerCoroutine);
            }

            // エリア内に留まっているときのSEを止める
            _spotSE[0].Stop();
            _gameManager.GameOverFlgSet(false);
            _gameManager.StaySpotArea = false;
            _spotJudge = false;
        }
    }

    IEnumerator TimeCount()
    {
        counter = countTime;

        // 5秒経過後にクリア判定を行う
        while (counter > 0)
        {
            yield return new WaitForSeconds(1.0f);
            //Debug.Log("Enter Collider Capsule");
            counter--;
        }

        _spotSE[0].Stop();
        _spotSE[1].Play();

        // GameManagerにクリア報告を渡す
        // SendMessageは複数オブジェクトで同じスクリプトを使用している場合に役立つ
        _gameManager.GameClearFlgSet(true);
        _unityChan.unitySuccess();
    }
}
