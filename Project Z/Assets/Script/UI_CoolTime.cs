using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoolTime : MonoBehaviour
{
    public enum CoolTimeType { HealPack, BaseAttack, Skill_0, Skill_1, Skill_2, Dash, Home }
    public CoolTimeType coolTimeType;

    [SerializeField] float coolTime;
    [SerializeField] float maxCoolTime;

    public Text timer;
    public Image disable;
    public Text healpackCount;

    private void Start()
    {
        UpdateCoolTime();
    }

    public void UpdateCoolTime()
    {
        switch (coolTimeType) {
            case CoolTimeType.HealPack:
                maxCoolTime = GameManager.instance.player.heal_CoolDown;
                GameManager.instance.player.ui_heal_pack = this;
                healpackCount.text = GameManager.instance.player.healPack_Cnt.ToString();
                break;
            case CoolTimeType.BaseAttack:
                maxCoolTime = GameManager.instance.player.reroad;
                GameManager.instance.player.ba_obj = this;
                break;
            case CoolTimeType.Skill_0:
                maxCoolTime = GameManager.instance.player.skill_0;
                GameManager.instance.player.skill0 = this;
                break;
            case CoolTimeType.Skill_1:
                maxCoolTime = GameManager.instance.player.skill_1;
                GameManager.instance.player.skill1 = this;
                break;
            case CoolTimeType.Skill_2:
                maxCoolTime = GameManager.instance.player.skill_2;
                GameManager.instance.player.skill2 = this;
                break;
            case CoolTimeType.Dash:
                maxCoolTime = GameManager.instance.player.dashCooldown;
                GameManager.instance.player.dash = this;
                break;
            case CoolTimeType.Home:
                maxCoolTime = GameManager.instance.player.recoll_Home;
                GameManager.instance.player.home = this;
                break;
        }
    }

    public IEnumerator CoolTimefunc()
    {
        SetSkillAvailable(false);
        coolTime = maxCoolTime; // ���� �� ��Ÿ�� �ִ�ġ

        while (coolTime > 0f) {
            coolTime -= Time.deltaTime; // �ð��� �������� �پ��
            disable.fillAmount = coolTime / maxCoolTime;

            // ���� �ð� ǥ��
            if (coolTime >= 60f) {
                int min = Mathf.FloorToInt(coolTime / 60);
                int sec = Mathf.FloorToInt(coolTime % 60);
                timer.text = string.Format("{0:D2}:{1:D2}", min, sec);
            }
            else {
                timer.text = string.Format("{0:0.0}s", coolTime);
            }

            yield return null; // �����Ӹ��� ����
        }

        // ��Ÿ�� ������ �ʱ�ȭ
        if (coolTimeType == CoolTimeType.HealPack) {
            healpackCount.text = GameManager.instance.player.healPack_Cnt.ToString();

            if (GameManager.instance.player.healPack_Cnt == 0) {
                SetSkillAvailable(false);
                GameManager.instance.player.ui_heal_pack.disable.fillAmount = 1f;
                timer.text = "Empty";
                yield break; // ���⼭ ������ �ؿ� �����ڵ� ���� �� ��
            }
        }

        // HealPack�� �ƴϰų�, ������ 0 �̻��� �� ����Ǵ� ���� ó��
        disable.fillAmount = 0f;
        timer.text = "Ready";
        SetSkillAvailable(true);
    }

    public void StartCoolTime()
    {
        StartCoroutine(CoolTimefunc());
    }

    private void SetSkillAvailable(bool value)
    {
        switch (coolTimeType) {
            case CoolTimeType.HealPack:
                GameManager.instance.player.heal_Pack = value;
                break;
            case CoolTimeType.BaseAttack:
                GameManager.instance.player.baseAttack = value;
                break;
            case CoolTimeType.Skill_0:
                GameManager.instance.player.learn_Skill0 = value;
                break;
            case CoolTimeType.Skill_1:
                GameManager.instance.player.learn_Skill1 = value;
                break;
            case CoolTimeType.Skill_2:
                GameManager.instance.player.learn_Skill2 = value;
                break;
            case CoolTimeType.Dash:
                GameManager.instance.player.can_Dash = value;
                break;
            case CoolTimeType.Home: 
                GameManager.instance.player.can_Home = value;
                break;
                // �ʿ��ϸ� �߰�
        }
    }
}
