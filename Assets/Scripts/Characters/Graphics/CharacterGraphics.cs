using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Characters/Character Graphics")]
public class CharacterGraphics : MonoBehaviour
{
    [Header("Flip")]
    [SerializeField] bool startFlip = false;

    [Header("Speed")]
    [SerializeField] float speedToRun = 0.01f;
    [SerializeField] float speedToIdle = 0.05f;

    [Header("Hand")]
    [SerializeField] GameObject hand = default;

    [Header("Parry")]
    [SerializeField] GameObject parryObject = default;
    [SerializeField] float timeBeforeHideParry = 1;

    [Header("Explosion Dead")]
    [SerializeField] GameObject spriteToHide = default;
    [SerializeField] GameObject explosionOnDead = default;

    [Header("Shield")]
    [SerializeField] GameObject shield = default;

    protected Animator anim;
    SpriteRenderer sprite;

    protected Character character;
    Rigidbody2D rb;

    bool isRunning;
    float previousSpeed;

    Coroutine removeParry;

    protected virtual void Start()
    {
        //get references
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();

        //default graphics disabled
        hand.SetActive(false);
        parryObject.SetActive(false);
        explosionOnDead.SetActive(false);

        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }

    protected virtual void Update()
    {
        //flip or not
        sprite.flipX = !character.IsMovingRight;
        if (startFlip == true) 
            sprite.flipX = !sprite.flipX;

        //set run animation
        Run();
    }

    protected virtual void Run()
    {
        if (isRunning && rb.velocity.magnitude < speedToIdle && rb.velocity.magnitude < previousSpeed)
        {
            anim?.SetBool("Running", false);
            isRunning = false;
            previousSpeed = rb.velocity.magnitude;
        }
        else if (!isRunning && rb.velocity.magnitude > speedToRun && rb.velocity.magnitude > previousSpeed)
        {
            anim?.SetBool("Running", true);
            isRunning = true;
            previousSpeed = rb.velocity.magnitude;
        }
    }

    #region events

    protected virtual void AddEvent()
    {
        character.OnPickBall += PickBall;
        character.OnThrowBall += OnThrowBall;
        character.OnParry += OnParry;
        character.OnDead += OnDead;
        character.OnShield += OnShield;
    }

    protected virtual void RemoveEvent()
    {
        if (character == null)
            return;

        character.OnPickBall -= PickBall;
        character.OnThrowBall -= OnThrowBall;
        character.OnParry -= OnParry;
        character.OnDead -= OnDead;
        character.OnShield -= OnShield;
    }

    void PickBall()
    {
        //show hand
        hand.SetActive(true);
    }

    void OnThrowBall()
    {
        //hide hand
        hand.SetActive(false);
    }

    void OnParry()
    {
        //show parry
        parryObject.SetActive(true);

        //start coroutine to hide parry
        if (removeParry != null)
            StopCoroutine(removeParry);

        removeParry = StartCoroutine(RemoveParry());
    }

    void OnDead()
    {
        //hide sprite and active explosion
        spriteToHide.SetActive(false);
        explosionOnDead.SetActive(true);
    }

    void OnShield(bool activate)
    {
        //activate or deactivate shield
        if(shield)
        {
            shield.SetActive(activate);
        }
    }

    #endregion

    #region private API

    IEnumerator RemoveParry()
    {
        //wait, then hide parry
        yield return new WaitForSeconds(timeBeforeHideParry);

        parryObject.SetActive(false);
    }

    #endregion
}
