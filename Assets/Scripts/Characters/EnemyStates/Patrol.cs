using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

public class Patrol : State
{
    Enemy enemy;
    Vector2 positionToReach;

    Vector2 previousDirection;

    public Patrol(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy = stateMachine as Enemy;

        //set a random point to reach
        positionToReach = SetRandomPoint();
    }

    public override void Execution()
    {
        base.Execution();

        //if enemy has a ball, throw it
        if(enemy.CurrentBall != null)
        {
            stateMachine.SetState(new ThrowBall(stateMachine));
            return;
        }

        //else if there is a ball in area, stop patrol and go pick it
        Ball ball = CheckBallInArea();
        if (ball)
        {
            stateMachine.SetState(new PickBall(stateMachine, ball));
            return;
        }

        //else patrol
        Vector2 direction = positionToReach - new Vector2(enemy.transform.position.x, enemy.transform.position.y);

        //if reached point, wait for next patrol
        if(ReachedPoint(direction))
        {
            stateMachine.SetState(new Wait(stateMachine));
        }
        //else move to point
        else
        {
            enemy.Movement(direction.normalized);
            previousDirection = direction;  //save new direction
        }
    }

    #region private API

    Vector2 SetRandomPoint()
    {
        if(enemy.AreasToMove == null || enemy.AreasToMove.Length <= 0)
        {
            Debug.LogWarning("Mancano le aree in cui muovere " + enemy.name);
            return enemy.transform.position;
        }

        //select random area
        Collider2D randomBox = enemy.AreasToMove[Random.Range(0, enemy.AreasToMove.Length)];
        Bounds randomArea = randomBox.bounds;
        Vector2 min = randomArea.center - randomArea.extents;
        Vector2 max = randomArea.center + randomArea.extents;

        //get random point inside that area
        float randomX = Random.Range(min.x, max.x);
        float randomY = Random.Range(min.y, max.y);
        Vector2 randomPoint = new Vector2(randomX, randomY);

        //if there is another collider over this one, can't reach point, repeat
        if (Physics2D.OverlapBoxAll(randomPoint, Vector2.one, 0).Length > 1)
            return SetRandomPoint();

        //return random point
        return randomPoint;
    }

    Ball CheckBallInArea()
    {
        List<Ball> ballsInArea = new List<Ball>();

        //foreach area
        foreach (BoxCollider2D box in enemy.AreasToMove)
        {
            Bounds area = box.bounds;
            Vector2 min = area.center - area.extents;
            Vector2 max = area.center + area.extents;

            //foreach ball in scene
            foreach (Ball ball in Object.FindObjectsOfType<Ball>())
            {
                //if exceed left, right, down or up - continue
                if (ball.transform.position.x < min.x || ball.transform.position.x > max.x || ball.transform.position.y < min.y || ball.transform.position.y > max.y)
                    continue;

                //else if is not throwed, add to list 
                if (ball.BallThrowed == false && ballsInArea.Contains(ball) == false)
                    ballsInArea.Add(ball);
            }
        }

        //return nearest to enemy
        return ballsInArea.FindNearest(enemy.transform.position);
    }

    bool ReachedPoint(Vector2 direction)
    {
        //if reached or exceeded, end patrol
        if(direction.magnitude < Mathf.Epsilon || (direction.magnitude > previousDirection.magnitude && previousDirection.magnitude > 0))
        {
            return true;
        }

        return false;
    }

    #endregion
}
