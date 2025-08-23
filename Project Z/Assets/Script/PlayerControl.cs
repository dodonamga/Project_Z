using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("#.. Player Info")]
    public int level = 0;
    public float hp;
    public float maxhp = 300;
    public float exp;
    public int[] nextExp = { 10, 20, 30 , 50, 60, 80, 100 };
    public int kill;
    public float str = 0.5f;    // 몬스터를 밀어내는 힘
    [SerializeField] Vector2 movePos; // 입력 위치값
    public float moveSpeed = 10f; // 이동속도

    [Header("#.. PlayerFlipX")]
    Vector3 cuserPoint;
    public Vector3 playerScale;

    [Header("#.. Dash")]
    [SerializeField] float dashPower = 10f;      // 대시 속도
    [SerializeField] float dashDuration = 0.2f;  // 대시 유지 시간
    [SerializeField] float dashCooldown = 1.0f;  // 대시 쿨타임
    float dashTimer;
    float dashCoolTimer;
    bool isDashing;
    Vector2 dashDir;

    [Header("#.. Timer")]
    [SerializeField] float timer_Home;
    [SerializeField] float timer_defaultAttack; // reroad보다 높아야 공격 가능
    public float reroad = 1;    // 재장전시간
    [SerializeField] float timer_Skill0;
    public int skill_0_CoolTime = 15;
    [SerializeField] float timer_Skill1;
    public int skill_1_CoolTime = 15;
    [SerializeField] float timer_Skill2;
    public int skill_2_CoolTime = 30;

    Animator ani;
    Rigidbody2D rb;
    Collider2D coll;
    
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        ani = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();  
        playerScale = transform.localScale;
    }

    private void Start()
    { 
        hp = maxhp;
    }

    private void Update()
    {
        if (!GameManager.instance.isLive) return;
        timer_Home += Time.deltaTime;

        levelUp();

        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

        // ..timer
        timer_defaultAttack += Time.deltaTime;
        timer_Skill0 += Time.deltaTime;
        timer_Skill1 += Time.deltaTime;

        // 대시 타이머 갱신
        if (isDashing) {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration) {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
                dashCoolTimer = dashCooldown; // 쿨타임 시작
            }
        }
        else if (dashCoolTimer > 0) {
            dashCoolTimer -= Time.deltaTime;
        }

        // position to -20, 0, 0 (Start Position)
        if (Input.GetKey(KeyCode.B) && timer_Home > 30f) {
            Home();
        }

        int index;
        if (Input.GetMouseButtonDown(0) && timer_defaultAttack >= reroad) {
            index = 0;
            timer_defaultAttack = 0;
            ani.SetTrigger("2_Attack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && timer_Skill0 >= skill_0_CoolTime && GameManager.instance.item1.level > 0) {
            index = 0;
            PlayerSkill0(index);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && timer_Skill1 >= skill_1_CoolTime && GameManager.instance.item2.level > 0) {
            index = 1;
            PlayerSkill1(index);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;

        if (!isDashing) {
            Vector2 nextPos = movePos.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = nextPos * moveSpeed;
        }
        else {
            rb.linearVelocity = dashDir * dashPower;
        }

        //Vector2 nextPos = movePos.normalized * moveSpeed * Time.fixedDeltaTime;
        ////rb.MovePosition(nextPos + rb.position);
        //rb.linearVelocity = nextPos * moveSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCoolTimer <= 0) {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            PLayerSkill2(4);    // 4 is Boom prefab;
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;

        if (movePos.magnitude != 0) {
            playerScale.x = movePos.x < 0 ? 1 : -1;
            transform.localScale = playerScale;
            ani.SetBool("1_Move", true);
        }
        else {
            ani.SetBool("1_Move", false);
        }
    }

    void levelUp()
    {
        int maxLevelIndex = nextExp.Length - 1;

        if (exp == nextExp[Mathf.Min(level, maxLevelIndex)]) {
            hp = maxhp;
            level++;
            exp = 0;
            GameManager.instance.uiLevelUp.Show();
        }

        else if (exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)]) {
            hp = maxhp;
            exp -= nextExp[Mathf.Min(level, maxLevelIndex)];
            level++;
            GameManager.instance.uiLevelUp.Show();
        }

    }

    void PlayerCuserFlipX()
    {
        cuserPoint = GameManager.instance.cuser.GetmousePoint();
        if (cuserPoint.x < transform.position.x) {
            playerScale.x = 1;
            transform.localScale = playerScale;
        }
        else {
            playerScale.x = -1;
            transform.localScale = playerScale;
        }
    }

    IEnumerator invincibility(float time)
    {
        coll.enabled = false;
        yield return new WaitForSeconds(time);
        coll.enabled = true;
    }

    // ...Attack and Player Skill
    void Home()
    {
        transform.position = new Vector3(-20, 0, 0);
    }

    void Dash()
    {
        Vector3 curVec = GameManager.instance.cuser.GetmousePoint();
        //rb.linearVelocity = (curVec - transform.position).normalized * 10f;
        dashDir = (curVec - transform.position).normalized;

        isDashing = true;
        dashTimer = 0;
        //StartCoroutine(invincibility(0.3f));
    }

    public void DefaultAttack()
    {
        PlayerCuserFlipX();
        GameManager.instance.ArrowDefault.ShotArrow(0);
    }

    public void PlayerSkill0(int index)
    {
        timer_Skill0 = 0;
        PlayerCuserFlipX();

        Vector3 shotPos = GameManager.instance.cuser.GetmousePoint();
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 baseDir = (shotPos - playerPos).normalized;

        float[] angles = { -10f, 0f, 10f };

        foreach (float angle in angles) {
            // Z축 기준으로 baseDir 벡터 회전
            Vector3 rotatedDir = Quaternion.Euler(0, 0, angle) * baseDir;
            GameManager.instance.ArrowDefault.ShotArrow(rotatedDir.normalized, index);
        }
    }

    public void PlayerSkill1(int index)
    {
        timer_Skill1 = 0;
        PlayerCuserFlipX();
        GameManager.instance.ArrowBig.ShotArrow(index);
    }

    public void PLayerSkill2(int index)
    {
        timer_Skill2 = 0;

        Vector3 cuserPos = GameManager.instance.cuser.GetmousePoint();
        Vector3 dirVec = transform.position - cuserPos;

        Debug.Log("cuser " + cuserPos + "\ndir " + dirVec);
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dirVec.normalized * 60f, ForceMode2D.Impulse);
        StartCoroutine(invincibility(0.3f));
        GameManager.instance.Boom.Boom(index);
    }
}
