using System;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public int prefabId;
    public int id;
    public int penetration;
    public float damage;
    public float speed;

    public PoolManager.PoolType poolType;
    public int prefabIndex;

    public void Init(WeaponsData data)
    {
        // ..basic set
        name = data.itemName;
        transform.parent = GameManager.instance.poolManager.transform;
        transform.localPosition = Vector3.zero;
        // ..property set
        id = data.itemId;
        damage = data.baseDamage;
        penetration = data.basePenetration;
        speed = data.baseSpeed;

        // 발사체 prefab이 어느 카테고리에 몇 번인지 매핑 찾기
        var idInfo = GameManager.instance.poolManager.GetPrefabId(data.projectile);
        poolType = idInfo.type;
        prefabIndex = idInfo.index;

    }

    public void ShotArrow(int index)
    {
        Vector3 shotPos = GameManager.instance.cuser.GetmousePoint();
        Vector3 dir = (shotPos - GameManager.instance.player.transform.position).normalized;
        ShotArrow(dir, index);
    }

    //특수 케이스: 방향 직접 지정
    public void ShotArrow(Vector3 dir, int index)
    {
        Transform arrow = GameManager.instance.poolManager.Get(PoolManager.PoolType.Player_Weapon, index).transform;
        arrow.position = GameManager.instance.player.transform.position;
        arrow.parent = transform;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        arrow.GetComponent<Arrow>().Init(damage, penetration, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }

    public void Boom(int index)
    {
        Transform boom = GameManager.instance.poolManager.Get(PoolManager.PoolType.Player_Weapon, index).transform;
        boom.position = GameManager.instance.player.transform.position;
        boom.parent = transform;

        boom.GetComponent<Boom>().Init(damage, speed);
    }

    public void Enemy_Fire(int index, Vector3 enemyPos, float damage)
    {
        Vector3 shotPos = GameManager.instance.player.transform.position;
        Vector3 dir = (shotPos - enemyPos).normalized;
        Transform fire = GameManager.instance.poolManager.Get(PoolManager.PoolType.Monster_Weapon, 0).transform;
        fire.position = enemyPos;
        fire.parent = transform;
        fire.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        fire.GetComponent<Fire>().Init(dir, damage);
    }
}
