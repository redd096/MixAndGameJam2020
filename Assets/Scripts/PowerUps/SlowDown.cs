using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Slow Down", fileName = "Slow Down")]
public class SlowDown : PowerUp
{
    [Header("Slow Down")]
    [SerializeField] float timeToSet = 0.5f;

    Coroutine slowDown_Coroutine;

    //constructor
    public override void Init(PowerUp powerUp)
    {
        base.Init(powerUp);

        SlowDown slowDown = powerUp as SlowDown;

        timeToSet = slowDown.timeToSet;
    }

    protected override void ActivateEffect()
    {
        base.ActivateEffect();

        //start power up coroutine
        if (slowDown_Coroutine != null)
            character.StopCoroutine(slowDown_Coroutine);

        slowDown_Coroutine = character.StartCoroutine(SlowDown_Coroutine());
    }

    protected override void DeactivateEffect()
    {
        base.DeactivateEffect();

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
