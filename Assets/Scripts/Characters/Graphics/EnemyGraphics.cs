using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Characters/Enemy Graphics")]
public class EnemyGraphics : CharacterGraphics
{
    protected override void AddEvent()
    {
        base.AddEvent();

        Enemy enemy = character as Enemy;

        enemy.onRunning += OnRunning;
        enemy.onIdle += OnIdle;
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();

        Enemy enemy = character as Enemy;

        enemy.onRunning -= OnRunning;
        enemy.onIdle -= OnIdle;
    }

    protected override void Run()
    {
        //do nothing
    }

    void OnRunning()
    {
        anim?.SetBool("Running", true);
    }

    void OnIdle()
    {
        anim?.SetBool("Running", false);
    }
}
