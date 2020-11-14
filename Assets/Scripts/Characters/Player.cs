using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Characters/Player")]
public class Player : Character
{
    [Header("Dash")]
    [SerializeField] bool canDash = true;
    [SerializeField] float dash = 180;

    [Header("Parry")]
    [SerializeField] float parry = 0.2f;

    float parryTimer;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //movement (add force, so is not related to player direction, but only input)
        Movement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void Update()
    {
        //set player direction (if no input, is right or left. Necessary to dash or throw ball in oblique or vertical and not only right or left)
        Vector2 playerDirection = SetPlayerDirection(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //if can dash, dash in direction
        if (canDash)
        {
            Dash(Input.GetButtonDown("Dash"), playerDirection);
        }

        //throw ball or parry (if ball in hand or not)
        if(currentBall)
            InputThrowBall(Input.GetButtonDown("Action"), playerDirection);
        else
            StartParryTimer(Input.GetButtonDown("Action"));
    }

    #region private API

    #region fixed update

    void Movement(float horizontal, float vertical)
    {
        //direction by input
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        //set velocity
        rb.velocity = direction * speed;
    }

    #endregion

    #region update

    Vector2 SetPlayerDirection(float horizontal, float vertical)
    {
        //get direction by input
        Vector2 playerDirection = new Vector2(horizontal, vertical);

        //if no input, direction is right or left
        if (Mathf.Abs(playerDirection.magnitude) <= Mathf.Epsilon)
        {
            playerDirection = isMovingRight ? Vector2.right : Vector2.left;
        }

        return playerDirection;
    }

    void Dash(bool inputDash, Vector2 playerDirection)
    {
        //if press input
        if (inputDash)
        {
            //dash in direction
            rb.AddForce(playerDirection * dash, ForceMode2D.Impulse);
        }
    }

    void InputThrowBall(bool inputThrow, Vector2 playerDirection)
    {
        //if press input throw ball and there is a ball in hand
        if (inputThrow && currentBall != null)
        {
            ThrowBall(playerDirection);
        }
    }

    void StartParryTimer(bool inputParry)
    {
        //if press input and no ball in hand
        if (inputParry && currentBall == null)
        {
            //set timer parry
            parryTimer = Time.time + parry;
        }
    }

    #endregion

    #region hit by ball + parry

    public override void HitByBall(Ball ball)
    {
        //if parry timer and no ball in hand
        if (Time.time < parryTimer && currentBall == null)
        {
            //parry if looking in direction of the ball (right or left)
            Vector2 direction = ball.transform.position - transform.position;
            if (isMovingRight && direction.x > 0 || !isMovingRight && direction.x < 0)
            {
                Parry(ball);

                return;
            }
        }

        //else normal hit by ball
        base.HitByBall(ball);
    }

    void Parry(Ball ball)
    {
        Debug.Log("parry riuscito");

        //parry
        ball.Parry();

        //pick ball
        PickBall(ball);
    }

    #endregion

    #endregion
}
