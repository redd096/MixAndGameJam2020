using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

public class Wait : State
{
    Enemy enemy;
    Coroutine wait_coroutine;

    public Wait(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy = stateMachine as Enemy;
        enemy.StopMovement();

        //start coroutine
        if (wait_coroutine == null)
            wait_coroutine = stateMachine.StartCoroutine(Wait_Coroutine());
    }

    IEnumerator Wait_Coroutine()
    {
        //wait
        yield return new WaitForSeconds(enemy.TimeBetweenPatrols);

        //start new patrol
        stateMachine.SetState(new Patrol(stateMachine));
    }
}
