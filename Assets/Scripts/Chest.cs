using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Power Up")]
    [SerializeField] PowerUp powerUp = default;

    Animator anim;

    bool addPowerUp;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit player
        Player player = collision.gameObject.GetComponentInParent<Player>();
        if(player & addPowerUp == false)
        {
            addPowerUp = true;

            //animation
            anim.SetTrigger("Open");

            PowerUp p = new PowerUp(powerUp);

            //add power up to player and UI
            player.AddPowerUp(p);
        }
    }
}
