using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    public int level;
    public WeaponsData data;
    public Weapons weapons;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] text = GetComponentsInChildren<Text>();
        textLevel = text[0];
        textName = text[1];
        textDesc = text[2];
        textName.text = data.itemName;
    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + level;

        switch (data.itemType) {
            case WeaponsData.Itemtype.ArrowDefault:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.penetration[level]);
                break;
            case WeaponsData.Itemtype.TripleArrow:
                if (level == 0 && data.itemId == 0) {
                    textDesc.text = string.Format("Get Skill :\nTripleArrow\nDamage : DefaultAtk\nCool Time : 15s");
                }
                else {
                    textDesc.text = string.Format(data.itemDesc, data.penetration[level]);
                }
                break;
            case WeaponsData.Itemtype.ArrowBig:
                if (level == 0 && data.itemId == 0) {
                    textDesc.text = string.Format("Get Skill :\nArrowBig\nDamage : 20\nCool Time : 15s");
                }
                else if (level > 0 && data.itemId == 0){
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.speeds[level] * 100);
                }
                if (data.itemId == 1) {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                }
                else if (data.itemId == 2) {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                }
                break;
            case WeaponsData.Itemtype.Boom:
                if (level == 0 && data.itemId == 0) {
                    textDesc.text = string.Format("Get Skill : Boom!\nDamage : 100\nCool Time : 30s");
                }
                else if (level > 0 && data.itemId == 0){
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                }
                if (data.itemId == 1) {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                } else if (data.itemId == 2 || data.itemId == 3) {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                } 
                    break;
            // .. player stats +@
            case WeaponsData.Itemtype.PlayerStats:
                if (data.itemId == 0) {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                }
                else if (data.itemId == 1 || data.itemId == 2) {
                    textDesc.text = string.Format(data.itemDesc, data.damages);
                }
                else if (data.itemId == 3) {
                    textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                }
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType) {
            case WeaponsData.Itemtype.ArrowDefault:
                if (level == 0 && data.itemId == 0) {
                    GameObject newGameObject = new GameObject();

                    weapons = newGameObject.AddComponent<Weapons>();
                    weapons.Init(data);

                    GameManager.instance.ArrowDefault = weapons;
                }
                else {
                    GameManager.instance.ArrowDefault.damage += data.baseDamage * data.damages[level];
                    GameManager.instance.ArrowDefault.penetration += data.penetration[level];
                }
                break;
            case WeaponsData.Itemtype.ArrowBig:
                if (level == 0 && data.itemId == 0) {
                    GameObject newGameObject = new GameObject();

                    weapons = newGameObject.AddComponent<Weapons>();
                    weapons.Init(data);

                    GameManager.instance.ArrowBig = weapons;
                }
                else if (level > 0 && data.itemId == 0) {
                    GameManager.instance.ArrowBig.damage += data.baseDamage * data.damages[level];
                    GameManager.instance.ArrowBig.penetration += data.penetration[level];
                    GameManager.instance.ArrowBig.speed += data.baseSpeed * data.speeds[level];
                }
                if (data.itemId == 1) {
                    GameManager.instance.ArrowBig.damage += GameManager.instance.ArrowBig.damage * data.damages[level];
                }
                else if (data.itemId == 2) {    
                    GameManager.instance.ArrowBig.penetration += (int)data.damages[level];
                }
                break;
            case WeaponsData.Itemtype.TripleArrow:
                GameManager.instance.player.skill_0_CoolTime -= data.penetration[level];
                break;
            case WeaponsData.Itemtype.Boom:
                if (level == 0 && data.itemId == 0) {
                    GameObject newGameObject = new GameObject();

                    weapons = newGameObject.AddComponent<Weapons>();
                    weapons.Init(data);

                    GameManager.instance.Boom = weapons;
                }
                else if (level > 0 && data.itemId == 0){
                    GameManager.instance.Boom.damage += data.baseDamage * data.damages[level];
                }
                if (data.itemId == 1) {
                    GameManager.instance.Boom.damage += data.baseDamage * data.damages[level];
                }
                else if (data.itemId == 2) {
                    GameManager.instance.player.skill_2_CoolTime -= (int)data.damages[level];
                } else if (data.itemId == 3) {
                    GameManager.instance.Boom.speed -= (int)data.damages[level];
                }
                    break;
            // ..Player stats +@
            case WeaponsData.Itemtype.PlayerStats:
                if (data.itemId == 0) {
                    GameManager.instance.player.moveSpeed += GameManager.instance.player.moveSpeed *= data.damages[level];
                }
                else if (data.itemId == 1) {
                    GameManager.instance.player.str += data.damages[level];
                }
                else if (data.itemId == 2) {
                    GameManager.instance.player.maxhp += data.damages[level];
                }
                else if (data.itemId == 3) {
                    GameManager.instance.player.reroad -= data.speeds[level];
                }
                break;

        }

        level++;

        if (level == data.damages.Length) {
            GetComponent<Button>().interactable = false;
        }
    }
}
