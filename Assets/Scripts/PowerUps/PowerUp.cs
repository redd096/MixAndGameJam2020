using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    [Header("General")]
    [SerializeField] Sprite spritePowerUp = default;
    [SerializeField] float duration = 5;

    public Sprite SpritePowerUp => spritePowerUp;

    Coroutine deactive_Coroutine;

    //constructor
    public PowerUp(PowerUp powerUp)
    {
        spritePowerUp = powerUp.spritePowerUp;
        duration = powerUp.duration;
    }

    public virtual void ActivatePowerUp(Character character)
    {
        //start coroutine to deactivate
        if (deactive_Coroutine != null)
            character.StopCoroutine(deactive_Coroutine);

        deactive_Coroutine = character.StartCoroutine(Deactive_Coroutine(character));
    }

    public virtual void DeactivatePowerUp(Character character)
    {
        //stop coroutine
        if (deactive_Coroutine != null)
            character.StopCoroutine(deactive_Coroutine);
    }

    IEnumerator Deactive_Coroutine(Character character)
    {
        //wait, then deactive
        yield return new WaitForSeconds(duration);

        DeactivatePowerUp(character);
    }
}
