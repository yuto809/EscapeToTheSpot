using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObjectを継承したクラス
// アセットメニューから作成
[CreateAssetMenu(menuName = "ScriptableObjects/Create StageData")]
public class StageData : ScriptableObject
{
    public List<Stage> StageDataList = new List<Stage>();
}

//System.Serializableを設定しないと、データを保持できない(シリアライズできない)ので注意
[System.Serializable]
public class Stage
{
    [SerializeField]
    private string _name;

    // Stageのマスター
    [SerializeField]
    private float _stageScaleX = 20.0f;
    [SerializeField]
    private float _stageScaleY = 1.0f;
    [SerializeField]
    private float _stageScaleZ = 20.0f;


    public float StageScaleX
    {
        get
        {
            return _stageScaleX;
        }
    }

    public float StageScaleY
    {
        get
        {
            return _stageScaleY;
        }
    }

    public float StageScaleZ
    {
        get
        {
            return _stageScaleZ;
        }
    }
}