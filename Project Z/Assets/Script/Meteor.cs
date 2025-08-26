using UnityEngine;

public class Meteor : MonoBehaviour
{
    Collider2D coll;
    Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        ani.SetTrigger("Skull_Skill");
    }

    public void Catch_Player()
    {
        coll.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        GameManager.instance.player.hp -= GameManager.instance.player.hp * 0.8f;
    }

    public void Reset()
    {
        coll.enabled=false;
        gameObject.SetActive(false);
    }
}
