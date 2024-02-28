using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : GravityController //Inherits from GravityController
{
    void Start()
    {
        PivotTrans = transform.GetChild(0).GetComponent<Transform>();
    }

    private void OnTriggerStay2D(Collider2D subject)
    {
        if (subject.gameObject.tag == "PlayerCore")
        {

            Vector3 PlayerPos = PlayerTrans.position;

            DirVect = PivotTrans.position - PlayerPos;

            float DirVecMag = (float)Math.Sqrt(Math.Pow(DirVect.x, 2) + Math.Pow(DirVect.y, 2));

            float opp = PlayerPos.x - PivotTrans.position.x;
            float adj = PivotTrans.position.y - PlayerPos.y;

            float PivotAngle = (float)(Math.Atan(opp / adj) * (180f / Math.PI));

            Vector2 PivotGravity = new Vector2(GravityConst * (DirVect.x / DirVecMag), GravityConst * (DirVect.y / DirVecMag));

            if (PivotTrans.position.y < PlayerTrans.position.y)
            {
                PivotAngle += 180;
            }

            PlayerCon.InfluencedGravity = PivotGravity;
            PlayerCon.InfluencedAngle = PivotAngle;
        }
    }
}
