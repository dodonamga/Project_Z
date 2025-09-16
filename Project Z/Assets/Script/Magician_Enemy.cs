using UnityEngine;

public class Magician_Enemy : BaseEnemy {
    [SerializeField] float skill_CoolTime;
    protected override void Awake()
    {
        base.Awake();
        health = maxHealth;
    }

    protected virtual void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive || stun == true) return;
        if (!agent.enabled) return;

        if (isBoss == true && skill_CoolTime > 10f && (inAttackRange = scanner.AttackRange()) == true) {
            timer_DefaultAttack = -3;
            skill_CoolTime = 0;
            ani.SetTrigger("7_Metero");
        }

        if (inAttackRange = scanner.AttackRange() && timer_DefaultAttack > reroad_DefaultAttack) {
            timer_DefaultAttack = 0;
            ani.SetTrigger("2_Attack");
        }

        if (findTarget == true) {
            if (!isBoss) agent.avoidancePriority = 15;
            if (inAttackRange = scanner.AttackRange() == false) {
                agent.isStopped = false;
                EnemyMove(scanner.target.transform.position);
            }
            else {
                agent.isStopped = true;
                rb.linearVelocity = Vector3.zero;
                ani.SetBool("1_Move", false);
            }
        }
        else if (damaged == true) {
            if (Vector3.Distance(transform.position, arrowStartPos) < 0.7f || findTarget == true) {
                agent.isStopped = true;
                rb.linearVelocity = Vector2.zero;
                damaged = false;
            }
            else {
                if (!isBoss) agent.avoidancePriority = 30;
                agent.isStopped = false;
                EnemyMove(arrowStartPos);
            }
        }
        else {
            rb.linearVelocity = Vector2.zero;
            agent.isStopped = true;
            ani.SetBool("1_Move", false);
        }
    }

    protected override void Update()
    {
        if (!GameManager.instance.isLive) return;
        base.Update();
        skill_CoolTime += Time.deltaTime;
    }

    public void Mg_enemyAttack()
    {
        GameManager.instance.enemy_Fire.Enemy_Fire(9, transform.position, damage);  // 9 is enemy attack prefab(Skull_M Bullet prefab)
    }

    void Skull_Skill()
    {
        Transform meteor = GameManager.instance.poolManager.Get(PoolManager.PoolType.Effect, 2).transform;  // 11 is meteor prefab
        meteor.position = GameManager.instance.player.transform.position;
        meteor.parent = GameManager.instance.enemy_Fire.transform;
        meteor.gameObject.SetActive(true);
    }
}
