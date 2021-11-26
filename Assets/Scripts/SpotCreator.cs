using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotCreator : MonoBehaviour
{
    const float SPOT_ANGLE = 12.0f;
    const float spotRangeOffset = 0.5f;
    // spot�̍����͌Œ�Ƃ���
    const float spotHeight = 5.0f;

    private GameManager gameManager;
    private StageManager stageManager;
    private Light spotLight;

    private Vector3 stageScale;
    private Vector3 spotPosition;
    private float spotPosition_x;
    private float spotPosition_z;
    private float orgSpotLightAngle;
    
    // �t�F�[�h�p�̉�f���
    private float red;
    private float green;
    private float blue;
    private float alpha;

    void Start()
    {
        // GameManager�C���X�^���X�擾
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        // Light�̃R���|�[�l���g�擾
        spotLight = GameObject.Find("SpotLight").GetComponent<Light>();
        orgSpotLightAngle = spotLight.spotAngle;

        red = spotLight.color.r;
        green = spotLight.color.g;
        blue = spotLight.color.b;
        alpha = spotLight.color.a;

        // SpotArea�̎����z�u���s��
        CreateSpotArea();
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = new Ray(transform.position, Vector3.down);

        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit)) // ����Ray�𓊎˂��ĉ��炩�̃R���C�_�[�ɏՓ˂�����
        //{
        //    string name = hit.collider.gameObject.name; // �Փ˂�������I�u�W�F�N�g�̖��O���擾
        //}

        // �X�|�b�g�p�x��0�ȏ�̏ꍇ
        if (spotLight.spotAngle > SPOT_ANGLE)
        {
            spotLight.color = new Color(red, green, blue, alpha);
        }
        else
        {
            spotLight.color = new Color(1.0f, 0.0f, 0.0f, alpha);
        }

        // �X�|�b�g�p�x��0�ȏ�̏ꍇ
        if (spotLight.spotAngle > 1)
        {
            if (false == gameManager.GameClearFlg)
            {
                // �p�x������������������
                spotLight.spotAngle -= Time.deltaTime * 2.0f;
            }
        }
        else
        {
            spotLight.spotAngle = 1;
            ReCreateApotArea();
        }
    }

    /// <summary>
    /// SpotArea���Ĕz�u����
    /// </summary>
    private void ReCreateApotArea()
    {
        CreateSpotArea();
        spotLight.spotAngle = orgSpotLightAngle;
    }

    /// <summary>
    /// SpotArea�������z�u����
    /// </summary>
    private void CreateSpotArea()
    {
        // �X�e�[�W�̃X�P�[�����擾���āA�X�|�b�g�G���A�̐����͈͂����߂�
        spotPosition_x = (stageManager.StageScale_x / 2) - spotRangeOffset;
        spotPosition_z = (stageManager.StageScale_z / 2) - spotRangeOffset;

        transform.position = new Vector3(Random.Range((-1) * spotPosition_x, spotPosition_x),
                                         spotHeight,
                                         Random.Range((-1) * spotPosition_z, spotPosition_z));
    }

}
