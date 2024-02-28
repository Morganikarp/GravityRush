using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProximityCheck : MonoBehaviour
{

    public bool YoyoAcquired;
    public bool WallHit = false;
    public bool Dead = false;

    private void OnTriggerEnter2D(Collider2D subject)
    {
        switch (subject.transform.tag)
        {
            case "Floor":
                WallHit = true;
                break;
            case "Hazard":
                Dead = true;
                break;
            case "YoyoItem":
                YoyoAcquired = true;
                Destroy(subject.gameObject);
                break;
        }
    }

}
