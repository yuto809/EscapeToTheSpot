using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject unityChan;

    [SerializeField]
    private GameObject ball;

    public float ballSpeed = 10f;

    void Start()
    {
        // UnityChan•ûŒü‚ğŒü‚­
        transform.LookAt(unityChan.transform);
        StartCoroutine("BallShot");
    }

    void Update()
    {
        // í‚ÉUnityChan•ûŒü‚ğŒü‚­
        transform.LookAt(unityChan.transform);
    }

    IEnumerator BallShot()
    {
        while (true)
        {
            // debug
            // Debug.Log("x : " + transform.position.x + "y : " + transform.position.y + "z : " + transform.position.z);
            var shot = Instantiate(ball, transform.position + new Vector3(0, 0, 0.5f), Quaternion.identity);
            shot.GetComponent<Rigidbody>().velocity = transform.forward.normalized * ballSpeed;
            yield return new WaitForSeconds(2.0f);
        }
    }
}
