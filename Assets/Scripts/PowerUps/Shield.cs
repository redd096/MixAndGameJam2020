using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Shield", fileName = "Shield")]
public class Shield : PowerUp
{
    [Header("Shield")]
    [SerializeField] float health = 100;

    public override void Init(PowerUp powerUp)
    {
        base.Init(powerUp);

        Shield shield = powerUp as Shield;

        health = shield.health;
    }

    protected override void ActivateEffect()
    {
        base.ActivateEffect();

        //activate shield
        character.SetShield(true);

        //add event
        character.OnGetDamage += OnGetDamage;
    }

    protected override void DeactivateEffect()
    {
        base.DeactivateEffect();

        //deactivate shield
        character.SetShield(false);

        //remove event
        character.OnGetDamage -= OnGetDamage;
    }

    void OnGetDamage(float damage)
    {
        //get damage
        health -= damage;

        //if end health, deactivate before time
        if (health <= 0)
            DeactivatePowerUp();
    }
}
