using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptble Object/WeaponsData")]
// CreateAssetMenu : Ŀ���� �޴��� �����ϴ� �Ӽ�
public class WeaponsData : ScriptableObject {
    public enum Itemtype { ArrowDefault, ArrowBig, TripleArrow, Boom, PlayerStats }

    [Header("# Main Info")]
    public Itemtype itemType;
    public int itemId;
    public string itemName;
    [TextArea] // [TextArea] : Text�� ������ �Է��� �� �ִ� ���� ����
    public string itemDesc; // itemDesc ������ ����
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