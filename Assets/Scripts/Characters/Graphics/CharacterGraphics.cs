﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Characters/Character Graphics")]
public class CharacterGraphics : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float speedToRun = 0.01f;
    [SerializeField] float speedToIdle = 0.05f;

    protected Animator anim;
    SpriteRenderer sprite;

    protected Character character;
    Rigidbody2D rb;

    bool isRunning;
    float previousSpeed;

    void Start()
    {
        //get references
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();

        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }

    void Update()
    {
        //flip or not
        sprite.flipX = !character.IsMovingRight;

        //set run animation
        Run();
    }

    #region events

    protected virtual void AddEvent()
    {
        character.OnPickBall += PickBall;
        character.OnThrowBall += OnThrowBall;
    }

    protected virtual void RemoveEvent()
    {
        character.OnPickBall -= PickBall;
        character.OnThrowBall -= OnThrowBall;
    }

    void PickBall()
    {
        anim?.SetTrigger("PickBall");
    }

    void OnThrowBall()
    {
        anim?.SetTrigger("ThrowBall");
    }

    #endregion

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
}