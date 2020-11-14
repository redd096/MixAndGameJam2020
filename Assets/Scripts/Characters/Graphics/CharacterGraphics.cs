using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Characters/Character Graphics")]
public class CharacterGraphics : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sprite;

    Character character;
    Rigidbody2D rb;

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

        //set speed
        anim?.SetFloat("Speed", rb.velocity.magnitude);
    }

    #region events

    void AddEvent()
    {
        character.OnPickBall += PickBall;
        character.OnThrowBall += OnThrowBall;
    }

    void RemoveEvent()
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
}
