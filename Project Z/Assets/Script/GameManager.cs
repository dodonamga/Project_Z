using NUnit.Framework.Internal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("#.. GameInfo")]
    public bool isLive = true;  // 게임 시간이 흐르는지 구분하는 변수
    public float gameTime;
    public float maxGameTime;

    [Header("#.. Manager and children")]
    public PoolManager poolManager;
    public Effect effect;

    [Header("#.. GameObject")]
    public PlayerControl player;
    public Cuser cuser;
    public Enemy enemy;
    public Arrow arrow;
    public LevelUp uiLevelUp;

    [Header("#.. Spawner")]
    public Spawner room0Spawner;

    [Header("#.. Weapons")]
    public Weapons ArrowDefault;
    public Weapons ArrowBig;
    public Weapons Boom;

    [Header("#.. Items")]
    public Item item0;
    public Item item1;
    public Item item2;

    private void Awake()
    {
        instance = this;
        maxGameTime = 30 * 60f;
    }

    private void Start()
    {
        uiLevelUp.Select(0);
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
