using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public enum Sfx { Dead, Hit0, Hit1, LevelUp, Lose, Melee0, Melee1, Range, Select, Win}

    [Header("#.. BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    public AudioSource bgmPlayer;
    AudioHighPassFilter bgmHighPassFilter;

    [Header("#.. SFX")]
    public AudioClip[] sfxClips;
    public int channels;
    public float sfxVolume;
    public AudioSource[] sfxPlayers;
    int channelsIndex;

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bjmObject = new GameObject("BgmPlayer");
        bjmObject.transform.parent = transform;
        bgmPlayer = bjmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for (int index = 0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
            sfxPlayers[index].bypassListenerEffects = true;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay) bgmPlayer.Play();
        else bgmPlayer.Stop();
    }

    public void filter(bool isPlay)
    {
        bgmHighPassFilter.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++) {
            int loopIndex = (index + channelsIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) continue;

            channelsIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
