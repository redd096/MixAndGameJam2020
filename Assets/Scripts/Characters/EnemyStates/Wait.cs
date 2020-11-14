using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

public class Wait : State
{
    Coroutine wait_coroutine;

    public Wait(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //start coroutine
        if(wait_coroutine == null)
            wait_coroutine = stateMachine.StartCoroutine(Wait_Coroutine());
    }

    IEnumerator Wait_Coroutine()
    {
        Enemy enemy = stateMachine as Enemy;

        //wait
        yield return new WaitForSeconds(enemy.TimeBetweenPatrols);

        //start new patrol
        stateMachine.SetState(new Patrol(stateMachine));
    }
}
