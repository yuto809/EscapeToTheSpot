using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotCreator : MonoBehaviour
{
    const float SPOT_ANGLE = 12.0f;
    const float spotRangeOffset = 0.5f;
    // spotの高さは固定とする
    const float spotHeight = 5.0f;

    private GameManager gameManager;
    private StageManager stageManager;
    private Light spotLight;

    private Vector3 stageScale;
    private Vector3 spotPosition;
    private float spotPosition_x;
    private float spotPosition_z;
    private float orgSpotLightAngle;
    
    // フェード用の画素情報
    private float red;
    private float green;
    private float blue;
    private float alpha;

    void Start()
    {
        // GameManagerインスタンス取得
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        // Lightのコンポーネント取得
        spotLight = GameObject.Find("SpotLight").GetComponent<Light>();
        orgSpotLightAngle = spotLight.spotAngle;

        red = spotLight.color.r;
        green = spotLight.color.g;
        blue = spotLight.color.b;
        alpha = spotLight.color.a;

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
        if (spotLight.spotAngle > SPOT_ANGLE)
        {
            spotLight.color = new Color(red, green, blue, alpha);
        }
        else
        {
            spotLight.color = new Color(1.0f, 0.0f, 0.0f, alpha);
        }

        // スポット角度が0以上の場合
        if (spotLight.spotAngle > 1)
        {
            if (false == gameManager.GameClearFlg)
            {
                // 角度を少しずつ小さくする
                spotLight.spotAngle -= Time.deltaTime * 2.0f;
            }
        }
        else
        {
            spotLight.spotAngle = 1;
            ReCreateApotArea();
        }
    }

    /// <summary>
    /// SpotAreaを再配置する
    /// </summary>
    private void ReCreateApotArea()
    {
        CreateSpotArea();
        spotLight.spotAngle = orgSpotLightAngle;
    }

    /// <summary>
    /// SpotAreaを自動配置する
    /// </summary>
    private void CreateSpotArea()
    {
        // ステージのスケールを取得して、スポットエリアの生成範囲を決める
        spotPosition_x = (stageManager.StageScale_x / 2) - spotRangeOffset;
        spotPosition_z = (stageManager.StageScale_z / 2) - spotRangeOffset;

        transform.position = new Vector3(Random.Range((-1) * spotPosition_x, spotPosition_x),
                                         spotHeight,
                                         Random.Range((-1) * spotPosition_z, spotPosition_z));
    }

}
