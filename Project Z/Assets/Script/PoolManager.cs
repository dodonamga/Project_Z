using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    public enum PoolType {
        Player_Weapon,
        Effect,
        Monster,
        Monster_Weapon
    }

    [System.Serializable]
    public class Pool {
        public PoolType type;          // 카테고리
        public GameObject[] prefabs;   // 해당 카테고리의 프리팹들
    }

    public Pool[] pools;

    // 카테고리별 오브젝트 풀
    private Dictionary<PoolType, List<GameObject>[]> poolDictionary;

    // prefab → (카테고리, 인덱스) 매핑
    private Dictionary<GameObject, (PoolType type, int index)> prefabLookup;

    private void Awake()
    {
        poolDictionary = new Dictionary<PoolType, List<GameObject>[]>();
        prefabLookup = new Dictionary<GameObject, (PoolType, int)>();

        foreach (var pool in pools) {
            var lists = new List<GameObject>[pool.prefabs.Length];
            for (int i = 0; i < lists.Length; i++) {
                lists[i] = new List<GameObject>();

                // prefab을 key로 카테고리/인덱스 매핑 저장
                prefabLookup[pool.prefabs[i]] = (pool.type, i);
            }
            poolDictionary[pool.type] = lists;
        }
    }

    // 오브젝트 꺼내오기
    public GameObject Get(PoolType type, int prefabIndex)
    {
        var list = poolDictionary[type][prefabIndex];
        GameObject select = null;

        foreach (var obj in list) {
            if (!obj.activeSelf) {
                select = obj;
                select.SetActive(true);
                return select;
            }
        }

        select = Instantiate(pools[(int)type].prefabs[prefabIndex], transform);
        list.Add(select);
        return select;
    }

    // prefab → (카테고리, 인덱스) 찾기
    public (PoolType type, int index) GetPrefabId(GameObject prefab)
    {
        return prefabLookup[prefab];
    }
}