using UnityEngine;

// �n���ꂽ�^(T)�ŏ������s���A�n���ꂽ�^(T)��Where�ɂ����MonoBehavor���p�����Ă���
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// xxManager�̃C���X�^���X���V���O���g���Ƃ��ĕێ�����
    /// </summary>
    private static T _Instance;

    /// <summary>
    /// �n���ꂽ�^�̃C���X�^���X�𐶐�����
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