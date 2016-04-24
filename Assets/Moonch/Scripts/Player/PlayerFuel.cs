using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFuel : MonoBehaviour
{
    public Image damageImage;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public Slider fuelSlider;
    public Image sliderImage;
    public int initialFuel = 100;
    public int currentFuel;
    public float flashSpeed = 0.05f;

    void Awake()
    {
        currentFuel = initialFuel;
        StartFuelConsuption();
    }

    void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        ConsumeFuel(amount);
    }

    void StartFuelConsuption()
    {
        InvokeRepeating("ConsumeFuel", fuelConsumptionTime, fuelConsumptionTime);
    }

    void ConsumeFuel()
    {
        ConsumeFuel(fuelConsumption);
    }

    public void ConsumeFuel(int amount)
    {
        currentFuel -= amount;
        fuelSlider.value = currentFuel;

        UpdateFuelIndicatorColor();

        if (currentFuel <= 0 && !isDead)
        {
            Die();
        }
    }

    private void UpdateFuelIndicatorColor()
    {
        if (currentFuel > 50)
        {
            sliderImage.color = new Color(86f / 255.0f, 181f / 255.0f, 100f / 255.0f, 255f / 255.0f);
        }
        else if (currentFuel <= 50 && currentFuel > 30)
        {
            sliderImage.color = new Color(1f, 1f, 0f, 1f);
        }
        else
        {
            sliderImage.color = new Color(1f, 0f, 0f, 0.8f);
        }
    }

    public void RefillFuel(int amount)
    {
        currentFuel += amount;
        fuelSlider.value = currentFuel;

        UpdateFuelIndicatorColor();
    }

    void Die()
    {
        isDead = true;
    }

    private bool isDead;
    private bool damaged;
    private int fuelConsumption = 1;
    private float fuelConsumptionTime = 1f;
}
