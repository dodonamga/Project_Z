using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour {
    public void ArrowOnEffect(Vector3 effectPos, int index)
    {
        effectPos.y += 0.3f;
        OnEffectAni(effectPos, index);
    }

    public void BoomOnEffect(Vector3 effectPos, int index)
    {
        effectPos.y += 2.5f;
        OnEffectAni(effectPos, index);
    }

    public void OnEffectAni(Vector3 effectPos, int index)
    {
        GameObject effect = GameManager.instance.poolManager.Get(index);
        effect.transform.position = effectPos;
        effect.transform.parent = transform;
        Animator animator = effect.GetComponent<Animator>();
        if (index == 2) {
            animator.SetTrigger("isHit");
        }
        else {
            animator.SetTrigger("isBoom");
        }
        StartCoroutine(DisableAfterSeconds(effect, 0.25f));
    }

    IEnumerator DisableAfterSeconds(GameObject go, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        go.gameObject.SetActive(false);
    }
}
