using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles : MonoBehaviour
{
    // menuにアセットバンドル作成を追加
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

        // 終了メッセージ表示
        EditorUtility.DisplayDialog("AssetBundleビルド", "AssetBundleのビルドが完了しました", "OK");
    }
}