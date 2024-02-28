using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEdgeController : MonoBehaviour
{
    float MoveConst;
    public Vector3 ProjectedMove;
    public bool signal;

    // Update is called once per frame
    void Start()
    {
        signal = false;

        MoveConst = 16f;

        if (transform.localPosition.x < 0f)
        {
            MoveConst *= -1;
        }
    }

    void Update()
    {
        ProjectedMove = new Vector3(Camera.main.transform.position.x + MoveConst, 0f, -10f);
    }

    private void OnTriggerEnter2D(Collider2D subject)
    {
        if (subject.transform.tag == "PlayerCore")
        {
            signal = true;
        }
    }
}
