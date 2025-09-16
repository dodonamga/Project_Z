using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnterRoom : MonoBehaviour
{
    // Room 번호에 따라 구분
    public enum RoomType { Boss, Room1, Room2, Room3, Room4, Room5, Exit };
    public RoomType roomType;
    // 이미지 파일 교체를 위한 변수
    public Sprite[] changeFace;
    public GameObject roomBossHP;
    public GameObject _bossFace;

    public bool seeToUi;
    public float max_HP;
    public float hp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.isLive == false) return;
        if (collision.CompareTag("Player")) {
            switch (roomType) {
                case RoomType.Boss:
                    GameManager.instance.boss_HP.roomNum = 0;
                    seeToUi = true;

                    ToggleBossUI(true);
                    ChangeFace(GameManager.instance.boss_HP.roomNum);
                    break;
                case RoomType.Room1:
                    GameManager.instance.boss_HP.roomNum = 1;
                    seeToUi = true;

                    ToggleBossUI(true);
                    ChangeFace(GameManager.instance.boss_HP.roomNum);
                    break;
                case RoomType.Room2:
                    GameManager.instance.boss_HP.roomNum = 2;
                    seeToUi = true;

                    ToggleBossUI(true);
                    ChangeFace(GameManager.instance.boss_HP.roomNum);
                    break;
                case RoomType.Room3:
                    GameManager.instance.boss_HP.roomNum = 3;
                    seeToUi = true;

                    ToggleBossUI(true);
                    ChangeFace(GameManager.instance.boss_HP.roomNum);
                    break;
                case RoomType.Exit:
                    seeToUi = false;
                    ToggleBossUI(false);
                    break;
                default:
                    GameManager.instance.boss_HP.roomNum = -1; break;
            }
        }
        return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            GameManager.instance.boss_HP.roomNum = -1;
            ToggleBossUI(false);
        }
    }

    private void ToggleBossUI(bool active)
    {
        if (GameManager.instance.isLive == false) return;
        if (roomBossHP == null) {
            return;
        }

        foreach (Transform child in roomBossHP.transform) {
            if (active) child.gameObject.GetComponent<RectTransform>().transform.localScale = Vector3.one;
            else child.gameObject.GetComponent<RectTransform>().transform.localScale = Vector3.zero;
        }
    }

    public void ChangeFace(int roomNum)
    {
        UnityEngine.UI.Image bossFace = _bossFace.GetComponent<UnityEngine.UI.Image>();
        bossFace.sprite = changeFace[roomNum];
    }

    
}
