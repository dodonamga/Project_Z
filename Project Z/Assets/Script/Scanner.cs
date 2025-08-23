using UnityEngine;

// 몬스터가 플레이어를 감지하는 스크립트
public class Scanner : MonoBehaviour
{
    // 플레이어 찾는 스캐너 변수
    [SerializeField] float scanRange = 3f;
    [SerializeField] LayerMask targetLayer;
    public Collider2D target;
    // 공격 범위 안에 들어오면 사용할 변수
    [SerializeField] float attackRange = 0.8f;
    [SerializeField] Collider2D inAttackRange;

    private Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        target = Physics2D.OverlapCircle(transform.position, scanRange, targetLayer);

        if (target != null) {
            enemy.findTarget = true;

            //  자동 연동으로 복제나 프리팹에도 안전
            if (enemy.target == null || enemy.target != target.attachedRigidbody)
                enemy.target = target.attachedRigidbody;
        }
        else {
            enemy.findTarget = false;
            //enemy.target = null;
        }
        
    }

    // 몬스터 범위 내에 플레이어를 탐지할 로직
    public bool AttackRange()
    {
        inAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, targetLayer);
        if (inAttackRange != null) {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        // 스캔 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, scanRange);

        // 공격 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
