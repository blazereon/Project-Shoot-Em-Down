using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource fxSource;
    public AudioSource musicSource;

    [Header("Random Pitch Range")]
    public float pitchLow = 0.95f;
    public float pitchHigh = 1.10f;

    [Header("Player Audio Clips")]
    public AudioClip[] playerIdle;
    public AudioClip[] playerWalk;
    public AudioClip[] playerRun;
    public AudioClip[] playerJump;
    public AudioClip[] playerLand;
    public AudioClip[] playerAttackMelee;
    public AudioClip[] playerAttackRanged;
    public AudioClip[] playerDash;
    public AudioClip[] playerWallGrab;
    public AudioClip[] playerWallJump;
    public AudioClip[] playerTakeDmg;
    public AudioClip playerDeath;

    [Header("Enemy Audio Clips")]
    public AudioClip[] enemyChaseAlert;
    public AudioClip[] enemyAttackMelee;
    public AudioClip[] enemyAttackRanged;
    public AudioClip[] enemyTakeDmg;
    public AudioClip[] enemyTakeDmgWeakSpot;
    public AudioClip[] enemyShieldDeflect;
    public AudioClip explosionCoundown;
    public AudioClip explosion;
    public AudioClip enemyDeath;

    [Header("Music BGM")]
    public AudioClip levelBGM;

    // Singleton Instance
    [HideInInspector]
    public static AudioManager instance = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        fxSource.volume = 0.5f;
        musicSource.volume = 0.5f;
    }

    public void PlayFX(AudioClip clip, bool random)
    {
        fxSource.clip = clip;
        fxSource.pitch = 1f;
        if (random)
        {
            float _randomPitch = Random.Range(pitchLow, pitchHigh);

            fxSource.pitch = _randomPitch;
            fxSource.PlayOneShot(clip);
        }
        else
        {
            fxSource.PlayOneShot(clip);
        }
    }

    public void PlayFX(AudioClip clip, float pLow, float pHigh)
    {
        fxSource.clip = clip;
        float _randomPitch = Random.Range(pLow, pHigh);

        fxSource.pitch = _randomPitch;
        fxSource.PlayOneShot(clip);

    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void RandomSFX(params AudioClip[] clips)
    {
        int _randomIndex = Random.Range(0, clips.Length);
        float _randomPitch = Random.Range(pitchLow, pitchHigh);

        fxSource.pitch = _randomPitch;
        fxSource.clip = clips[_randomIndex];
        fxSource.PlayOneShot(clips[_randomIndex]);
    }

    public void RandomSFX(AudioClip[] clips, float pLow, float pHigh)
    {
        int _randomIndex = Random.Range(0, clips.Length);

        fxSource.clip = clips[_randomIndex];
        PlayFX(fxSource.clip, pLow, pHigh);
    }
}

