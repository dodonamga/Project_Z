using UnityEngine;

// 몬스터가 플레이어를 감지하는 스크립트
public class Scanner : MonoBehaviour
{
    // 플레이어 찾는 스캐너 변수
    [SerializeField] float scanRange = 3f;
    [SerializeField] LayerMask targetLayer;
    public Collider2D target;
    // 공격 범위 안에 들어오면 사용할 변수
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] Collider2D inAttackRange;
    public float upAddToY;
    // Tp range

    [SerializeField] float tpScanner = 0.3f;
    [SerializeField] Collider2D canTP;


    private BaseEnemy enemy;
    private void Awake()
    {
        enemy = GetComponent<BaseEnemy>();
    }

    private void FixedUpdate()
    {
        // 기준점: 현재 위치 + Y 오프셋
        Vector3 scanOrigin = transform.position + new Vector3(0, upAddToY, 0);

        target = Physics2D.OverlapCircle(scanOrigin, scanRange, targetLayer);

        if (target != null) {
            enemy.findTarget = true;

            //  자동 연동으로 복제나 프리팹에도 안전
            if (enemy.target == null || enemy.target != target.attachedRigidbody)
                enemy.target = target.attachedRigidbody;
        }
        else {
            enemy.findTarget = false;
        }
        
    }

    // 몬스터 범위 내에 플레이어를 탐지할 로직
    public bool AttackRange()
    {
        Vector3 scanOrigin = transform.position + new Vector3(0, upAddToY, 0);

        inAttackRange = Physics2D.OverlapCircle(scanOrigin, attackRange, targetLayer);
        if (inAttackRange != null) {
            return true;
        }
        return false;
    }

    public bool TpSanner()
    {
        Vector3 scanOrigin = transform.position + new Vector3(0, upAddToY, 0);

        canTP = Physics2D.OverlapCircle(scanOrigin, tpScanner, targetLayer);
        if (canTP != null) {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 scanOrigin = transform.position + new Vector3(0, upAddToY, 0);

        // 스캔 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(scanOrigin, scanRange);

        // 공격 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(scanOrigin, attackRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(scanOrigin, tpScanner);
    }
}
