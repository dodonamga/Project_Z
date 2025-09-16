using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {
    public enum BossType { Boss, Room1, Room2, Room3 };
    public BossType bossType;

    [Header("#.. Enemy Info")]
    public bool isBoss;
    public int prefabId;
    public bool isLive = true;
    public float damage;
    public float speed = 1f;
    public float health;
    public float maxHealth = 100;
    public float exp;
    [Header("#.. Enemy Attack")]
    public float reroad_DefaultAttack = 1.5f;
    public float timer_DefaultAttack = 0;
    [Header("#.. Target information")]
    public Rigidbody2D target;
    public Vector3 arrowStartPos;
    [Header("#.. state variable")]
    public float rePos;
    public bool findTarget = false;
    public bool damaged = false;
    public bool stun = false;
    public bool inAttackRange = false;
    // ..component variable
    protected Vector3 enemyScale;
    protected Rigidbody2D rb;
    protected Animator ani;
    protected Collider2D coll;
    protected Scanner scanner;
    public NavMeshAgent agent;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        coll = GetComponent<Collider2D>();
        scanner = GetComponent<Scanner>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        enemyScale = transform.localScale;
        SetUP();
    }

    protected virtual void SetUP()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
    }

    protected virtual void Update()
    {
        if (!GameManager.instance.isLive) return;

        timer_DefaultAttack += Time.deltaTime;

        if (transform.position != transform.parent.position && findTarget != true && damaged != true) {
            rePos += Time.deltaTime;
            if (rePos > 25) transform.position = transform.parent.position;
        }
        else rePos = 0;
    }

    protected virtual void OnEnable()
    {
        transform.position = transform.parent.position;
        transform.parent.gameObject.SetActive(true);
        coll.enabled = true;
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        scanner.enabled = true;
        health = maxHealth;
        isLive = true;
        damaged = false;
        StartCoroutine(delay(0.5f));
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLive) return;
        if (collision.CompareTag("Player")) {
            if (GameManager.instance.player.isDashing != false) {
                damaged = false;
                findTarget = false;
            }
        }

        if (collision.CompareTag("Arrow")) {
            var arrow = collision.GetComponent<Arrow>();
            arrowStartPos = arrow.shotPos;
            TakeDamage(arrow.damage, arrowStartPos);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit0);
        }
        else if (collision.CompareTag("Boom")) {
            var boom = collision.GetComponent<Boom>();
            arrowStartPos = boom.transform.position;
            TakeDamage(boom.damage, arrowStartPos);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit0);
        }
    }

    public void Init(float up)
    {
        maxHealth *= up;
        damage *= up;
        exp *= up;
    }

    protected void EnemyMove(Vector2 targetPos)
    {
        ani.SetBool("1_Move", true);
        // NavMeshAgent로 목적지 설정
        agent.SetDestination(targetPos);
        enemyScale.x = transform.position.x < target.transform.position.x ? -1f : 1f;
        transform.localScale = enemyScale;
    }

    protected void TakeDamage(float damage, Vector3 arrowStartPos)
    {
        if (!isLive) return;
        health -= damage;

        if (health > 0) StartCoroutine(DamagedKnockBack(arrowStartPos));
        else Dead();
    }

    protected virtual void Dead()
    {
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        if (prefabId == 11) GameManager.instance.GameVitroy();

        // 사망 처리
        isLive = false;
        rb.linearVelocity = Vector3.zero;
        ani.SetTrigger("4_Death");
        ani.SetBool("isDeath", true);

        scanner.enabled = false;
        agent.enabled = false;
        coll.enabled = false;

        GameManager.instance.player.kill += 1;
        GameManager.instance.player.exp += exp;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

        GetComponentInParent<Spawner>().Respawn(transform.parent, 10f, prefabId);

        StartCoroutine(DisableAfterSeconds(3f));
    }

    protected IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        transform.parent.gameObject.SetActive(false);
    }

    protected IEnumerator DamagedKnockBack(Vector3 pos)
    {
        stun = true;
        rb.linearVelocity = Vector3.zero;

        ani.SetBool("1_Move", false);
        ani.SetBool("5_Debuff", true);

        Vector3 dirVec = transform.position - pos;
        rb.AddForce(dirVec.normalized * GameManager.instance.player.str, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);
        ani.SetBool("5_Debuff", false);
        stun = false;
        damaged = true;
    }

    protected IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        agent.enabled = true;
    }

}