using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MixAndGameJam2020/Ball")]
public class Ball : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float damage = 10;

    public bool BallThrowed { get; private set; }
    public float Damage => damage;

    Rigidbody2D rb;
    Character owner;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //if is sleeping, remove ball throwed
        if (rb.IsSleeping())
            RemoveBallThrowed();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit a character
        Character character = collision.gameObject.GetComponentInParent<Character>();
        if (character)
        {
            if (BallThrowed)
            {
                //check this character is not who throwed the ball
                if (owner != character)
                {
                    //hit by ball
                    RemoveBallThrowed();
                    character.HitByBall(this);
                }
            }

            //pick ball if no ball in hand (and this is not the ball we throwed)
            if (owner != character && character.CurrentBall == null)
                character.PickBall(this);
        }

        //if hit something that is not a character, remove ball throwed
        if (collision.gameObject.GetComponentInParent<Character>() == null)
            RemoveBallThrowed();
    }

    void RemoveBallThrowed()
    {
        //remove ball throwed
        if (BallThrowed)
        {
            BallThrowed = false;

            //if there is already a owner, be sure to not ignore collision with him
            if (owner != null)
                Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), owner.GetComponentInChildren<Collider2D>(), false);

            //remove owner
            owner = null;
        }
    }

    #region public API

    public void PickBall()
    {
        //stop movement
        rb.velocity = Vector2.zero;

        //deactive ball
        gameObject.SetActive(false);
    }

    public void ThrowBall(Vector2 force, Vector2 spawnPosition, Character owner)
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

        //set ball throwed and push
        BallThrowed = true;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Parry()
    {
        //damage owner
        owner.KillByParry();

        //remove ball throwed
        RemoveBallThrowed();
    }

    #endregion
}
