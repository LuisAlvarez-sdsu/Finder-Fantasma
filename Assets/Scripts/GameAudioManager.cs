using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager instance;

    [Header("Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip levelBGM; // assign your GameScene music here
    public AudioClip jumpSound;
    public AudioClip playerDamage;
    public AudioClip ghostDamage;
    public AudioClip coinPickup;
    public AudioClip batteryPickup;
    public AudioClip gameOverSound;

    [Header("Ghost Damage Settings")]
    public float ghostDamageCooldown = 0.2f;
    private float lastGhostDamageTime = 0f;

    [Header("Pitch Variation")]
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
     if (scene.name == "GameScene")
    {
    if (GameAudioManager.instance != null)
        GameAudioManager.instance.PlayLevelBGM(); //  force restart
    }   
    }

    public void PlayLevelBGM()
    {
        if (bgmSource == null || levelBGM == null)
            return;

        bgmSource.Stop();              // force reset
        bgmSource.clip = levelBGM;     // ensure correct clip
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // Helper to play sound with random pitch
    private void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(clip, volume);
        sfxSource.pitch = 1f;
    }

    public void PlayJump() => PlaySFX(jumpSound);
    public void PlayPlayerDamage() => PlaySFX(playerDamage);

    public void PlayGhostDamage()
    {
        if (Time.time - lastGhostDamageTime < ghostDamageCooldown)
            return;

        PlaySFX(ghostDamage);
        lastGhostDamageTime = Time.time;
    }

    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    public void PlayCoinPickup() => PlaySFX(coinPickup, 1.5f);
    public void PlayBatteryPickup() => PlaySFX(batteryPickup, 1.5f);
    public void PlayGameOver() => PlaySFX(gameOverSound, 1.2f);
}