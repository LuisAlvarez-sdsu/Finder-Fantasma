using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startMenuPanel;
    public GameObject introPanel;
    public Image introImageDisplay;
    public GameObject fadePanel;
    public float fadeDuration = 0.5f;

    [Header("Intro Images")]
    public Sprite[] introImages;
    public float imageDuration = 1.5f;

    [Header("Scene")]
    public string nextSceneName = "GameScene";

    [Header("Audio")]
    public AudioSource audioSource;       // main audio source (menu music)
    public AudioSource musicSource2;      // second source for crossfade
    public AudioClip buttonClickSound;
    public AudioClip introMusic;
    public float musicFadeDuration = 1f;

    private CanvasGroup introCanvasGroup;
    private CanvasGroup fadeCanvasGroup;

    void Awake()
    {
        // Intro panel setup
        introCanvasGroup = introPanel.GetComponent<CanvasGroup>();
        if (introCanvasGroup == null)
            introCanvasGroup = introPanel.AddComponent<CanvasGroup>();
        introCanvasGroup.alpha = 0f;

        // Fade panel setup
        fadeCanvasGroup = fadePanel.GetComponent<CanvasGroup>();
        if (fadeCanvasGroup == null)
            fadeCanvasGroup = fadePanel.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f;

        introPanel.SetActive(false);
        fadePanel.SetActive(true);
    }

    void Start()
    {
        startMenuPanel.SetActive(true);
    }

    public void PlayGame()
    {
        // Play button click
        audioSource.PlayOneShot(buttonClickSound);

        startMenuPanel.SetActive(false);

        // 🎵 Start crossfade to intro music
        StartCoroutine(CrossfadeMusic(introMusic));

        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        introPanel.SetActive(true);

        foreach (Sprite img in introImages)
        {
            introImageDisplay.sprite = img;

            yield return StartCoroutine(FadeCanvas(introCanvasGroup, 0f, 1f, fadeDuration));
            yield return new WaitForSeconds(imageDuration);
            yield return StartCoroutine(FadeCanvas(introCanvasGroup, 1f, 0f, fadeDuration));
        }

        // Fade to black
        yield return StartCoroutine(FadeCanvas(fadeCanvasGroup, 0f, 1f, fadeDuration));

        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float start, float end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }

    //  Smooth crossfade between music tracks
    IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        musicSource2.clip = newClip;
        musicSource2.volume = 0f;
        musicSource2.loop = true;
        musicSource2.Play();

        float timer = 0f;

        float startVolume1 = audioSource.volume;
        float targetVolume2 = startVolume1;

        while (timer < musicFadeDuration)
        {
            float t = timer / musicFadeDuration;

            audioSource.volume = Mathf.Lerp(startVolume1, 0f, t);
            musicSource2.volume = Mathf.Lerp(0f, targetVolume2, t);

            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        musicSource2.volume = targetVolume2;

        audioSource.Stop();

        // Swap sources (so audioSource is always main)
        AudioSource temp = audioSource;
        audioSource = musicSource2;
        musicSource2 = temp;
    }

    void Update()
    {
        if (introPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SkipIntro());
        }
    }

    IEnumerator SkipIntro()
    {
        introPanel.SetActive(false);

        yield return StartCoroutine(FadeCanvas(fadeCanvasGroup, 0f, 1f, fadeDuration));

        SceneManager.LoadScene(nextSceneName);
    }
}