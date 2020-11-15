﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] bool isActiveAtStart = false;
    [SerializeField] ArenaManager arenaToMove = default;

    [Header("Move To Next Arena")]
    [SerializeField] float timeToMoveCamera = 1;
    [SerializeField] Transform playerPosition = default;

    Coroutine moveToNextArena;

    void Start()
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
            if (moveToNextArena == null)
            {
                //throw ball and deactive player
                player.ThrowBall();
                player.enabled = false;

                //move camera with smooth
                moveToNextArena = StartCoroutine(MoveCamera());

                //deactive old arena
                FindObjectOfType<ArenaManager>().gameObject.SetActive(false);

                //active new arena
                arenaToMove.gameObject.SetActive(true);

                //move player
                player.transform.position = playerPosition.position;
            }
        }
    }

    IEnumerator MoveCamera()
    {
        //get camera and start position
        Transform cam = Camera.main.transform;
        Vector3 startPosition = cam.position;

        //animation camera
        float delta = 0;
        while(delta < 1)
        {
            delta += Time.deltaTime / timeToMoveCamera;
            cam.position = Vector3.Lerp(startPosition, arenaToMove.CameraPosition.position, delta);

            yield return null;
        }

        moveToNextArena = null;
    }
}
