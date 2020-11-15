using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Speed Up Character", fileName = "Speed Up Character")]
public class SpeedUpCharacter : PowerUp
{
    [Header("Speed Up")]
    [SerializeField] float speedToSet = 6;

    float previousSpeed;

    //constructor
    public SpeedUpCharacter(PowerUp powerUp) : base(powerUp)
    {
        SpeedUpCharacter speedUp = powerUp as SpeedUpCharacter;

        speedToSet = speedUp.speedToSet;
    }

    protected override void ActivateEffect()
    {
        base.ActivateEffect();

        //set speed
        character.SetSpeed(speedToSet, out previousSpeed);
    }

    protected override void DeactivateEffect()
    {
        base.DeactivateEffect();

        //reset speed
        character.SetSpeed(previousSpeed, out previousSpeed);
    }
}
