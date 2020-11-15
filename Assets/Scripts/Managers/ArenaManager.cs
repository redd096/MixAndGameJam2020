﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Vector3 cameraPosition = Vector3.zero;

    [Header("Doors")]
    [SerializeField] GameObject[] toActivate = default;
    [SerializeField] GameObject[] objectsToDeactivate = default;

    [Header("Timer")]
    [SerializeField] int timer = 3;

    public Vector3 CameraPosition => cameraPosition;

    List<Enemy> enemiesInScene = new List<Enemy>();
    Coroutine timerCoroutine;

    void OnEnable()
    {
        enemiesInScene.Clear();

        //create enemies list
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemiesInScene.Add(enemy);

            //deactive enemy at start
            enemy.enabled = false;

            //check if there is a boss and show health bar
            if(enemy.IsBoss)
            {
                redd096.GameManager.instance.uiManager.ShowHealthBoss(true);
            }
        }

        //if there are enemies in scene, start timer
        if(enemiesInScene.Count > 0)
        {
            if (timerCoroutine == null)
                timerCoroutine = StartCoroutine(TimerCoroutine());
        }
        //else active player
        else
        {
            redd096.GameManager.instance.player.enabled = true;
        }
    }

    IEnumerator DeactiveEnemy(Enemy enemy)
    {
        //wait, then deactivate enemy
        yield return new WaitForSeconds(1);

        enemy.gameObject.SetActive(false);
    }

    IEnumerator TimerCoroutine()
    {
        //show timer
        redd096.GameManager.instance.uiManager.SetTimerText(timer.ToString("F0"));
        redd096.GameManager.instance.uiManager.ShowTimerText(true);

        for(int i = timer; i >= 0; i--)
        {
            //wait one second
            yield return new WaitForSeconds(1);

            //if 0, hide timer
            if(i == 0)
            {
                redd096.GameManager.instance.uiManager.ShowTimerText(false);
            }
            //else set text
            {
                redd096.GameManager.instance.uiManager.SetTimerText(i.ToString("F0"));
            }
        }

        //enable player
        redd096.GameManager.instance.player.enabled = true;

        //enable every enemy
        foreach(Enemy enemy in enemiesInScene)
        {
            enemy.enabled = true;
        }
    }

    #region public API

    public void EnemyDead(Enemy enemy)
    {
        //deactivate enemy and remove from the list
        StartCoroutine(DeactiveEnemy(enemy));
        enemiesInScene.Remove(enemy);

        //if no enemies in scene, open doors
        if(enemiesInScene == null || enemiesInScene.Count <= 0)
        {
            OpenDoors();
        }
    }

    public void OpenDoors()
    {
        //hide boos health bar
        redd096.GameManager.instance.uiManager.ShowHealthBoss(false);

        //active every door
        foreach (GameObject door in toActivate)
        {
            door.SetActive(true);
        }

        //deactive every object in objectsToDeactivate
        foreach(GameObject go in objectsToDeactivate)
        {
            go.SetActive(false);
        }
    }

    #endregion
}
