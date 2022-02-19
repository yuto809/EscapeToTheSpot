using UnityEngine;
using UnityEngine.Events;

public class DestroyObject : MonoBehaviour
{
    const float DESTROY_AREA = 30.0f;

    private AudioManager _audioManager;
    private GameManager _gameManager;
    private StageManager _stageManager;
    private AudioSource _unityFall;
    private BoxCollider _boxCollider;

    // Inspector����GameManager���A�^�b�`����ƁA�V�[���ēǂݍ��ݎ���
    // DontDestroyLoad�I�u�W�F�N�g����GameManager������邽��
    // Inspector���ł̃C�x���g�o�^�͕s��
    //[SerializeField]
    //private UnityEvent _setGameOverFlgEvent = new UnityEvent();

    private UnityEvent _setGameOverFlgEventBuckUp;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _gameManager.SetGameOverFlgEvent();
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _stageManager = StageManager.Instance;
        _boxCollider = GetComponent<BoxCollider>();
        _unityFall = GetComponent<AudioSource>();

        SetDestroyArea();
    }

    private void SetDestroyArea()
    {
        if (_stageManager.SelectStageLevel == (int)StageManager.StageLevel.HARD)
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
        if (other.CompareTag("unityChan"))
        {
            // GameManger�ɃC�x���g���s���āAGameManger���Ńt���O��Ԃ��X�V����
            //if (_setGameOverFlgEvent == null)
            //{
            //    Debug.Log("Event Null");
            //    _setGameOverFlgEvent = new UnityEvent();
            //}

            //Debug.Log("Event");
            _gameManager.CallGameOverFlgEvent();
            _audioManager.PlayMusicVoice((int)AudioManager.PlayVoice.VOICE_FALL);

        }

        Destroy(other.gameObject);
    }
}
