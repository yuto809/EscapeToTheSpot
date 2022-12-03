using UnityEngine;

// 渡された型(T)で処理を行い、渡された型(T)はWhereによってMonoBehavorを継承している
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// xxManagerのインスタンスをシングルトンとして保持する
    /// </summary>
    private static T _Instance;

    /// <summary>
    /// 渡された型のインスタンスを生成する
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = (T)FindObjectOfType(typeof(T));

                if (_Instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }

            return _Instance;
        }
    }
}