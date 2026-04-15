using UnityEngine;
using TMPro;
using System.Collections;

public class EndNPCDialogue : MonoBehaviour
{
    [Header("UI & Feedback")]
    public GameObject dialoguePanel;       // Panel for messages
    public TMP_Text dialogueText;          // Text inside panel

    [Header("Door Settings")]
    public Animator doorAnimator;          // Animator for the door
    public string doorOpenTrigger = "Open"; // Animator trigger to open door

    [Header("Coins Requirement")]
    public int requiredCoins = 5;          // Coins required to open door

    [Header("Typing Effect")]
    public float typingSpeed = 0.05f;

    [Header("Door Settings")]
    public DoorTrigger doorTrigger; // Assign in Inspector

    private bool doorOpened = false;
    private Coroutine typingCoroutine;

    void Start()
    {
    if (dialoguePanel != null)
        dialoguePanel.SetActive(false);

    // Close door when the scene starts/replays
    if (doorTrigger != null)
        doorTrigger.CloseDoor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || doorOpened)
            return;

        // Stop any ongoing typing
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Check coins collected
        bool hasAllCoins = CoinManager.Instance != null && CoinManager.Instance.CurrentCoins >= requiredCoins;

        if (hasAllCoins)
        {
         typingCoroutine = StartCoroutine(ShowMessage("Good job! You got all my coins!"));

    // Open the door via the DoorTrigger instance
         if (doorTrigger != null)
             doorTrigger.OpenDoor();

          doorOpened = true; // Prevent reopening
        }
        else
        {
            typingCoroutine = StartCoroutine(ShowMessage("Go get all my coins!"));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private IEnumerator ShowMessage(string message)
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "";

            foreach (char c in message.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}