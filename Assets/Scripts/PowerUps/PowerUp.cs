using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : ScriptableObject
{
    [Header("General")]
    [SerializeField] Sprite spritePowerUp = default;
    [SerializeField] float duration = 5;

    public Sprite SpritePowerUp => spritePowerUp;

    protected Character character;
    protected Coroutine deactive_Coroutine;

    //constructor
    public PowerUp(PowerUp powerUp)
    {
        spritePowerUp = powerUp.spritePowerUp;
        duration = powerUp.duration;
    }

    void OnDestroy()
    {
        DeactivatePowerUp();
    }

    public void ActivatePowerUp(Character character)
    {
        //do only one time
        if (deactive_Coroutine == null)
        {
            //start coroutine to deactivate
            deactive_Coroutine = character.StartCoroutine(Deactive_Coroutine());

            //save character reference
            this.character = character;

            //active effect
            ActivateEffect();
        }
    }

    public void DeactivatePowerUp()
    {
        if (character == null)
            return;

        //do only one time
        if (deactive_Coroutine != null)
        {
            //stop coroutine
            character.StopCoroutine(deactive_Coroutine);
            deactive_Coroutine = null;

            //deactivate effect
            DeactivateEffect();

            //remove from game
            character.RemovePowerUp(this);
            character = null;
        }
    }

    IEnumerator Deactive_Coroutine()
    {
        //wait, then deactive
        yield return new WaitForSeconds(duration);

        DeactivatePowerUp();
    }

    #region effect

    protected virtual void ActivateEffect()
    {

    }

    protected virtual void DeactivateEffect()
    {

    }

    #endregion
}
