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
    public Character Owner => owner;

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
        //if hit something, remove ball throwed
        RemoveBallThrowed();
    }

    void RemoveBallThrowed()
    {
        //remove ball throwed
        if (BallThrowed)
        {
            BallThrowed = false;
            gameObject.layer = LayerMask.NameToLayer("Ball");
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
        //set owner and set layer based on owner
        this.owner = owner;
        gameObject.layer = owner is Player ? LayerMask.NameToLayer("No Hit Player") : LayerMask.NameToLayer("No Hit Enemy");

        //set spawn position and active
        transform.position = spawnPosition;
        gameObject.SetActive(true);

        //set ball throwed and push
        BallThrowed = true;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Parry()
    {
        //remove ball throwed
        BallThrowed = false;

        //damage owner
        owner.KillByParry();
    }

    #endregion
}
