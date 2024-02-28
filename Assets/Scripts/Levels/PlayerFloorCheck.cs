using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloorCheck : MonoBehaviour
{
    public bool playerActive;
    public bool playerGrounded;
    public bool YoyoCharge;

    private void OnTriggerStay2D(Collider2D subject)
    {
        if (subject.transform.tag == "Floor")
        {
            playerActive = true;
            playerGrounded = true;
            YoyoCharge = true;
        }
    }

    private void OnTriggerExit2D(Collider2D subject)
    {
        if (subject.transform.tag == "Floor")
        {
            playerGrounded = false;
        }
    }
}
