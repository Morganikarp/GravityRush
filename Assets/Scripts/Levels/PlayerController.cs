using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : PlayerStats
{

    Rigidbody2D rb2d;
    PlayerFloorCheck fc;
    PlayerProximityCheck pc;

    float standardGravScale = 5f;
    public bool withinGravityField;
    public Vector3 InfluencedGravity;
    public float InfluencedAngle;
    Vector3 StandardGravity = new Vector3(0f, -9.81f, 0f);

    public Transform ForwardTrans;
    public Transform UpTrans;

    float InputMove;

    Vector3 PlayerScale;

    float RopeDuration = 4f;
    bool JumpInput = false;
    bool ThrowYoyo = false;

    public float PlayerVelocityMag;
    public float AdjustedVelocityMag;
    float MinVelocityMag = 0.15f;
    Vector3 YoyoPos;

    bool FirstPass;
    bool ClockwiseSwing;
    public bool Rising;
    public bool LeftOfYoyo;
    bool YoyoDismount = false;

    float YoyoBoost = 100f;

    public Vector3 RespawnCoords;

    float rateOfChange = 0.995f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        fc = transform.GetChild(0).GetComponent<PlayerFloorCheck>();
        pc = transform.GetChild(1).GetComponent<PlayerProximityCheck>();

        fc.playerActive = false;

        ForwardTrans = transform.GetChild(2).transform;
        UpTrans = transform.GetChild(3).transform;

        withinGravityField = false;

        PlayerScale = transform.localScale;

        pc.Dead = false;

        if ((SceneManager.GetActiveScene().buildIndex - 2) == 0)
        {
            pc.YoyoAcquired = false;
        }
        else
        {
            pc.YoyoAcquired = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pc.Dead && fc.playerActive)
        {
            if (Input.GetMouseButtonDown(0) && !ThrowYoyo && !fc.playerGrounded && fc.YoyoCharge && pc.YoyoAcquired)
            {
                ThrowYoyo = true;
                FirstPass = false;
                Rising = true;
                PlayerVelocityMag = rb2d.velocity.magnitude;
                AdjustedVelocityMag = rb2d.velocity.magnitude;

                rb2d.gravityScale = 0f;

                YoyoPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

                if (YoyoPos.x >= transform.position.x)
                {
                    transform.localScale = PlayerScale;
                    ClockwiseSwing = false;
                }

                else if (YoyoPos.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-PlayerScale.x, PlayerScale.y, 1);
                    ClockwiseSwing = true;
                }

                StartCoroutine("RopeBreak");
            }

            if (Input.GetKeyDown(KeyCode.Space) && fc.playerGrounded)
            {
                JumpInput = true;
            }

            if (fc.playerGrounded)
            {
                pc.WallHit = false;
            }


            if (ThrowYoyo)
            {
                Yoyo();

                if (Input.GetKeyDown(KeyCode.Space))
                {

                    rb2d.gravityScale = 5f;

                    transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

                    ThrowYoyo = false;

                    YoyoDismount = true;
                }
            }

            Gravity();

            InputMove = Input.GetAxisRaw("Horizontal");
        }

        else if (pc.Dead)
        {
            transform.position = RespawnCoords;
            pc.Dead = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator RopeBreak()
    {
        yield return new WaitForSeconds(RopeDuration);
        
        if (ThrowYoyo)
        {
            rb2d.gravityScale = standardGravScale;

            transform.rotation = Quaternion.Euler(Vector3.zero);

            ThrowYoyo = false;

            YoyoDismount = true;
        }
    }

    void FixedUpdate()
    {
        if (!ThrowYoyo)
        {
            Movement(playerSpeed);
        }

        if (YoyoDismount)
        {
            Vector2 ForwardDir = ForwardTrans.position - transform.position;
            rb2d.AddForce(new Vector2(ForwardDir.x * YoyoBoost, ForwardDir.y), ForceMode2D.Impulse);
            YoyoDismount = false;
            fc.YoyoCharge = false;
        }

        if (JumpInput)
        {
            Jump();
            JumpInput = false;
        }
    }

    void Movement(float Speed)
    {
        
        Vector3 DirVect = ForwardTrans.position - transform.position;
        float DirVectMag = Mathf.Sqrt(Mathf.Pow(DirVect.x, 2) + Mathf.Pow(DirVect.y, 2));
        Vector2 jumpForceVector = new Vector2(Speed * (DirVect.x / DirVectMag), Speed * (DirVect.y / DirVectMag));
        switch (InputMove)
        {
            case 1f:
                transform.localScale = PlayerScale;
                rb2d.AddForce(jumpForceVector, ForceMode2D.Impulse);
                break;
            case -1f:
                transform.localScale = new Vector3(-PlayerScale.x, PlayerScale.y, 1);
                rb2d.AddForce(jumpForceVector, ForceMode2D.Impulse);
                break;
            case 0f:
                if (fc.YoyoCharge)
                {
                    rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                }
                break;
        }


        //transform.position += InputVect * 0.005f * DirVect;

            //rb2d.AddForce((InputVect * Speed) * DirVect, ForceMode2D.Impulse);

            //if (rb2d.velocity.magnitude >= Speed || rb2d.velocity.magnitude <= -Speed)
            //{
            //    rb2d.velocity = new Vector2(rb2d.velocity.x / Speed, rb2d.velocity.y / Speed);
            //}

        if (rb2d.velocity.x >= playerSpeedMax)
        {
            rb2d.velocity = new Vector2(playerSpeedMax, rb2d.velocity.y);
        }

        else if (rb2d.velocity.x <= -playerSpeedMax)
        {
            rb2d.velocity = new Vector2(-playerSpeedMax, rb2d.velocity.y);
        }

        else if (rb2d.velocity.y >= playerSpeedMax)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, playerSpeedMax);
        }

        else if (rb2d.velocity.y <= -playerSpeedMax)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -playerSpeedMax);
        }

        //if (InputVect.x == 0f)
        //{
        //    rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        //}
    }

    void Yoyo()
    {

        Vector3 YoyoVec = YoyoPos - transform.position; //Creates direction vector from pivot to player

        if (YoyoPos.x >= transform.position.x) //Learns if the player started before or after the pivot
        {
            LeftOfYoyo = true;
        }

        else
        {
            LeftOfYoyo = false;
        }

        float PivotAngle = (float)(Mathf.Acos(YoyoVec.y / YoyoVec.magnitude) * (180f / Mathf.PI)); // Gets the degree seperation of the player from the pivot point

        if (YoyoVec.x > 0f)
        {
            PivotAngle = -PivotAngle;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, PivotAngle));

        Vector2 ForwardVec = ForwardTrans.position - transform.position; //Creates direction vector going forward from the player relative to rotation

        float ForwardVecMag = Mathf.Sqrt(Mathf.Pow(ForwardVec.x, 2) + Mathf.Pow(ForwardVec.y, 2));

        if (!FirstPass) //If the Player hasn't swung past the pivot yet
        {
            Vector2 SwingForce = new Vector2(PlayerVelocityMag * (ForwardVec.x / ForwardVecMag), PlayerVelocityMag * (ForwardVec.y / ForwardVecMag)); //Determines swing velocity vector using the velocity the player entered with

            rb2d.velocity = SwingForce;

            if (LeftOfYoyo && ClockwiseSwing)
            {
                FirstPass = true;
                Rising = true;
            }

            else if (!LeftOfYoyo && !ClockwiseSwing)
            {
                FirstPass = true;
                Rising = true;
            }
        }

        else
        {
            switch (Rising)
            {
                case true:
                    AdjustedVelocityMag *= rateOfChange;
                    break;
                case false:
                    AdjustedVelocityMag /= rateOfChange;
                    break;
            }

            Vector2 SwingForce = new Vector2(AdjustedVelocityMag * (ForwardVec.x / ForwardVecMag), AdjustedVelocityMag * (ForwardVec.y / ForwardVecMag));  //Determines swing velocity vector using the players rapidly building/deterioating velocity

            if (pc.WallHit) //If the player hits a wall, turn them around and main velocity
            {
                transform.localScale = new Vector3(-transform.localScale.x, PlayerScale.y, 1);
                pc.WallHit = false;
                Rising = !Rising;
            }

            rb2d.velocity = SwingForce;


            switch (Rising)
            {
                case true:
                    if (AdjustedVelocityMag <= MinVelocityMag && AdjustedVelocityMag >= -MinVelocityMag)
                    {
                        Rising = false;

                        transform.localScale = new Vector3(-transform.localScale.x, PlayerScale.y, 1);
                    }
                    break;
                case false:
                    if (AdjustedVelocityMag >= PlayerVelocityMag)
                    {
                        AdjustedVelocityMag = PlayerVelocityMag;

                        Rising = true;
                    }

                    else if (AdjustedVelocityMag < -PlayerVelocityMag)
                    {
                        AdjustedVelocityMag = -PlayerVelocityMag;
                        Rising = true;
                    }
                    break;
            }

        }
    }

    void Jump()
    {
        rb2d.AddForce(new Vector2(0f, playerJump), ForceMode2D.Impulse);
        Physics2D.gravity = StandardGravity;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void Gravity()
    {

        if (withinGravityField && fc.playerGrounded)
        {
            Physics2D.gravity = InfluencedGravity;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, InfluencedAngle));
        }

        if (!withinGravityField && !fc.playerGrounded)
        {
            //Physics2D.gravity = StandardGravity;
            //transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        
    }
}