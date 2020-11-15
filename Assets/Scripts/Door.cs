using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool isActiveAtStart = false;
    [SerializeField] ArenaManager arenaToMove = default;

    [Header("Move To Next Arena")]
    [SerializeField] Transform playerPosition = default;

    void Awake()
    {
        if (isActiveAtStart == false)
            gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if collide with player
        Player player = collision.gameObject.GetComponentInParent<Player>();
        if (player)
        {
            //throw ball and deactive player
            player.ThrowBall();
            player.enabled = false;

            //move camera with smooth
            redd096.GameManager.instance.MoveCamera(arenaToMove);

            //deactive old arena
            FindObjectOfType<ArenaManager>().gameObject.SetActive(false);

            //active new arena
            arenaToMove.gameObject.SetActive(true);

            //move player
            player.transform.position = playerPosition.position;
        }
    }
}
