using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnerType { BossSpawner, Boss, Room0, Room1, Room1_Plus, Room2, Room2_Plus,Room3, Room3_Plus };
    public SpawnerType spawner;
    Transform[] spawnPoint;
    Transform bossSpawnPoint;
    int index = 1;
    string Boss_name = "";
    public bool isBoss;
    public bool wantFlipX;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        switch (spawner) {
            case SpawnerType.Room0:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 0, isBoss, wantFlipX, Boss_name);
                }
                break;
            case SpawnerType.Room1:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 1, isBoss, wantFlipX, Boss_name);
                }
                break;
            case SpawnerType.Room1_Plus:
                Spawn(spawnPoint[1], 3, isBoss, wantFlipX, Boss_name);
                break;
            case SpawnerType.Room2:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 4, isBoss, wantFlipX, Boss_name);
                }
                break;
            case SpawnerType.Room2_Plus:
                Spawn(spawnPoint[1], 6, isBoss, wantFlipX, Boss_name);
                break;
            case SpawnerType.Room3:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 7, isBoss, wantFlipX, Boss_name);
                }
                break;
            case SpawnerType.Room3_Plus:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 9, isBoss, wantFlipX, Boss_name);
                }
                break;
            case SpawnerType.Boss:
                for (int index = 1; index < spawnPoint.Length; index++) {
                    Spawn(spawnPoint[index], 10, isBoss, wantFlipX, Boss_name);
                }
                break;
            case SpawnerType.BossSpawner:
                int indexPrefab = 2;
                for (int index = 1; index < spawnPoint.Length; index++) {
                    if (index < 4) Boss_name = "Boss" + index;
                    else Boss_name = "Boss";

                    if (index == 1 || index == 4) wantFlipX = true;
                    Spawn(spawnPoint[index], indexPrefab, isBoss, wantFlipX, Boss_name);
                    indexPrefab += 3;
                }
                break;
        }
    }

    public void Spawn(Transform point, int prefabIndex, bool isBoss, bool wantFlipX, string Boss_name)
    {
        Vector2 flipX = new Vector2(-1, 0);

        GameObject enemy = GameManager.instance.poolManager.Get(PoolManager.PoolType.Monster, prefabIndex);

        if (prefabIndex == 2) GameManager.instance.boss_HP.boss0[1] = enemy;
        else if (prefabIndex == 5) GameManager.instance.boss_HP.boss0[2] = enemy; 
        else if (prefabIndex == 8) GameManager.instance.boss_HP.boss0[3] = enemy;
        else if (prefabIndex == 11) GameManager.instance.boss_HP.boss0[0] = enemy;

        enemy.GetComponentInParent<RectTransform>().transform.position = Vector3.zero;
        if (isBoss) {
            enemy.transform.SetParent(point, false);
            enemy.transform.position = point.transform.position;
            WantFlipX(enemy, Boss_name, wantFlipX);
        }
        else {
            enemy.transform.SetParent(spawnPoint[index], false);
            enemy.transform.position = spawnPoint[index++].transform.position;
            WantFlipX(enemy, Boss_name, wantFlipX);
        }
    }

    public void Respawn(Transform point, float delay, int prefabNum)
    {
        StartCoroutine(RespawnRoutine(point, delay, prefabNum));
    }

    private IEnumerator RespawnRoutine(Transform point, float delay, int prefabNum)
    {
        yield return new WaitForSeconds(delay);
        GameObject enemy = GameManager.instance.poolManager.Get(PoolManager.PoolType.Monster, prefabNum);

        if (prefabNum == 2) GameManager.instance.boss_HP.boss0[1] = enemy;
        else if (prefabNum == 5) GameManager.instance.boss_HP.boss0[2] = enemy;
        else if (prefabNum == 8) GameManager.instance.boss_HP.boss0[3] = enemy;
        else if (prefabNum == 11) GameManager.instance.boss_HP.boss0[0] = enemy;

        BaseEnemy unit = enemy.GetComponentInChildren<BaseEnemy>();
        if (unit != null) {
            unit.Init(1.5f); // +10%
            unit.health = unit.maxHealth;
        }
    }

    private void WantFlipX(GameObject enemy, string boss_name, bool wantFlipX)
    {
        if (enemy.GetComponentInChildren<BaseEnemy>().isBoss != true) {
            enemy.transform.Find("UnitRoot").GetComponent<Transform>().localScale = new Vector3(wantFlipX ? -1 : 1, 1, 1);
        }
        else {
            int id = enemy.GetComponentInChildren<BaseEnemy>().prefabId;
            if (id == 9 || id == 10) boss_name = "UnitRoot";
            enemy.transform.Find(boss_name).GetComponent<Transform>().localScale = new Vector3(wantFlipX ? -1 : 1, 1, 1);
        }
    }
}
