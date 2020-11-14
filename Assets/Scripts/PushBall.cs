using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBall : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float timeBeforePush = 3;
    [SerializeField] float pushForce = 2;

    Dictionary<Ball, Coroutine> balls = new Dictionary<Ball, Coroutine>();

    void OnTriggerEnter2D(Collider2D other)
    {
        //if hit ball, add to list and start coroutine
        Ball ball = other.GetComponentInParent<Ball>();
        if (ball)
        {
            if (!balls.ContainsKey(ball))
                balls.Add(ball, StartCoroutine(Timer_BeforePush(ball)));   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if ball exit from trigger, remove from the list and stop coroutine
        Ball ball = other.GetComponentInParent<Ball>();
        if (ball)
        {
            if (balls.ContainsKey(ball))
            {
                StopCoroutine(balls[ball]);
                balls.Remove(ball);
            }
        }
    }

    IEnumerator Timer_BeforePush(Ball ball)
    {
        //wait
        yield return new WaitForSeconds(timeBeforePush);
        
        //push right or left
        Vector2 direction = Random.Range(0, 2) <= 0 ? Vector2.left : Vector2.right;
        ball.GetComponent<Rigidbody2D>().AddForce(direction * pushForce, ForceMode2D.Impulse);

        Debug.Log("ball pushata dal centro");

        //remove from the list
        if (balls.ContainsKey(ball))
            balls.Remove(ball);
    }
}
