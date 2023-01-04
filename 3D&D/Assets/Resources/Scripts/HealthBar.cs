using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Image healthBarImage;
    public MinionCharacter character;

    void Start()
    {
        healthBar.maxValue = character.maxHealth;
        healthBar.value = character.currentHealth;
        healthBarImage.color = Color.green;
    }

    public void UpdateHealthBar()
    {
        healthBar.value = character.currentHealth;
        var healthPercentage = character.currentHealth / (float)character.maxHealth;
        if (healthPercentage >= 0.7)
            healthBarImage.color = Color.green;
        else if (healthPercentage < 0.7 && healthPercentage > 0.3)
            healthBarImage.color = Color.yellow;
        else
            healthBarImage.color = Color.red;
    }
}
