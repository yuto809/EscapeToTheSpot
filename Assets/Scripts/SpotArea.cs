using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotArea : MonoBehaviour
{
    const int countTime = 3;
    const float SPOT_ANGLE = 12.0f;

    [SerializeField]
    private string targetTag;

    [SerializeField]
    private UnityChanController unityChan;

    int counter = 0;
    Coroutine timerCoroutine;
    GameManager gameManager;
    SpotCreator spotCreator;
    Light spotLight;
    private AudioSource[] spotSE;
    private StageManager stageManager;
    private bool spotJudge;

    void Start()
    {
        // GameManagerインスタンス取得
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        spotSE = GetComponents<AudioSource>();

        // SpotCreatorインスタンス取得
        spotCreator = GameObject.Find("SpotLight").GetComponent<SpotCreator>();
        spotLight = spotCreator.GetComponent<Light>();

        spotJudge = false;
    }
    
    void Update()
    {
        if (gameManager.GameOverFlg)
        {
            // コルーチンが生きている場合
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
                spotSE[0].Stop();
                //gameManager.GameClearFlg = false;
                gameManager.GameOverFlgSet(false);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        // ゲームオーバーの場合はクリア判断処理は実行させない
        if (gameManager.GameOverFlg)
        {
            //gameOverFlg = true;
            // コルーチンが生きている場合
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
            }
            // エリア内に留まっているときのSEを止める
            spotSE[0].Stop();
            return;
        }

        // 一定角度未満になったら衝突判定はしない
        if (spotLight.spotAngle < SPOT_ANGLE)
        {
            // コルーチンが生きている場合
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
            }

            // エリア内に留まっているときのSEを止める
            spotSE[0].Stop();
            return;
        }

        // ゲームオーバーとなっていない場合 かつ、1度もエリア判定処理を行っていない場合
        if ((false == gameManager.GameOverFlg) && (false == spotJudge))
        {
            // UnityChanがコライダー範囲内にある場合
            // クリア判断を行う
            if (other.gameObject.tag == targetTag)
            {
                if (stageManager.SelectStageLevel == 2)
                {
                    gameManager.StaySpotArea = true;
                }

                timerCoroutine = StartCoroutine(TimeCount());
                spotSE[0].Play();
                spotJudge = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // コライダー範囲内からUnityChanが出た場合
        if (other.gameObject.tag == targetTag)
        {
            // コルーチンが生きている場合
            if (null != timerCoroutine)
            {
                StopCoroutine(timerCoroutine);
            }

            // エリア内に留まっているときのSEを止める
            spotSE[0].Stop();
            gameManager.GameOverFlgSet(false);
            gameManager.StaySpotArea = false;
            spotJudge = false;
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

        spotSE[0].Stop();
        spotSE[1].Play();

        // GameManagerにクリア報告を渡す
        // SendMessageは複数オブジェクトで同じスクリプトを使用している場合に役立つ
        gameManager.GameClearFlgSet(true);
        unityChan.unitySuccess();
    }
}
