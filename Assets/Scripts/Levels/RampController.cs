using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampController : GravityController //Inherits from GravityController
{
    void Start()
    {
        PivotTrans = transform.GetChild(0).GetComponent<Transform>();
    }

    private void OnTriggerStay2D(Collider2D subject)
    {
        if (subject.gameObject.tag == "PlayerCore") //If the Player's center point enters the ramps field
        {

            Vector3 PlayerPos = PlayerTrans.position;

            DirVect = PlayerPos - PivotTrans.position; //Directional vector going towards the Player from the pivot

            if (Slope)
            {
                DirVect = PivotTrans.position - PlayerPos; //Directional vector going towards the pivot from the player
            }

            float DirVecMag = (float)Math.Sqrt(Math.Pow(DirVect.x, 2) + Math.Pow(DirVect.y, 2)); //Finds the magnitude of the direction vector

            float opp = PlayerPos.x - PivotTrans.position.x; //Opposing line magnitude
            float adj = PivotTrans.position.y - PlayerPos.y; //Adjacent line magnitude

            float PivotAngle = (float)(Math.Atan(opp / adj) * (180f / Math.PI)); //Trigonometric equation to find the degree seperation of the player from the start of the ramp

            if (Slope)
            {
                PivotAngle += 180; //If the ramp is a slope, the pivot is in a different spot. This adjustment accounts for that
            }

            Vector2 PivotGravity = new Vector2(GravityConst * (DirVect.x / DirVecMag), GravityConst * (DirVect.y / DirVecMag)); //Uses the magnitude of the direction vector to find it as a unit vector, and then multiplies that by the gravity const to find the force vector of gravity

            if (PivotTrans.position.y < PlayerTrans.position.y)
            {
                PivotAngle += 180; //If the player goes upside down, adjust the pivot angle to account for that
            }

            PlayerCon.InfluencedGravity = PivotGravity;
            PlayerCon.InfluencedAngle = PivotAngle;
        }
    }
}