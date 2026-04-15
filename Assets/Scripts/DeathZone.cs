using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    public CameraFollow cameraFollow;

private void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag("Player"))
        return;

    Player player = other.GetComponent<Player>();
    if (player == null)
        return;

    Debug.Log("Player died in DeathZone!");

    // Stop camera
    if (cameraFollow != null)
        cameraFollow.StopFollowing();

    // Stop movement velocity only, keep physics active
    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
    if (rb != null)
        rb.linearVelocity = Vector2.zero;

    // Disable control temporarily
    player.enabled = false;

    // Delay death
    if (GameManager.Instance != null)
        StartCoroutine(HandleDeath());
}

private IEnumerator HandleDeath()
{
    yield return new WaitForSeconds(0.5f); // small pause
    GameManager.Instance.LoseLife();
}
}