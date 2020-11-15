using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Characters/Enemy")]
public class Enemy : Character
{
    [Header("Patrol")]
    [SerializeField] BoxCollider2D[] areasToMove = default;
    [SerializeField] float timeBetweenPatrols = 2;
    [SerializeField] float timeBeforeThrowBall = 1;

    [Header("Boss")]
    [SerializeField] bool isBoss = false;
    [SerializeField][Range(0, 100)]  int percentageNotParryable = 70;

    [Header("Parry")]
    [SerializeField] [Range(0, 100)] int minParry = 25;
    [SerializeField] [Range(0, 100)] int maxParry = 75;
    [SerializeField] float timeSlider = 1;

    public BoxCollider2D[] AreasToMove => areasToMove;
    public float TimeBetweenPatrols => timeBetweenPatrols;
    public float TimeBeforeThrowBall => timeBeforeThrowBall;
    public int MinParry => minParry;
    public int MaxParry => maxParry;

    public System.Action onRunning { get; set; }
    public System.Action onIdle { get; set; }

    public float parry { get; private set; }
    Coroutine parry_Coroutine;

    void Start()
    {
        //start patrol
        SetState(new Patrol(this));
    }

    void Update()
    {
        state?.Execution();

        //if pick the ball, start coroutine
        StartParry();
    }

    protected override void FixedUpdate()
    {
        //do nothing
    }

    #region hit by ball + parry

    public override void HitByBall(Ball ball, bool isParryable)
    {
        if (isParryable)
        {
            //if parry inside min and max, and no ball in hand
            if (parry > minParry && parry < maxParry && currentBall == null)
            {
                Parry(ball);

                return;
            }
        }

        //else normal hit by ball
        base.HitByBall(ball, isParryable);
    }

    void StartParry()
    {
        //if no ball in hand, start coroutine
        if (currentBall == null && parry_Coroutine == null)
        {
            parry_Coroutine = StartCoroutine(Parry_Coroutine());
        }
        //if pick the ball, stop coroutine and parry at 0
        else if (currentBall && parry_Coroutine != null)
        {
            StopCoroutine(parry_Coroutine);
            parry_Coroutine = null;

            parry = 0;
        }
    }

    IEnumerator Parry_Coroutine()
    {
        while (true)
        {
            float delta = 0;
            //move to right
            while (delta < 1)
            {
                delta += Time.deltaTime / timeSlider;
                parry = Mathf.Lerp(0, 100, delta);

                yield return null;
            }
            //move to left
            while (delta > 0)
            {
                delta -= Time.deltaTime / timeSlider;
                parry = Mathf.Lerp(0, 100, delta);

                yield return null;
            }
        }
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

    #region public API

    public void StopMovement()
    {
        onIdle?.Invoke();
    }

    public void Movement(Vector2 direction)
    {
        onRunning?.Invoke();

        //set if moving right
        if (isMovingRight == false && direction.x > 0.1f)
            isMovingRight = true;
        else if (isMovingRight && direction.x < -0.1f)
            isMovingRight = false;

        //move enemy
        transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
    }

    public void EnemyThrowBall()
    {
        //if is boss, there a percentage is not parryable
        bool isParryable = isBoss ? Random.value > percentageNotParryable / 100 : true;

        //throw to player
        Vector2 direction = redd096.GameManager.instance.player.transform.position - transform.position;

        ThrowBall(direction.normalized, isParryable);
    }

    public void LookToPlayer()
    {
        Vector3 direction = redd096.GameManager.instance.player.transform.position - transform.position;

        //set if moving right based on direction to player
        if (isMovingRight == false && direction.x > 0.1f)
            isMovingRight = true;
        else if (isMovingRight && direction.x < -0.1f)
            isMovingRight = false;
    }

    #endregion
}
