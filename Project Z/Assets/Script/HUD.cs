using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    enum BossName { Boss, Boss1, Boss2, Boss3 };
    public int roomNum;

    public enum InfoType { Exp, level, Kill, Time, Health, Enemy_Health, Boss_Health }
    public InfoType type;

    public GameObject[] boss0 = new GameObject[4];


    public float boss_curHealth;
    public float boss_maxHealth;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<Text>();
    }

    private void LateUpdate()
    {
        switch (type) {
            case InfoType.Exp:
                float curExp = GameManager.instance.player.exp;
                float maxExp = GameManager.instance.player.nextExp
                    [Mathf.Min(GameManager.instance.player.level, GameManager.instance.player.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.player.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.player.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.player.hp;
                float maxHealth = GameManager.instance.player.maxhp;
                mySlider.value = curHealth / maxHealth;
                break;
            case InfoType.Enemy_Health:
                BaseEnemy be = transform.parent.parent.parent.Find("UnitRoot").GetComponent<BaseEnemy>();
                float E_curHealth = be.health;
                float E_maxHealth = be.maxHealth;
                mySlider.value = E_curHealth / E_maxHealth;
                break;
            case InfoType.Boss_Health:
                //GameObject go = FindGameObject(roomNum);
                //if (go == null) return;
                //boss_curHealth = go.GetComponent<BaseEnemy>().health;
                //boss_maxHealth = go.GetComponent<BaseEnemy>().maxHealth;
                for (int index = 0; index < boss0.Length; index++) {
                    if (index == roomNum) {
                        boss_curHealth = boss0[index].GetComponentInChildren<BaseEnemy>().health;
                        boss_maxHealth = boss0[index].GetComponentInChildren<BaseEnemy>().maxHealth;
                        mySlider.value = boss_curHealth / boss_maxHealth;
                    }
                }

                //mySlider.value = boss_curHealth / boss_maxHealth;
                break;
        }
    }
    public GameObject FindGameObject(int roomNum)
    {
        if (roomNum == -1) return null;
        string name = ((BossName)roomNum).ToString();
        GameObject boss = GameObject.Find(name);
        return boss;
    }
}

