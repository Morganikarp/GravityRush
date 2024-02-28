using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraController : MonoBehaviour
{
    public bool Moving;
    Vector3 ProjectedMove;

    public float TransitionSpeed = 20f;

    CameraEdgeController[] CameraEdges = new CameraEdgeController[2];

    GameObject Player;

    bool intro = true;

    float difBound;

    // Start is called before the first frame update
    void Start()
    {
        difBound = 0.01f;
        Moving = false;

        Player = GameObject.Find("Player");


        for (int i = 0; i < CameraEdges.Length; i++)
        {
            CameraEdges[i] = transform.GetChild(i).GetComponent<CameraEdgeController>();
        }

        intro = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (intro)
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10f);

            if (transform.position.y <= 0f)
            {
                transform.position = new Vector3(0f, 0f, -10f);
                intro = false;
            }
        }

        if (!Moving)
        {
            if (CameraEdges[0].signal)
            {
                ProjectedMove = CameraEdges[0].ProjectedMove;
                Moving = true;
                CameraEdges[0].signal = false;
            }

            else if (CameraEdges[1].signal)
            {
                ProjectedMove = CameraEdges[1].ProjectedMove;
                Moving = true;
                CameraEdges[1].signal = false;
            }
        }

        if (Moving)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, ProjectedMove.x, Time.deltaTime * TransitionSpeed), 0f, -10f);

        float dif = ProjectedMove.x - transform.position.x;

        if (-difBound < dif && dif < difBound)
        {
            transform.position = ProjectedMove;
            Moving = false;

        }

    }
    // When player enters camera "edges" (child objects with colliders)...
    // ...understand the direction in which the player is going, and LERP transform a camera's width in that direction
    //Create an overlap space so that the player does not accidentally activate the opposing wall when it moves over to them
}