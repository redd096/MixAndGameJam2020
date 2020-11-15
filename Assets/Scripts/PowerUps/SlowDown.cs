using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : PowerUp
{
    [Header("Slow Down")]
    [SerializeField] float timeToSet = 0.5f;

    Coroutine slowDown_Coroutine;

    //constructor
    public SlowDown(PowerUp powerUp) : base(powerUp)
    {
        SlowDown slowDown = powerUp as SlowDown;

        timeToSet = slowDown.timeToSet;
    }

    public override void ActivatePowerUp(Character character)
    {
        base.ActivatePowerUp(character);

        //start power up coroutine
        if (slowDown_Coroutine != null)
            character.StopCoroutine(slowDown_Coroutine);

        slowDown_Coroutine = character.StartCoroutine(SlowDown_Coroutine());
    }

    public override void DeactivatePowerUp(Character character)
    {
        base.DeactivatePowerUp(character);

        //stop power up coroutine
        if (slowDown_Coroutine != null)
            character.StopCoroutine(slowDown_Coroutine);

        //if not paused, reset time scale
        if (Time.timeScale > 0)
            Time.timeScale = 1;
    }

    IEnumerator SlowDown_Coroutine()
    {
        while(true)
        {
            //if not paused, set time scale
            if (Time.timeScale > 0 && Time.timeScale > timeToSet)
                Time.timeScale = timeToSet;

            yield return null;
        }
    }
}
