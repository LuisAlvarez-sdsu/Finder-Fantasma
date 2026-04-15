using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Add 1 life
        if (GameManager.Instance != null)
            GameManager.Instance.AddLife(1);

        // Optional sound
        if (GameAudioManager.instance != null)
            GameAudioManager.instance.PlayBatteryPickup();

        // Remove heart
        Destroy(gameObject);
    }
}