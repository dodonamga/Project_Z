using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnerType { Boss, Room0, Room1, Room2, Room3 };
    public SpawnerType spawner;
    Transform[] spawnPoint;
    Transform bossSpawnPoint;
    int index = 1;
    public bool isBoss;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        switch (spawner) {
            case SpawnerType.Room0:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 3, isBoss);
                } break;
            case SpawnerType.Room1:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 6, isBoss);
                } break;
            case SpawnerType.Room2:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 8, isBoss);
                }
                break;
            case SpawnerType.Boss:
                Spawn(spawnPoint[1], 7, isBoss);
                Spawn(spawnPoint[2], 10, isBoss);
                break;
        }
    }

    public void Spawn(Transform point, int prefabIndex, bool isBoss)
    {
        GameObject enemy = GameManager.instance.poolManager.Get(prefabIndex); // 3 is enemy prefab
        enemy.GetComponentInParent<RectTransform>().transform.position = Vector3.zero;
        if (isBoss) {
            enemy.transform.SetParent(point, false);
            enemy.transform.position = point.transform.position;
        }
        else {
            enemy.transform.SetParent(spawnPoint[index], false);
            enemy.transform.position = spawnPoint[index++].transform.position;
        }
        //enemy.transform.parent = spawnPoint[index++].transform;
        if (index == spawnPoint.Length) index = 1;
    }

    public void Respawn(Transform point, float delay, int prefabNum)
    {
        StartCoroutine(RespawnRoutine(point, delay, prefabNum));
    }

    private IEnumerator RespawnRoutine(Transform point, float delay, int prefabNum)
    {
        yield return new WaitForSeconds(delay);
        GameObject enemy = GameManager.instance.poolManager.Get(prefabNum);

        Enemy unit = enemy.GetComponentInChildren<Enemy>();
        if (unit != null)
            unit.Init(1.5f); // +10%
    }
}
