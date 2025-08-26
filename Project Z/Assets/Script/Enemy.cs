using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("#.. Enemy Info")]
    public int myPrefabIndex;
    public bool isLive = true;
    public float death_Timer;
    [SerializeField] float rePos;
    [SerializeField] float exp = 10;
    [SerializeField] float damage = 5;
    [SerializeField] float speed = 1f;
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float reroad_DefaultAttack = 1.5f;
    [SerializeField] float timer_DefaultAttack = 0;
    [SerializeField] float skill_0_CoolTime = 0;

    [Header("#.. Target information")]
    public Rigidbody2D target;
    [SerializeField] Vector3 arrowStartPos;

    [Header("#.. state variable")]
    public bool findTarget = false;
    [SerializeField] bool damaged = false;
    [SerializeField] bool stun = false;
    [SerializeField] bool inAttackRange = false;

    // ..component variable
    Vector3 enemyScale; // ÁÂ¿ì ¹ÝÀü
    Collider2D coll;
    Rigidbody2D rb;
    Animator ani;
    [SerializeField] Scanner scanner;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        scanner = GetComponent<Scanner>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        enemyScale = transform.localScale;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive || stun == true) return;

        if (myPrefabIndex == 10 && skill_0_CoolTime > 10 && inAttackRange == true) {
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
            if ((myPrefabIndex == 8 || myPrefabIndex == 10) && (inAttackRange = scanner.AttackRange()) == false) {
                EnemyMove(scanner.target.transform.position, rb.position);
            }
            else if ((myPrefabIndex == 8 || myPrefabIndex == 10) && (inAttackRange = scanner.AttackRange()) == true) {
                Debug.Log("2nd secces");
                rb.linearVelocity = Vector3.zero;
                ani.SetBool("1_Move", false);
            }
            else EnemyMove(scanner.target.transform.position, rb.position);
        }
        else if (damaged == true) {
            if ((myPrefabIndex == 8 || myPrefabIndex == 10) && (Vector3.Distance(transform.position, arrowStartPos) < 0.7f || findTarget == true)) {
                damaged = false;
            }
            else if (Vector3.Distance(transform.position, arrowStartPos) < 0.7f || findTarget == true) {
                damaged = false;
            }
            EnemyMove(arrowStartPos, rb.position);
        }
        else if (findTarget != true && damaged != true) {
            rb.linearVelocity = Vector3.zero;
            ani.SetBool("1_Move", false);
        }

        if (findTarget == true && !(myPrefabIndex == 8 || myPrefabIndex == 10)) {
            EnemyMove(scanner.target.transform.position, rb.position);
        }
    }

    private void Update()
    {
        if (!GameManager.instance.isLive) return;

        if (myPrefabIndex == 10) skill_0_CoolTime += Time.deltaTime;

        if (isLive != true) {
            death_Timer += Time.deltaTime;
        }

        if (transform.position != transform.parent.position && findTarget == false) {
            rePos += Time.deltaTime;
            if (rePos > 15) transform.position = transform.parent.position;
        }
        else rePos = 0;

        timer_DefaultAttack += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.CompareTag("Arrow") || !collision.CompareTag("Boom") || !isLive) return;
        if (!isLive) return;

        if (collision.CompareTag("Arrow")) {
            arrowStartPos = collision.GetComponent<Arrow>().shotPos;
            TakeDamage(collision.GetComponent<Arrow>().damage, arrowStartPos);
        }

        if (collision.CompareTag("Boom")) {
            arrowStartPos = collision.GetComponent<Boom>().transform.position;
            TakeDamage(collision.GetComponent<Boom>().damage, arrowStartPos);
        }

        return;

    }

    private void OnEnable()
    {
        transform.position = transform.parent.position;
        transform.parent.gameObject.SetActive(true);
        coll.enabled = true;
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        scanner.enabled = true;
        health = maxHealth;
        isLive = true;
        damaged = false;
    }

    public void Init(float up)
    {
        maxHealth *= up;
        damage *= up;
        exp *= up;
    }

    public void TakeDamage(float damage, Vector3 arrowStartPos)
    {
        if (!isLive) return;

        health -= damage;

        if (health > 0) {
            StartCoroutine(DamagedKnockBack(arrowStartPos));
        }
        else {
            Dead();
        }
    }

    void EnemyMove(Vector2 pos1, Vector2 pos2)
    {
        enemyScale.x = transform.position.x < pos1.x ? -1f : 1f;
        transform.localScale = enemyScale;

        Vector2 nextPos = (pos1 - pos2).normalized;
        rb.linearVelocity = nextPos * speed;
        ani.SetBool("1_Move", true);
    }

    void enemyAttack()
    {
        if (myPrefabIndex == 8 || myPrefabIndex == 10) {
            ani.SetTrigger("2_Attack");
            GameManager.instance.enemy_Fire.Enemy_Fire(9, transform.position, damage);  // 9 is enemy attack prefab(Skull_M Bullet prefab)
        }
        else {
            GameManager.instance.player.hp -= damage;
            ani.SetTrigger("2_Attack");
        }
    }

    void Dead()
    {
        isLive = false;
        rb.linearVelocity = Vector3.zero;
        ani.SetTrigger("4_Death");
        ani.SetBool("isDeath", true);

        scanner.enabled = false;
        coll.enabled = false;

        GameManager.instance.player.kill += 1;
        GameManager.instance.player.exp += exp;

        GetComponentInParent<Spawner>().Respawn(transform.parent, 10f, myPrefabIndex);

        StartCoroutine(DisableAfterSeconds(3f));
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator DamagedKnockBack(Vector3 pos)
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

    void Skull_Skill()
    {
        Transform meteor = GameManager.instance.poolManager.Get(11).transform;  // 11 is meteor prefab
        meteor.position = GameManager.instance.player.transform.position;
        meteor.parent = GameManager.instance.enemy_Fire.transform;
    }

}
