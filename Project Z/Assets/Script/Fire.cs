using UnityEngine;

public class Fire : MonoBehaviour
{
    public float damage;
    public int penetration;
    public float fireSpeed = 5;

    Rigidbody2D rb;
    SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        GameManager.instance.fire = this;
    }

    public void Init(Vector3 dir, float damage)
    {
        this.damage = damage;
        rb.linearVelocity = dir * fireSpeed;
        sr.flipX = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽에 맞으면 사라지고 위치를 arrowSpawnPoint로 이동
        if (collision.CompareTag("Wall")) {
            ResetArrow();
            return;
        }
        if (!collision.CompareTag("Player")) return;

        GameManager.instance.player.hp -= damage;
        penetration--;

        GameManager.instance.effect.ArrowOnEffect(collision.transform.position, 0);

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
        gameObject.SetActive(false);
    }
}
