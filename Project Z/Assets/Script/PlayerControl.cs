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
    public int healPack_Cnt = 0;

    [Header("#.. PlayerFlipX")]
    Vector3 cuserPoint;
    public Vector3 playerScale;

    [Header("#.. Dash")]
    [SerializeField] float dashPower = 10f;      // 대시 속도
    [SerializeField] float dashDuration = 0.2f;  // 대시 유지 시간
    public float dashCooldown = 3.0f;  // 대시 쿨타임
    [SerializeField] float dashTimer;
    public bool isDashing;
    [SerializeField] Vector2 dashDir;

    [Header("#.. Timer")]
    public float heal_CoolDown = 1;
    public float recoll_Home = 30;
    public float reroad = 1;
    public float skill_0 = 15;
    public float skill_1 = 15;
    public float skill_2 = 30;

    [Header("Skill Condition")]
    public bool baseAttack, can_Dash, can_Home = true;
    public bool heal_Pack, learn_Skill0, learn_Skill1, learn_Skill2;
    public UI_CoolTime ui_heal_pack, ba_obj, skill0, skill1, skill2, dash, home;


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

    private void Update()
    {
        if (!GameManager.instance.isLive) return;
        if (hp < 0) Dead();

        levelUp();

        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

        // 대시 타이머 갱신
        if (isDashing) {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration) {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            if (heal_Pack && healPack_Cnt > 0) {
                UsedHealPack();
                ui_heal_pack.StartCoolTime();
            }
            else Debug.Log("CoolTime or not have healpack");
        }

        if (Input.GetKey(KeyCode.B)) {
            if (can_Home) {
                Home();
                home.StartCoolTime();
            }
            else Debug.Log("Home CoolTime");
        }

        int index;
        if (Input.GetMouseButtonDown(0) && GameManager.instance.isLive) {
            if (ba_obj == null) return;
            if (baseAttack != false) {
                index = 0;
                //timer_defaultAttack = 0;
                ani.SetTrigger("2_Attack");
                ba_obj.StartCoolTime();

            } else Debug.Log("CoolTime");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager.instance.tripleArrow.level > 0) {
            if (learn_Skill0) {
                index = 0;
                PlayerSkill0(index);
                skill0.StartCoolTime();
            }
            else Debug.Log("Not Learn Skill 0");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && GameManager.instance.Arrow_Big.level > 0) {
            if (learn_Skill1) {
                index = 1;
                PlayerSkill1(index);
                skill1.StartCoolTime();
            }
            else Debug.Log("Not Learn Skill 2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && GameManager.instance.Boom_.level > 0) {
            if (learn_Skill2) {
                index = 2;
                PlayerSkill2(index);
                skill2.StartCoolTime();
            }
            else Debug.Log("Not Learn Skill 3");
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) {
            if (can_Dash) {
                Dash();
                dash.StartCoolTime();
            }
            else Debug.Log("Dash is CoolDown");
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
        dashDir = (curVec - transform.position).normalized;

        isDashing = true;
        dashTimer = 0;
    }

    public void Dead()
    {
        ani.SetTrigger("4_Death");

        GameManager.instance.GameOver();
    }

    void UsedHealPack()
    {
        healPack_Cnt--;
        hp += maxhp * 0.8f;
        if (hp < maxhp) hp = maxhp;
    }
    public void DefaultAttack()
    {
        PlayerCuserFlipX();
        GameManager.instance.ArrowDefault.ShotArrow(0);
    }

    public void PlayerSkill0(int index)
    {
        //timer_Skill0 = 0;
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
        PlayerCuserFlipX();
        GameManager.instance.ArrowBig.ShotArrow(index);
    }

    public void PlayerSkill2(int index)
    {
        Vector3 cuserPos = GameManager.instance.cuser.GetmousePoint();
        Vector3 dirVec = transform.position - cuserPos;

        float moveDistance = 1.5f; // 원하는 넉백 거리(단위: world units)

        // 새 위치 계산
        Vector3 targetPos = transform.position + dirVec.normalized * moveDistance;

        // MovePosition으로 이동
        rb.MovePosition(targetPos);

        StartCoroutine(invincibility(0.3f));
        GameManager.instance.Boom.Boom(index);
    }
}
