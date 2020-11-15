using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool isActiveAtStart = false;

    [Header("Power Up")]
    [SerializeField] PowerUp powerUp = default;

    [Header("Timer Before Active")]
    [SerializeField] float timerBeforeActive = 0.5f;

    Animator anim;
    
    float timer;
    bool addPowerUp;

    void OnEnable()
    {
        timer = Time.time + timerBeforeActive;
    }

    private void Awake()
    {
        if (isActiveAtStart == false)
            gameObject.SetActive(false);

        anim = GetComponentInChildren<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //do only if ended timer
        if (timer > Time.time)
            return;

        //if hit player
        Player player = collision.gameObject.GetComponentInParent<Player>();
        if(player & addPowerUp == false)
        {
            addPowerUp = true;

            //animation
            anim.SetTrigger("Open");

            PowerUp p = null;

            if (powerUp is SpeedUpCharacter)
            {
                p = ScriptableObject.CreateInstance("SpeedUpCharacter") as SpeedUpCharacter;
            }
            else if(powerUp is Shield)
            {
                p = ScriptableObject.CreateInstance("Shield") as Shield;
            }
            else
            {
                p = ScriptableObject.CreateInstance("SlowDown") as SlowDown;
            }

            p.Init(powerUp);

            //add power up to player and UI
            player.AddPowerUp(p);
        }
    }
}
