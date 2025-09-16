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
        public PoolType type;          // ī�װ�
        public GameObject[] prefabs;   // �ش� ī�װ��� �����յ�
    }

    public Pool[] pools;

    // ī�װ��� ������Ʈ Ǯ
    private Dictionary<PoolType, List<GameObject>[]> poolDictionary;

    // prefab �� (ī�װ�, �ε���) ����
    private Dictionary<GameObject, (PoolType type, int index)> prefabLookup;

    private void Awake()
    {
        poolDictionary = new Dictionary<PoolType, List<GameObject>[]>();
        prefabLookup = new Dictionary<GameObject, (PoolType, int)>();

        foreach (var pool in pools) {
            var lists = new List<GameObject>[pool.prefabs.Length];
            for (int i = 0; i < lists.Length; i++) {
                lists[i] = new List<GameObject>();

                // prefab�� key�� ī�װ�/�ε��� ���� ����
                prefabLookup[pool.prefabs[i]] = (pool.type, i);
            }
            poolDictionary[pool.type] = lists;
        }
    }

    // ������Ʈ ��������
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

    // prefab �� (ī�װ�, �ε���) ã��
    public (PoolType type, int index) GetPrefabId(GameObject prefab)
    {
        return prefabLookup[prefab];
    }
}