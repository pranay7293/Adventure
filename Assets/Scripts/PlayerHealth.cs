using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBarSlider;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            // Player is dead, perform necessary actions (e.g., game over, respawn, etc.)
            // You can add your own logic here
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
    }
}

