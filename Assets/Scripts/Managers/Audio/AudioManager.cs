using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource randFXSource;
    public AudioSource nonRandFXSource;
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
    public AudioClip fullAggro;
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

    [Header("Other Audio Clips")]
    public AudioClip tab;
    public AudioClip HoldSelect;
    public AudioClip select;

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

        // DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        randFXSource.volume = 0.5f;
        nonRandFXSource.volume = 0.5f;
        musicSource.volume = 0.5f;
    }

    public void PlayFX(AudioClip clip, bool random)
    {
        if (random)
        {
            float _randomPitch = Random.Range(pitchLow, pitchHigh);

            randFXSource.pitch = _randomPitch;
            randFXSource.PlayOneShot(clip);
        }
        else
        {
            nonRandFXSource.PlayOneShot(clip);
        }
    }

    public void PlayFX(AudioClip clip, float pLow, float pHigh)
    {
        randFXSource.clip = clip;
        float _randomPitch = Random.Range(pLow, pHigh);

        randFXSource.pitch = _randomPitch;
        randFXSource.PlayOneShot(clip);

    }

    public void RandomSFX(params AudioClip[] clips)
    {
        int _randomIndex = Random.Range(0, clips.Length);
        float _randomPitch = Random.Range(pitchLow, pitchHigh);

        randFXSource.pitch = _randomPitch;
        randFXSource.clip = clips[_randomIndex];
        randFXSource.PlayOneShot(clips[_randomIndex]);
    }

    public void RandomSFX(AudioClip[] clips, float pLow, float pHigh)
    {
        int _randomIndex = Random.Range(0, clips.Length);

        randFXSource.clip = clips[_randomIndex];
        PlayFX(randFXSource.clip, pLow, pHigh);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Use this for sfx that will play all the time, even or after if the object is destroyed
    public void PlayIndependent(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }

    public void StopSFX()
    {
        nonRandFXSource.Stop();
    }
}

