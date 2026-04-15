using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    public PlayerFlashlight player;
    public Image batteryFill;

    void Update()
    {
        if (player != null)
        {
            batteryFill.fillAmount = player.GetBatteryPercent();
        }
    }
}