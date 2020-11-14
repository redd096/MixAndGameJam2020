using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float health = 100;
    [SerializeField] float speed = 5;

    [Header("Dash")]
    [SerializeField] bool canDash = true;
    [SerializeField] float dash = 10;

    Rigidbody2D rb;
    Ball currentBall;

    bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Update()
    {
        if (canDash)
        {
            Dash(Input.GetButtonDown("Dash"), Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if hit by a ball
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball)
        {
            if(ball.BallThrowed)
            {
                //hit by ball
                HitByBall(ball);
            }
            else
            {
                //pick ball
                PickBall(ball);
            }
        }
    }

    #region private API

    void Movement(float horizontal, float vertical)
    {
        //direction by input
        Vector2 direction = new Vector2(horizontal, vertical);

        //add force
        rb.AddForce(direction * speed);
    }

    void Dash(bool inputDash, float horizontal, float vertical)
    {
        //if pressed input
        if(inputDash)
        {
            //get direction by input
            Vector2 direction = new Vector2(horizontal, vertical);

            //if no input, direction is right or left
            if(Mathf.Abs(direction.magnitude) <= Mathf.Epsilon)
            {
                direction = movingRight ? Vector2.right : Vector2.left;
            }

            //dash in direction
            rb.AddForce(direction * dash);
        }
    }

    void HitByBall(Ball ball)
    {
        //can parry if no current ball
        //else get damage


        //can deflect if current ball != null
        //but is not in Player, cause there is a collider on the ball
    }

    void PickBall(Ball ball)
    {
        //save reference and pick ball
        currentBall = ball;
        ball.PickBall();

        //TODO change sprite animation
    }

    #endregion
}
