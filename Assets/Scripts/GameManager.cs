using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 playerStartPos;

    [Header("Lives Settings")]
    public int maxLives = 3;
    [HideInInspector] public int currentLives;

    [Header("UI")]
    public TMP_Text livesText;       // TextMeshPro
    public GameObject gameOverUI;    // Game Over panel

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentLives = maxLives;
    }


    void Start()
    {
    // store initial player position
    Player player = Object.FindFirstObjectByType<Player>();
    if (player != null)
        playerStartPos = player.transform.position;

    UpdateLivesUI();
    if (gameOverUI != null)
        gameOverUI.SetActive(false);
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
    // Find LivesText in the current scene
    TMP_Text tmp = GameObject.Find("LivesText")?.GetComponent<TMP_Text>();
    if (tmp != null)
        livesText = tmp;

    // Find GameOver panel in the current scene
    GameObject goUI = GameObject.Find("GameOverUI");
    if (goUI != null)
        gameOverUI = goUI;

    // Hide GameOver UI at the start of the scene
    if (gameOverUI != null)
        gameOverUI.SetActive(false);

    // Reset lives if this is a fresh start of GameScene
    if (scene.name == "GameScene" && currentLives <= 0)
        currentLives = maxLives;

    // Update Lives UI
    UpdateLivesUI();

    // --- COINS ---
    if (CoinManager.Instance != null)
    {
        TMP_Text newCoinText = GameObject.Find("CoinTxt")?.GetComponent<TMP_Text>();
        if (newCoinText != null)
            CoinManager.Instance.coinText = newCoinText;

        // Reset coins counter and update UI
        CoinManager.Instance.ResetCoins();
    }

}
    public void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            RespawnPlayer();
        }
    }

    public void AddLife(int amount)
    {
        currentLives += amount;   // NO LIMIT
        UpdateLivesUI();
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }


void RespawnPlayer()
{
    Player player = Object.FindFirstObjectByType<Player>();
    if (player != null)
    {
        // Move player to start position
        player.transform.position = playerStartPos;
        player.health = 100;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;       // stop all motion
            rb.angularVelocity = 0f;          // stop rotation spin
            rb.isKinematic = false;           // make sure physics is active
            rb.Sleep();                        // reset physics state
        }

        // Re-enable Player script
        player.enabled = true;

        // Resume camera following
        CameraFollow cam = Object.FindFirstObjectByType<CameraFollow>();
        if (cam != null)
            cam.ResumeFollowing();
    }
}

    void GameOver()
    {
        // Stop level BGM
        if (GameAudioManager.instance != null)
            GameAudioManager.instance.StopBGM();

        if (GameAudioManager.instance != null)
        GameAudioManager.instance.PlayGameOver();

        // Show Game Over UI if exists
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Delay returning to StartMenuScene
        Invoke(nameof(ReturnToStartMenu), 3f);
    }

    void ReturnToStartMenu()
    {
        // Reset lives
        currentLives = maxLives;
        
        UpdateLivesUI();

        // Load Start Menu
        SceneManager.LoadScene("StartMenuScene");
    }
}