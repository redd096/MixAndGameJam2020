using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("MixAndGameJam2020/Characters/Enemy Graphics")]
public class EnemyGraphics : CharacterGraphics
{
    [Header("Parry")]
    [SerializeField] Slider parrySlider = default;

    Enemy enemy;

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();

        //parry default is deactive, so we reinitialize it in the update
        parrySlider.gameObject.SetActive(false);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        //if ball in hand
        if(enemy.CurrentBall)
        {
            if (parrySlider.gameObject.activeInHierarchy)
            {
                //remove slider
                parrySlider.gameObject.SetActive(false);
            }
        }
        //else
        else
        {
            //set parry slider
            parrySlider.value = enemy.parry;

            if(parrySlider.gameObject.activeInHierarchy == false)
            {
                //show slider
                parrySlider.gameObject.SetActive(true);
            }
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();

        enemy.onRunning += OnRunning;
        enemy.onIdle += OnIdle;
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();

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
