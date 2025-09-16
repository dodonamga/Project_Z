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

        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.filter(true);
    }

    public void Hide()
    {
        GameManager.instance.Resume();
        rect.localScale = Vector3.zero;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.filter(false);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    public void Next()
    {
        // 모든 아이템 비활성화
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }

        int[] random = new int[3];

        // 0,1,2,3에서 서로 다른 3개 뽑기
        while (true) {
            random[0] = Random.Range(0, 4); // 0~3
            random[1] = Random.Range(0, 4);
            random[2] = Random.Range(0, 4);

            if (random[0] != random[1] && random[0] != random[2] && random[1] != random[2])
                break;
        }

        // 선택된 아이템 처리
        for (int i = 0; i < random.Length; i++) {
            int index = random[i];
            Item selected = items[index];

            // 만렙이면 다른 후보 뽑기
            if (selected.level >= selected.data.damages.Length) {
                List<int> candidates = new List<int>();

                // 3번부터 끝까지 중에서 아직 만렙이 아닌 애들만 후보
                for (int j = 3; j < items.Length; j++) {
                    if (items[j].level < items[j].data.damages.Length &&
                        System.Array.IndexOf(random, j) == -1) // 이미 뽑힌 거 제외
                    {
                        candidates.Add(j);
                    }
                }

                if (candidates.Count > 0) {
                    index = candidates[Random.Range(0, candidates.Count)];
                }
                else {
                    // 대체할 아이템이 아예 없는 경우 (모두 만렙)
                    index = -1;
                }
            }

            if (index != -1) {
                items[index].gameObject.SetActive(true);
                random[i] = index; // 뽑힌 값 갱신
            }
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
