using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public Vector3 CameraPos; //The position the camera should be at after respawning the player

    private void OnTriggerEnter2D(Collider2D subject)
    {
        if (subject.gameObject.tag == "Player") {
            PlayerController PlayerCon = subject.gameObject.GetComponent<PlayerController>();
            PlayerCon.RespawnCoords = transform.position;
            Camera.main.transform.position = CameraPos;
        }
    }
}
