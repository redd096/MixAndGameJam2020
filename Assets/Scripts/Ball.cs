using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool BallThrowed { get; private set; }

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //if hit something, remove ball throwed
        if (BallThrowed)
            BallThrowed = false;
    }

    #region public API

    public void PickBall()
    {
        //deactive ball
        gameObject.SetActive(false);
    }

    public void ThrowBall(Vector3 force)
    {
        //active ball
        gameObject.SetActive(true);

        //set ball throwed and push
        BallThrowed = true;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    #endregion
}
