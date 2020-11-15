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

    public bool BallThrowed { get; private set; }
    public float Damage => damage;

    bool isParryable;

    Rigidbody2D rb;
    Character owner;

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
            RemoveBallThrowed();
            HideTrail();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit a character
        Character character = collision.gameObject.GetComponentInParent<Character>();
        if (character)
        {
            if (BallThrowed)
            {
                RemoveBallThrowed();
                character.HitByBall(this, isParryable);
            }

            //pick ball if no ball in hand
            if (character.CurrentBall == null)
                character.PickBall(this);
        }

        //if hit something that is not a character, remove ball throwed
        if (character == null)
        {
            RemoveBallThrowed();
        }
    }

    #region private API

    void RemoveBallThrowed()
    {
        //remove ball throwed
        if (BallThrowed)
        {
            BallThrowed = false;

            //if there is already a owner, be sure to not ignore collision with him
            if (owner != null)
                Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), owner.GetComponentInChildren<Collider2D>(), false);
        }
    }

    void ShowTrail(bool isParryable)
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
        if(this.owner != null)
        {
            Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), this.owner.GetComponentInChildren<Collider2D>(), false);
        }

        //set owner and set layer based on owner
        this.owner = owner;
        Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), owner.GetComponentInChildren<Collider2D>(), true);

        //set spawn position and active
        transform.position = spawnPosition;
        gameObject.SetActive(true);

        ShowTrail(isParryable);

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
        RemoveBallThrowed();
    }

    #endregion
}
