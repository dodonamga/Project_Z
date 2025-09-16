using System.Collections;
using UnityEngine;

public class Final_Boss : BaseEnemy
{
    [SerializeField] bool canTP;
    [SerializeField] float skill_0_CoolTime;
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

        if ((canTP = scanner.TpSanner()) == true && skill_1_CoolTime > 10) {
            timer_DefaultAttack = -1.5f;
            stun = true;
            skill_1_CoolTime = 0;
            ani.SetBool("1_Move", false);
            Tp_Skill();
            return;
        }

        if (skill_0_CoolTime > 7 && (canTP = scanner.TpSanner()) == true) {
            timer_DefaultAttack = -1.5f;
            skill_0_CoolTime = 0;
            ani.SetTrigger("2_Attack");
            Skull_Skill();
            return;
        }

        if (inAttackRange = scanner.AttackRange() && timer_DefaultAttack > reroad_DefaultAttack) {
            timer_DefaultAttack = 0;
            enemyAttack();
        }

        if (findTarget == true) {
            EnemyMove(scanner.target.transform.position);
        }
        else if (damaged == true) {
            if (Vector3.Distance(transform.position, arrowStartPos) < 0.7f || findTarget == true) {
                damaged = false;
            }
            EnemyMove(arrowStartPos);
        }
        else {
            rb.linearVelocity = Vector3.zero;
            ani.SetBool("1_Move", false);
        }

    }
    protected override void Update()
    {
        if (!GameManager.instance.isLive) return;
        base.Update();
        skill_0_CoolTime += Time.deltaTime;
        skill_1_CoolTime += Time.deltaTime;
    }

    void enemyAttack()
    {
        GameManager.instance.player.hp -= damage;
        ani.SetTrigger("2_Attack");
    }

    void Skull_Skill()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;

        // X�� ���� ����
        Vector3[] offsets = new Vector3[]
        {
        Vector3.zero,                                // �߾�
        new Vector3(1, 1, 0),   // ��
        new Vector3(-1, 1, 0),  // ��
        new Vector3(1, -1, 0),  // ��
        new Vector3(-1, -1, 0)  // ��
        };

        for (int i = 0; i < offsets.Length; i++) {
            Transform meteor = GameManager.instance.poolManager.Get(PoolManager.PoolType.Effect, 2).transform; // 11 = meteor prefab
            meteor.position = playerPos + offsets[i] * 4f;  // �Ÿ� ���� (2f�� ����, �ʿ信 ���� ����)
            meteor.parent = GameManager.instance.enemy_Fire.transform;
            meteor.gameObject.SetActive(true);
        }
    }

    void Tp_Skill()
    {
        // ���ڰ� ���� ������ (�߾�, ��, �Ʒ�, ����, ������)
        Vector3[] offsets = new Vector3[]
        {
        Vector3.zero,                // �߾�
        Vector3.up * 2f,             // ��
        Vector3.down * 2f,           // �Ʒ�
        Vector3.left * 2f,           // ����
        Vector3.right * 2f           // ������
        };

        // TP ����Ʈ ���� ����
        Transform[] tpEffects = new Transform[offsets.Length];
        for (int i = 0; i < offsets.Length; i++) {
            tpEffects[i] = GameManager.instance.poolManager.Get(PoolManager.PoolType.Effect, 3).transform;
            tpEffects[i].position = GameManager.instance.player.transform.position + offsets[i];
            tpEffects[i].parent = GameManager.instance.enemy_Fire.transform;

            Animator ani = tpEffects[i].GetComponent<Animator>();
            ani.SetTrigger("TP");
        }

        // ���� �����̵��� ��ġ �ϳ� ���� ����
        int randomIndex = Random.Range(0, offsets.Length);
        Vector3 chosenPos = tpEffects[randomIndex].position;

        stun = false;
        StartCoroutine(TP(tpEffects, chosenPos));
    }

    IEnumerator TP(Transform[] tpEffects, Vector3 chosenPos)
    {
        yield return new WaitForSeconds(1.5f);

        // ���� �̵�
        transform.position = chosenPos;

        // ��� TP ����Ʈ ��Ȱ��ȭ
        foreach (Transform tp in tpEffects) {
            tp.gameObject.SetActive(false);
        }
    }
}
