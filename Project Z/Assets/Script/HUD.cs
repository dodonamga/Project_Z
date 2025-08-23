using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, level, Kill, Time, Health }
    public InfoType type;

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

        }
    }
}

