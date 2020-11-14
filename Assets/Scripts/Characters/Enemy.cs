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

    public BoxCollider2D[] AreasToMove => areasToMove;
    public float TimeBetweenPatrols => timeBetweenPatrols;
    public Ball CurrentBall => currentBall;
    public float TimeBeforeThrowBall => timeBeforeThrowBall;

    void Start()
    {
        //start patrol
        SetState(new Patrol(this));
    }

    void Update()
    {
        state?.Execution();
    }

    #region public API

    public void EnemyMovement(Vector2 direction)
    {
        //move enemy
        transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
    }

    public void EnemyThrowBall()
    {
        //throw to player
        Vector2 direction = redd096.GameManager.instance.player.transform.position - transform.position;

        ThrowBall(direction.normalized);
    }

    #endregion
}
