using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBall : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float timeBeforePush = 3;
    [SerializeField] float pushForce = 2;

    List<Ball> balls = new List<Ball>();

    void OnTriggerEnter(Collider other)
    {
        //if hit ball, add to list and start coroutine
        Ball ball = other.GetComponentInParent<Ball>();
        if (ball)
        {
            balls.Add(ball);
            StartCoroutine(Timer_BeforePush(ball));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if ball exit from trigger, remove from the list
        Ball ball = other.GetComponentInParent<Ball>();
        if (ball)
        {
            if (balls.Contains(ball))
                balls.Remove(ball);
        }
    }

    IEnumerator Timer_BeforePush(Ball ball)
    {
        //wait
        yield return new WaitForSeconds(timeBeforePush);

        //if ball still in the list
        if (balls.Contains(ball))
        {
            //push right or left
            Vector2 direction = Random.Range(0, 2) <= 0 ? Vector2.left : Vector2.right;
            ball.GetComponent<Rigidbody>().AddForce(direction * pushForce);

            //remove from the list
            if (balls.Contains(ball))
                balls.Remove(ball);
        }
    }
}
