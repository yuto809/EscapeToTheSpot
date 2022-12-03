using UnityEngine;

public class Collision : MonoBehaviour
{
    // unityChanにアタッチされたスクリプトを、アタッチする
    [SerializeField]
    private UnityChanController _unityChan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            _unityChan.UnityDead();
        }
    }
}
