using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

public class PickBall : State
{
    Enemy enemy;
    Ball ballToReach;

    public PickBall(StateMachine stateMachine, Ball ballToReach) : base(stateMachine)
    {
        this.ballToReach = ballToReach;
    }

    public override void Enter()
    {
        base.Enter();

        enemy = stateMachine as Enemy;
    }

    public override void Execution()
    {
        base.Execution();

        //if there is no more ball to reach, change state
        if(NoBallToReach())
        {
            ChangeState();
            return;
        }

        Vector2 direction = ballToReach.transform.position - enemy.transform.position;

        //else move to ball
        enemy.EnemyMovement(direction.normalized);
    }

    #region private API

    bool NoBallToReach()
    {
        //if ball is null, or is not active, or is not in area
        return ballToReach == null || ballToReach.gameObject.activeInHierarchy == false || BallInArea() == false;
    }

    bool BallInArea()
    {
        //not in area if null
        if (ballToReach == null)
            return false;

        //foreach area
        foreach (BoxCollider2D box in enemy.AreasToMove)
        {
            Bounds area = box.bounds;
            Vector2 min = area.center - area.extents;
            Vector2 max = area.center + area.extents;

            //if exceed left, right, down or up - not in area
            if (ballToReach.transform.position.x < min.x || ballToReach.transform.position.x > max.x || ballToReach.transform.position.y < min.y || ballToReach.transform.position.y > max.y)
                return false;
        }

        //else is in area
        return true;
    }

    #endregion

    void ChangeState()
    {
        //if enemy has the ball, throw it
        if (enemy.CurrentBall != null)
        {
            stateMachine.SetState(new ThrowBall(stateMachine));
        }
        //else back to patrol
        else
        {
            stateMachine.SetState(new Patrol(stateMachine));
        }
    }
}
