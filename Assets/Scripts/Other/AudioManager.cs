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
    public AudioClip[] playerAttack;
    public AudioClip[] playerDash;
    public AudioClip[] playerWallGrab;
    public AudioClip[] playerWallJump;
    public AudioClip[] playerTakeDmg;
    public AudioClip playerDeath;

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

    public void PlayFX(AudioClip clip)
    {
        fxSource.clip = clip;
        fxSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayFXOneShot(AudioClip clip, bool random)
    {
        fxSource.clip = clip;

        if (random)
        {
            float _randomPitch = Random.Range(pitchLow, pitchHigh);

            fxSource.pitch = _randomPitch;
            fxSource.PlayOneShot(clip);
        }
        else
        {
            fxSource.Play();
        }
    }

    public void RandomSFX(params AudioClip[] clips)
    {
        int _randomIndex = Random.Range(0, clips.Length);
        float _randomPitch = Random.Range(pitchLow, pitchHigh);

        fxSource.pitch = _randomPitch;
        fxSource.clip = clips[_randomIndex];
        fxSource.Play();
    }
}

