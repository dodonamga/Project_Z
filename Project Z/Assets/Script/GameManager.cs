using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("#.. GameInfo")]
    public bool isLive = false;  // 게임 시간이 흐르는지 구분하는 변수
    public float gameTime;
    public float maxGameTime;

    [Header("#.. UI")]
    public HUD boss_HP;
    public UI_CoolTime ui_CoolTime;
    public Ui_Result ui_Result;

    [Header("#.. Manager and children")]
    public PoolManager poolManager;
    public Effect effect;

    [Header("#.. GameObject")]
    public PlayerControl player;
    public Cuser cuser;
    public Enemy enemy;
    public Arrow arrow;
    public Fire fire;
    public LevelUp uiLevelUp;
    public EnterRoom enterRoom;

    [Header("#.. Spawner")]
    public Spawner room0Spawner;

    [Header("#.. Weapons")]
    public Weapons enemy_Fire;
    public Weapons ArrowDefault;
    public Weapons ArrowBig;
    public Weapons Boom;

    [Header("#.. Items")]
    public Item baseArrow;
    public Item tripleArrow;
    public Item Arrow_Big;
    public Item Boom_;

    //... test
    public BaseEnemy baseEnemy;

    private void Awake()
    {
        instance = this;
        maxGameTime = 30 * 60f;

    }

    public void GameStart()
    {
        uiLevelUp.Select(0);
        player.hp = player.maxhp;
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        ui_Result.gameObject.SetActive(true);
        ui_Result.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVitroy()
    {
        StartCoroutine(GameVictroyRoutine());
    }

    IEnumerator GameVictroyRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        ui_Result.gameObject.SetActive(true);
        ui_Result.Win();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
        gameTime = 0f;
    }

    private void Update()
    {
        if (!isLive) return;

        gameTime += Time.deltaTime;
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
