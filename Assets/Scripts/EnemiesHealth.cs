using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth = 100;
    public Slider slider;
    public GameObject healthBar;

    void Start()
    {
        Debug.Log("Virus has no health script");
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = CalculateHealth();
        healthBar.SetActive(true);
    }

    void Update()
    {
    }

    float CalculateHealth()
    {
        Debug.Log(currentHealth / maxHealth);
        return currentHealth / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Virus has no health script");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        slider.value = CalculateHealth();
    }
}
