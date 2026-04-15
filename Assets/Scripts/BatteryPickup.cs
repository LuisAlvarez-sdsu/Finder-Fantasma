using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float batteryTime = 10f;

 private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player"))
    {
        PlayerFlashlight playerFlash = other.GetComponent<PlayerFlashlight>();
        Player player = other.GetComponent<Player>();

        if (playerFlash != null)
            playerFlash.AddBattery(batteryTime);

        player?.CollectBattery();
        Destroy(gameObject);
    }
    }
}