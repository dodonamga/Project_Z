using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoint;
    int index = 1;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        for (int index = 1; index < spawnPoint.Length; index++) {
            Spawn(spawnPoint[index]);
        }
    }

    public void Spawn(Transform point)
    {
        GameObject enemy = GameManager.instance.poolManager.Get(3); // 3 is enemy prefab
        enemy.GetComponentInParent<RectTransform>().transform.position = Vector3.zero;
        enemy.transform.SetParent(spawnPoint[index], false);
        enemy.transform.position = spawnPoint[index++].transform.position;
        //enemy.transform.parent = spawnPoint[index++].transform;
        if (index == spawnPoint.Length) index = 1;
    }

    public void Respawn(Transform point, float delay)
    {
        StartCoroutine(RespawnRoutine(point, delay));
    }

    private IEnumerator RespawnRoutine(Transform point, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject enemy = GameManager.instance.poolManager.Get(3);

        Enemy unit = enemy.GetComponentInChildren<Enemy>();
        if (unit != null)
            unit.Init(1.5f); // +10%
    }
}
