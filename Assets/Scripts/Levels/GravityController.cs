using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public Transform PlayerTrans;
    public PlayerController PlayerCon;

    public Transform PivotTrans;

    public Vector2 DirVect; //Directional Vector between player and pivot

    public bool Slope; //Determines if the surface is convex or concave

    public float GravityConst = 9.81f;

    private void OnTriggerEnter2D(Collider2D subject)
    {
        switch (subject.gameObject.tag)
        {
            case "Player":
                PlayerTrans = subject.GetComponent<Transform>();
                PlayerCon = subject.GetComponent<PlayerController>();
                break;
            case "PlayerCore":
                PlayerCon.withinGravityField = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D subject)
    {
        if (subject.gameObject.tag == "PlayerCore")
        {
            PlayerCon.withinGravityField = false;
        }
    }

}
