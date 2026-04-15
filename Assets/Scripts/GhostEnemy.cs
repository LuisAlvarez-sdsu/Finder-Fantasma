using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 3f;
    public float damagePerSecond = 5f;
    public GameObject poofEffect;

    [Header("Flicker Effect")]
    public float flickerSpeed = 20f;
    public float flickerAmount = 0.5f;

    [Header("Detection")]
    public float detectionRadius = 6f;
    public float coneAngle = 60f;

    private float currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private PlayerFlashlight flashlight;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        currentHealth = maxHealth;

        flashlight = PlayerFlashlight.Instance;

        if (flashlight == null)
            Debug.LogError("Flashlight NOT FOUND!");
    }

    void Update()
    {
        if (flashlight == null || !flashlight.IsFlashlightOn())
        {
            ResetVisual();
            return;
        }

        if (IsInFlashlightCone())
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
            Flicker();
        }
        else
        {
            ResetVisual();
        }
    }

    bool IsInFlashlightCone()
    {
        Transform player = flashlight.transform.root;

        Vector2 directionToGhost = (transform.position - player.position);
        float distance = directionToGhost.magnitude;

        if (distance > detectionRadius)
            return false;

        directionToGhost.Normalize();
        Vector2 flashlightDirection = player.localScale.x > 0 ? Vector2.right : Vector2.left;

        float angle = Vector2.Angle(flashlightDirection, directionToGhost);

        return angle < coneAngle;
    }

    void TakeDamage(float amount)
    {
        currentHealth -= amount;

        // Play ghost damage sound
        if (GameAudioManager.instance != null)
            GameAudioManager.instance.PlayGhostDamage();

        if (currentHealth <= 0f)
            Die();
    }

    void Flicker()
    {
        if (spriteRenderer == null) return;

        float alpha = 1f - (Mathf.Sin(Time.time * flickerSpeed) * flickerAmount);
        alpha = Mathf.Clamp(alpha, 0.2f, 1f);

        Color c = originalColor;
        c.a = alpha;
        spriteRenderer.color = c;
    }

    void ResetVisual()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (poofEffect != null)
            Instantiate(poofEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}