using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles : MonoBehaviour
{
    // menu�ɃA�Z�b�g�o���h���쐬��ǉ�
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
 
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
        BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        // �I�����b�Z�[�W�\��
        EditorUtility.DisplayDialog("AssetBundle�r���h", "AssetBundle�̃r���h���������܂���", "OK");
    }
}