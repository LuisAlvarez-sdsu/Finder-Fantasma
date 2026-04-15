using UnityEngine;
using TMPro; // For TextMeshPro

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [HideInInspector] public int currentCoins = 0;

    [Header("Settings")]
    public int maxCoins = 0; // Total coins in the level

    [Header("UI")]
    public TMP_Text coinText; // Reference to the TextMeshPro UI element

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
    }

    void OnEnable()
    {
    UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
    // Try to find the CoinText in the new scene
    TMP_Text tmp = GameObject.Find("CoinText")?.GetComponent<TMP_Text>();
    if (tmp != null)
        coinText = tmp;

    // Update UI immediately
    UpdateCoinUI();
    }

    public int CurrentCoins
    {
        get { return currentCoins; }
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
        Debug.Log("Coins: " + currentCoins + "/" + maxCoins);
    }

    public void ResetCoins()
    {
        currentCoins = 0;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = $"Coins: {currentCoins}/{maxCoins}";
    }
}