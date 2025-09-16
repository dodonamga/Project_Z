using System.Collections;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public float timer = 0f;
    public bool onBoom;

    public int penetration;
    public float damage;
    public float speed;     // speed is timer

    Collider2D coll;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (onBoom) {
            timer += Time.deltaTime;
        }

        if (timer > speed) {
            StartCoroutine(WaitForSec());
            onBoom = false;
        }
    }

    private void OnEnable()
    {
        onBoom = true;
    }   

    public void Init(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

    IEnumerator WaitForSec()
    {
        coll.enabled = true;
        yield return new WaitForSeconds(0.25f);
        GameManager.instance.effect.BoomOnEffect(transform.position, 1);
        timer = 0;
        coll.enabled = false;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
