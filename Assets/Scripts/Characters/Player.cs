﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Characters/Player")]
public class Player : Character
{
    [Header("Parry")]
    [SerializeField] float parry = 0.2f;
    [SerializeField] float delayParry = 0.1f;

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

        //throw ball or parry (if ball in hand or not)
        if(currentBall)
            InputThrowBall(Input.GetButtonDown("Action"), playerDirection);
        else
            StartParryTimer(Input.GetButtonDown("Action"));

        //active power up
        ActivePowerUp(Input.GetButtonDown("Spell1"), 0);
        ActivePowerUp(Input.GetButtonDown("Spell2"), 1);
        ActivePowerUp(Input.GetButtonDown("Spell3"), 2);

        PauseGame(Input.GetButtonDown("Pause"));
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

    void InputThrowBall(bool inputThrow, Vector2 playerDirection)
    {
        //if press input throw ball and there is a ball in hand
        if (inputThrow && currentBall != null)
        {
            ThrowBall(playerDirection, true);
        }
    }

    void StartParryTimer(bool inputParry)
    {
        //if press input and no ball in hand, and parry is not in delay
        if (inputParry && currentBall == null && Time.time > parryTimer + delayParry)
        {
            //set timer parry
            parryTimer = Time.time + parry;
        }
    }

    void PauseGame(bool inputPause)
    {
        //if press escape or start, pause or resume game
        if (inputPause)
        {
            if (Time.timeScale <= 0)
                redd096.SceneLoader.instance.ResumeGame();
            else
                redd096.SceneLoader.instance.PauseGame();
        }
    }

    #endregion

    #region hit by ball + parry

    public override void HitByBall(Ball ball, bool isParryable)
    {
        if (isParryable)
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
        }

        //else normal hit by ball
        base.HitByBall(ball, isParryable);
    }

    #endregion

    protected override void Die()
    {
        //do only one time
        if (isDead)
            return;

        base.Die();

        //invoke end game
        redd096.GameManager.instance.Invoke("EndGame", 1);
    }

    #endregion
}
