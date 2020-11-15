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

    public BoxCollider2D[] AreasToMove => areasToMove;
    public float TimeBetweenPatrols => timeBetweenPatrols;
    public float TimeBeforeThrowBall => timeBeforeThrowBall;

    public System.Action onRunning { get; set; }
    public System.Action onIdle { get; set; }

    void Start()
    {
        //start patrol
        SetState(new Patrol(this));
    }

    void Update()
    {
        state?.Execution();
    }

    protected override void FixedUpdate()
    {
        //do nothing
    }

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
        //throw to player
        Vector2 direction = redd096.GameManager.instance.player.transform.position - transform.position;

        bool isParryable = isBoss ? Random.Range(0, 100) > percentageNotParryable : true;

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
