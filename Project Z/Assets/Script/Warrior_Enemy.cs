using System.Collections;
using UnityEngine;

public class Warrior_Enemy : BaseEnemy {
    [SerializeField] bool canTP;
    [SerializeField] float skill_1_CoolTime;

    protected override void Awake()
    {
        base.Awake();
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive || stun == true) return;
        if (agent.enabled != true) return;

        if ((canTP = scanner.TpSanner()) == true && isBoss == true && skill_1_CoolTime > 10) {
            timer_DefaultAttack = -3;
            stun = true;
            skill_1_CoolTime = 0;
            Tp_Skill();
            return;
        }

        if (inAttackRange = scanner.AttackRange() && timer_DefaultAttack > reroad_DefaultAttack) {
            timer_DefaultAttack = 0;
            enemyAttack();
        }

        if (findTarget == true) {
            if (!isBoss) agent.avoidancePriority = 15;
            agent.isStopped = false;
            EnemyMove(scanner.target.transform.position);
        }
        else if (damaged == true) {
            if (Vector3.Distance(transform.position, arrowStartPos) < 0.7f || findTarget == true) {
                damaged = false;
            }
            else {
                agent.isStopped = false;
                EnemyMove(arrowStartPos);
            }
        }
        else {
            rb.linearVelocity = Vector2.zero;
            if (!isBoss) agent.avoidancePriority = 30;
            agent.isStopped = true;
            ani.SetBool("1_Move", false);
        }

    }

    protected override void Update()
    {
        if (!GameManager.instance.isLive) return;
        base.Update();
        skill_1_CoolTime += Time.deltaTime;
    }

    void enemyAttack()
    {
        GameManager.instance.player.hp -= damage;
        ani.SetTrigger("2_Attack");
    }

    void Tp_Skill()
    {
        Transform tp = GameManager.instance.poolManager.Get(PoolManager.PoolType.Effect, 3).transform;
        tp.position = GameManager.instance.player.transform.position +
            (GameManager.instance.player.transform.position - transform.position).normalized * 2f;
        tp.parent = GameManager.instance.enemy_Fire.transform;

        Animator ani = tp.GetComponent<Animator>();
        ani.SetTrigger("TP");
        stun = false;
        StartCoroutine(TP(tp));
    }
    
    IEnumerator TP(Transform tp)
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = tp.position;
        tp.gameObject.SetActive(false);
    }
}
