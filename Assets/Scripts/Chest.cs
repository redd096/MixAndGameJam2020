using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Power Up")]
    [SerializeField] PowerUp powerUp = default;

    bool addPowerUp;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit player
        Player player = collision.gameObject.GetComponentInParent<Player>();
        if(player & addPowerUp == false)
        {
            addPowerUp = true;

            //add power up to player and UI
            player.AddPowerUp(powerUp);
            redd096.GameManager.instance.uiManager.AddPowerUp(powerUp.SpritePowerUp);

            //remove this chest
            gameObject.SetActive(false);
        }
    }
}
