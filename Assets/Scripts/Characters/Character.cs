using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : redd096.StateMachine
{
    [Header("Important")]
    [SerializeField] protected float health = 100;
    [SerializeField] protected float speed = 300;

    [Header("Throw Ball")]
    [SerializeField] protected float pushForce = 5;

    protected Rigidbody2D rb;
    protected Ball currentBall;

    protected bool isMovingRight = true;
    protected bool isDead;

    public Ball CurrentBall => currentBall;
    public bool IsMovingRight => isMovingRight;
    public System.Action OnThrowBall { get; set; }
    public System.Action OnPickBall { get; set; }
    public System.Action OnParry { get; set; }
    public System.Action OnDead { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        //set if moving right
        if (isMovingRight == false && rb.velocity.x > 0.1f)
            isMovingRight = true;
        else if (isMovingRight && rb.velocity.x < -0.1f)
            isMovingRight = false;
    }

    #region protected API

    protected void ThrowBall(Vector2 direction, bool isParryable)
    {
        Vector2 ballPosition = new Vector2(transform.position.x, transform.position.y);

        //throw ball and remove reference
        currentBall.ThrowBall(direction * pushForce, ballPosition, this, isParryable);
        currentBall = null;

        //call event
        OnThrowBall?.Invoke();
    }

    protected virtual void Die()
    {
        //do only one time
        if (isDead)
            return;

        isDead = true;

        OnDead?.Invoke();

        //stop character
        enabled = false;
    }

    protected void Parry(Ball ball)
    {
        //parry
        ball.Parry();

        //pick ball
        PickBall(ball);

        //call event
        OnParry?.Invoke();
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

    #endregion

    #region public API

    public void KillByParry(float parryDamage)
    {
        GetDamage(parryDamage);
    }

    public virtual void HitByBall(Ball ball, bool isParryable)
    {
        //if we have a ball in hand
        if(currentBall)
        {
            //deflect if looking in direction of the ball (right or left)
            Vector2 direction = ball.transform.position - transform.position;
            if (isMovingRight && direction.x > 0 || !isMovingRight && direction.x < 0)
            {
                return;
            }
        }

        //else get damage
        GetDamage(ball.Damage);
    }

    public void PickBall(Ball ball)
    {
        //save reference and pick ball
        currentBall = ball;
        ball.PickBall();

        //call event
        OnPickBall?.Invoke();
    }

    public void ThrowBall()
    {
        if (currentBall)
        {
            //throw ball random to remove from character
            ThrowBall(Random.insideUnitCircle, true);
        }
    }

    #endregion
}
