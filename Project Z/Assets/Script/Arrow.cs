using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public float damage;
    public int penetration;
    public float arrowSpeed = 1;

    Rigidbody2D rb;
    SpriteRenderer sr;
    // 화살을 쏜 순간의 위치를 갖는 변수
    public Vector3 shotPos;

    private HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        GameManager.instance.arrow = this;
    }

    private void Update()
    {
        shotPos = GameManager.instance.player.transform.position;
    }

    public void Init(float damage, int penetration, Vector3 dir)
    {
        this.damage = damage;
        this.penetration = penetration;

        rb.linearVelocity = dir * arrowSpeed;
        sr.flipX = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽에 맞으면 사라지고 위치를 arrowSpawnPoint로 이동
        if (collision.CompareTag("Wall")) {
            ResetArrow();
            return;
        }
        if (!collision.CompareTag("Enemy")) return;
        if (hitTargets.Contains(collision)) return;

        hitTargets.Add(collision);
        penetration--;

        GameManager.instance.effect.ArrowOnEffect(collision.transform.position, 2);

        if (penetration <= 0) {
            ResetArrow();
        }
    }

    private void ResetArrow()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = GameManager.instance.player.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 90f);
        sr.flipX = false;
        hitTargets.Clear();
        gameObject.SetActive(false);
    }

}
