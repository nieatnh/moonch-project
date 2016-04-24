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
        currentFuel -= amount;
        fuelSlider.value = currentFuel;

        if (currentFuel <= 50 && currentFuel > 0)
        {
            // Yellow
            sliderImage.color = new Color(1f, 1f, 0f, 1f);
        }

        if (currentFuel <= 30 && currentFuel > 0)
        {
            // Red
            sliderImage.color = new Color(1f, 0f, 0f, 0.8f);
        }

        if (currentFuel <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
    }
    
    private bool isDead;
    private bool damaged;
}
