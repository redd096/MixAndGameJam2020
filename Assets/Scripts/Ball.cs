using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Ball")]
public class Ball : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float damage = 10;
    [SerializeField] float parryDamage = 100;

    [Header("Graphics")]
    [SerializeField] GameObject normalTrail = default;
    [SerializeField] GameObject bossTrail = default;

    [Header("Sound")]
    [SerializeField] AudioClip normalSound = default;
    [SerializeField] AudioClip noParryableSound = default;

    public bool BallThrowed { get; private set; }
    public float Damage => damage;

    bool isParryable;

    Rigidbody2D rb;
    Character owner;

    void OnEnable()
    {
        //ignore collision of the owner
        IgnoreCollision(true);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //default hide trail
        HideTrail();
    }

    void FixedUpdate()
    {
        //if is sleeping, remove ball throwed
        if (rb.IsSleeping())
        {
            RemoveBallThrowed(true);
            HideTrail();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit a character
        Character character = collision.gameObject.GetComponentInParent<Character>();
        if (character)
        {
            //no hit allies
            if ((owner is Player && character is Player) || (owner is Enemy && character is Enemy))
                return;

            if (BallThrowed)
            {
                RemoveBallThrowed(false);

                //do damage (or deflect or parry)
                character.HitByBall(this, isParryable);
            }

            //pick ball if no ball in hand
            if (character.CurrentBall == null)
                character.PickBall(this);
        }

        //if hit something that is not a character, remove ball throwed
        if (character == null)
        {
            RemoveBallThrowed(true);
        }
    }

    #region private API

    void RemoveBallThrowed(bool removeOwner)
    {
        //remove ball throwed
        if (BallThrowed)
        {
            BallThrowed = false;

            //if there is already a owner, be sure to not ignore collision with him
            IgnoreCollision(false);

            if (removeOwner)
                owner = null;
        }
    }

    void IgnoreCollision(bool ignore)
    {
        if (owner != null)
            if (owner.GetComponentInChildren<Collider2D>() != null)
                Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), owner.GetComponentInChildren<Collider2D>(), ignore);
    }

    void GraphicsAndSound(bool isParryable)
    {
        if (normalTrail == null || bossTrail == null)
            return;

        //active trail
        if (isParryable)
        {
            normalTrail.SetActive(true);
            bossTrail.SetActive(false);
        }
        else
        {
            bossTrail.SetActive(true);
            normalTrail.SetActive(false);
        }

        //play sound
        AudioSource audio = GetComponent<AudioSource>();
        if (audio)
        {
            if (isParryable)
                audio.clip = normalSound;
            else
                audio.clip = noParryableSound;

            audio.Play();
        }
    }

    void HideTrail()
    {
        if (normalTrail == null || bossTrail == null)
            return;

        normalTrail.SetActive(false);
        bossTrail.SetActive(false);
    }

    #endregion

    #region public API

    public void PickBall()
    {
        //stop movement
        rb.velocity = Vector2.zero;

        //deactive ball
        gameObject.SetActive(false);
    }

    public void ThrowBall(Vector2 force, Vector2 spawnPosition, Character owner, bool isParryable)
    {
        //if there is already a owner, be sure to not ignore collision with him
        IgnoreCollision(false);

        //set owner and set layer based on owner
        this.owner = owner;
        IgnoreCollision(true);

        //set spawn position and active
        transform.position = spawnPosition;
        gameObject.SetActive(true);

        GraphicsAndSound(isParryable);

        //set ball throwed and push
        BallThrowed = true;
        this.isParryable = isParryable;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Parry()
    {
        //damage owner
        owner.KillByParry(parryDamage);

        //remove ball throwed
        RemoveBallThrowed(false);
    }

    #endregion
}
