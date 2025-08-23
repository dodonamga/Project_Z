using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        GameManager.instance.Stop();
        rect.localScale = Vector3.one;
    }

    public void Hide()
    {
        GameManager.instance.Resume();
        rect.localScale = Vector3.zero;
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    public void Next()
    {
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }

        List<int> selectedIndexes = new List<int>();

        while (selectedIndexes.Count < 3) {
            int index = Random.Range(0, Mathf.Min(4, items.Length)); // 0~3 기본 풀에서 선택
            Item candidate = items[index];

            // 만약 candidate가 만렙이라면 → 4~13 중에서 랜덤 선택
            if (candidate.level == candidate.data.damages.Length) {
                index = Random.Range(4, 13); // 4~13 범위
                candidate = items[index];
            }

            // 후보도 만렙이면 → 다시 랜덤 뽑기
            int safety = 0;
            while (candidate.level == candidate.data.damages.Length || selectedIndexes.Contains(index)) {
                index = Random.Range(4, Mathf.Min(13, items.Length));
                candidate = items[index];
                safety++;
                if (safety > 50) break; // 무한루프 방지
            }

            // 최종 선택
            if (!selectedIndexes.Contains(index)) {
                selectedIndexes.Add(index);
            }
        }

        // 뽑은 아이템 활성화
        foreach (int i in selectedIndexes) {
            items[i].gameObject.SetActive(true);
        }
    }


    //public void Next()
    //{
    //    foreach (Item item in items) {
    //        item.gameObject.SetActive(false);
    //    }

    //    int[] random = new int[3];

    //    while (true) {
    //        random[0] = Random.Range(0, items.Length);
    //        random[1] = Random.Range(0, items.Length);
    //        random[2] = Random.Range(0, items.Length);

    //        if (random[0] != random[1] && random[0] != random[2] && random[1] != random[2]) {
    //            break;
    //        }
    //    }

    //    for (int index = 0; index < random.Length; index++) {
    //        Item randomItem = items[random[index]];

    //        if (randomItem.level == randomItem.data.damages.Length) {
    //            randomItem.gameObject.SetActive(true);
    //        }
    //        else randomItem.gameObject.SetActive(true);
    //    }
    //}
}
