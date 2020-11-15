using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] Vector3 cameraPosition = Vector3.zero;

    [Header("Doors")]
    [SerializeField] GameObject[] toActivate = default;
    [SerializeField] GameObject[] objectsToDeactivate = default;

    public Vector3 CameraPosition => cameraPosition;

    List<Enemy> enemiesInScene = new List<Enemy>();

    void OnEnable()
    {
        enemiesInScene.Clear();

        //create enemies list
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
            enemiesInScene.Add(enemy);
    }

    IEnumerator DeactiveEnemy(Enemy enemy)
    {
        //wait, then deactivate enemy
        yield return new WaitForSeconds(1);

        enemy.gameObject.SetActive(false);
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
