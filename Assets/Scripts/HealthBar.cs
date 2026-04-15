using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthFill;
    public Image heartIcon;
    public Player player; // optional, can be null at start

    private int lastHealth;

    void Start()
    {
        // Try to find the player if not assigned
        if (player == null)
            player = Object.FindFirstObjectByType<Player>();

        if (player != null)
            lastHealth = player.health;
        else
            lastHealth = 100; // default if player not found
    }

    void Update()
    {
        // If player is missing, try to find again (respawned)
        if (player == null)
        {
            player = Object.FindFirstObjectByType<Player>();
            if (player != null)
                lastHealth = player.health;
            else
                return; // nothing to update
        }

        // Update health bar fill
        float t = player.health / 100f;
        if (healthFill != null)
            healthFill.fillAmount = t;

        // Detect damage and flash heart icon
        if (player.health < lastHealth)
        {
            Flash();
        }

        lastHealth = player.health;
    }

    void Flash()
    {
        if (heartIcon == null) return;
        heartIcon.color = Color.red;
        Invoke(nameof(ResetColor), 0.15f);
    }

    void ResetColor()
    {
        if (heartIcon == null) return;
        heartIcon.color = Color.white;
    }
}