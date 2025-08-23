using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptble Object/WeaponsData")]
// CreateAssetMenu : 커스텀 메뉴를 생성하는 속성
public class WeaponsData : ScriptableObject {
    public enum Itemtype { ArrowDefault, ArrowBig, TripleArrow, Boom, PlayerStats }

    [Header("# Main Info")]
    public Itemtype itemType;
    public int itemId;
    public string itemName;
    [TextArea] // [TextArea] : Text를 여러줄 입력할 수 있는 공간 생성
    public string itemDesc; // itemDesc 아이템 설명
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int basePenetration;
    public float baseSpeed;
    public float[] damages;
    public int[] penetration;
    public float[] speeds;

    [Header("# Weapon")]
    public GameObject projectile;

}