using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueBox;   // The panel
    public TMP_Text dialogueText;    // The text to show

    [Header("Dialogue")]
    [TextArea]
    public string message;

    [Header("Settings")]
    public float showDelay = 0.2f;       // Small delay before showing
    public float typingSpeed = 0.05f;    // Time per character

    private Coroutine showCoroutine;

    void Start()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);  // Start hidden
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (showCoroutine != null)
                StopCoroutine(showCoroutine);

            showCoroutine = StartCoroutine(ShowDialogue());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (showCoroutine != null)
                StopCoroutine(showCoroutine);

            if (dialogueBox != null)
                dialogueBox.SetActive(false);
        }
    }

    IEnumerator ShowDialogue()
    {
        // Optional small delay
        if (showDelay > 0)
            yield return new WaitForSeconds(showDelay);

        if (dialogueBox != null && dialogueText != null)
        {
            dialogueBox.SetActive(true);
            dialogueText.text = "";

            // Typing effect
            foreach (char c in message.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}