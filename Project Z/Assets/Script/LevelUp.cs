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
        // ��� ������ ��Ȱ��ȭ
        foreach (Item item in items) {
            item.gameObject.SetActive(false);
        }

        int[] random = new int[3];

        // 0,1,2,3���� ���� �ٸ� 3�� �̱�
        while (true) {
            random[0] = Random.Range(0, 4); // 0~3
            random[1] = Random.Range(0, 4);
            random[2] = Random.Range(0, 4);

            if (random[0] != random[1] && random[0] != random[2] && random[1] != random[2])
                break;
        }

        // ���õ� ������ ó��
        for (int i = 0; i < random.Length; i++) {
            int index = random[i];
            Item selected = items[index];

            // �����̸� �ٸ� �ĺ� �̱�
            if (selected.level >= selected.data.damages.Length) {
                List<int> candidates = new List<int>();

                // 3������ ������ �߿��� ���� ������ �ƴ� �ֵ鸸 �ĺ�
                for (int j = 3; j < items.Length; j++) {
                    if (items[j].level < items[j].data.damages.Length &&
                        System.Array.IndexOf(random, j) == -1) // �̹� ���� �� ����
                    {
                        candidates.Add(j);
                    }
                }

                if (candidates.Count > 0) {
                    index = candidates[Random.Range(0, candidates.Count)];
                }
                else {
                    // ��ü�� �������� �ƿ� ���� ��� (��� ����)
                    index = -1;
                }
            }

            if (index != -1) {
                items[index].gameObject.SetActive(true);
                random[i] = index; // ���� �� ����
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
