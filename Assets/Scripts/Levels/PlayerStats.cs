using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float playerSpeedPrivate = 20f;
    private float playerSpeedMaxPrivate = 10f;
    private float playerJumpPrivate = 150f;

    public float playerSpeed { get => playerSpeedPrivate; set => playerSpeedPrivate = value; }
    public float playerSpeedMax { get => playerSpeedMaxPrivate; set => playerSpeedMaxPrivate = value; }
    public float playerJump { get => playerJumpPrivate; set => playerJumpPrivate = value; }
}
