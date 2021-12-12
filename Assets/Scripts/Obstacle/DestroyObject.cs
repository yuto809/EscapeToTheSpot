using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    const float DESTROY_AREA = 30.0f;

    private GameManager _gameManager;
    private StageManager _stageManager;
    private AudioSource _unityFall;
    private BoxCollider _boxCollider;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        _boxCollider = GetComponent<BoxCollider>();
        _unityFall = GetComponent<AudioSource>();

        SetDestroyArea();
    }

    private void SetDestroyArea()
    {
        // Hardの場合
        if (2 == _stageManager.SelectStageLevel)
        {
            _boxCollider.size = new Vector3(DESTROY_AREA * 2, 1.0f, DESTROY_AREA * 2);
        }
        else
        {
            _boxCollider.size = new Vector3(DESTROY_AREA, 1.0f, DESTROY_AREA);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "unityChan")
        {
            _gameManager.GameOverFlgSet(true);
            _unityFall.Play();
        }

        Destroy(other.gameObject);
    }
}
