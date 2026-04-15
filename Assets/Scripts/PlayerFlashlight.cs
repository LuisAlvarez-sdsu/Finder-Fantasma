using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    public static PlayerFlashlight Instance;

    public float maxBattery = 10f;
    private float currentBattery;

    public GameObject flashlightVisual;
    private bool isOn = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentBattery = maxBattery;

        if (flashlightVisual != null)
            flashlightVisual.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentBattery > 0)
        {
            ToggleFlashlight();
        }

        if (isOn)
        {
            currentBattery -= Time.deltaTime;

            if (currentBattery <= 0)
            {
                currentBattery = 0;
                TurnOffFlashlight();
            }
        }
    }

    void ToggleFlashlight()
    {
        isOn = !isOn;

        if (flashlightVisual != null)
            flashlightVisual.SetActive(isOn);
    }

    void TurnOffFlashlight()
    {
        isOn = false;

        if (flashlightVisual != null)
            flashlightVisual.SetActive(false);
    }

    public bool IsFlashlightOn()
    {
        return isOn;
    }

    public float GetBatteryPercent()
    {
        return Mathf.Clamp01(currentBattery / maxBattery);
    }

    public void AddBattery(float amount) // ✅ FIX ADDED
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);
    }
}