using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : redd096.StateMachine
{
    [Header("Important")]
    [SerializeField] protected float health = 100;
    [SerializeField] protected float speed = 300;
    [SerializeField] bool moveWithAcceleration = true;

    [Header("Throw Ball")]
    [SerializeField] protected float pushForce = 5;

    protected Rigidbody2D rb;
    protected Ball currentBall;

    protected bool isMovingRight = true;

    public bool IsMovingRight => isMovingRight;
    public System.Action OnThrowBall { get; set; }
    public System.Action OnPickBall { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        //set if moving right
        if (isMovingRight == false && rb.velocity.x > Mathf.Epsilon)
            isMovingRight = true;
        else if (isMovingRight && rb.velocity.x < Mathf.Epsilon)
            isMovingRight = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit by a ball, check if is throwed
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball)
        {
            if (ball.BallThrowed)
            {
                //check this character is not who throwed the ball
                if (ball.Owner != this)
                {
                    //hit by ball
                    HitByBall(ball);
                }
            }

            //pick ball if no ball in hand
            if (currentBall == null)
                PickBall(ball);
        }
    }

    #region protected API

    protected void Movement(Vector2 direction)
    {
        if (moveWithAcceleration)
        {
            //add force
            rb.AddForce(direction * speed);
        }
        else
        {
            //set velocity
            rb.velocity = direction * speed;
        }
    }

    protected void ThrowBall(Vector2 direction)
    {
        Vector2 ballPosition = new Vector2(transform.position.x, transform.position.y) + direction;

        //throw ball and remove reference
        currentBall.ThrowBall(direction * pushForce, ballPosition, this);
        currentBall = null;

        //call event
        OnThrowBall?.Invoke();
    }

    protected virtual void HitByBall(Ball ball)
    {
        //can deflect if current ball != null
        //but is not in Character, cause there is a collider on the ball

        //get damage
        GetDamage(ball.Damage);
    }

    protected void PickBall(Ball ball)
    {
        //save reference and pick ball
        currentBall = ball;
        ball.PickBall();

        //call event
        OnPickBall?.Invoke();
    }

    #endregion

    #region private API

    void GetDamage(float damage)
    {
        //remove health
        health -= damage;

        //check death
        if (health <= 0)
            Die();
    }

    void Die()
    {
        //TODO die
    }

    #endregion

    #region public API

    public void KillByParry()
    {
        Die();
    }

    #endregion
}
