using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    const float DESTROY_AREA = 30.0f;

    private GameManager gameManager;
    private StageManager stageManager;
    private AudioSource unityFall;
    private BoxCollider boxCollider;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        boxCollider = GetComponent<BoxCollider>();
        unityFall = GetComponent<AudioSource>();

        SetDestroyArea();
    }

    private void SetDestroyArea()
    {
        // Hard�̏ꍇ
        if (2 == stageManager.SelectStageLevel)
        {
            boxCollider.size = new Vector3(DESTROY_AREA * 2, 1.0f, DESTROY_AREA * 2);
        }
        else
        {
            boxCollider.size = new Vector3(DESTROY_AREA, 1.0f, DESTROY_AREA);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "unityChan")
        {
            gameManager.GameOverFlgSet(true);
            unityFall.Play();
        }

        Destroy(other.gameObject);
    }
}
