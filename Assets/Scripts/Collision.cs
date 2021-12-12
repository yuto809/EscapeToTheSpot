using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField]
    private UnityChanController _unityChan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "obstacle")
        {
            _unityChan.unityDead();
        }
    }
}
