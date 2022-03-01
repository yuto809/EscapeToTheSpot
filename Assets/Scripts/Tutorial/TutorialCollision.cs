using UnityEngine;

public class TutorialCollision : MonoBehaviour
{
    // unityChanにアタッチされたスクリプトを、アタッチする
    [SerializeField]
    private TutorialUnityChanController _unityChan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            _unityChan.UnityDead();
        }
    }
}
