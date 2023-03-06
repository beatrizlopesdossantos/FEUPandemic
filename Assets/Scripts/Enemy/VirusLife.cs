using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusLife : MonoBehaviour
{
    [SerializeField] private ItemInfo[] items;
    [SerializeField] private int points = 100;
    private ItemCollector collector;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private AudioSource deathSound;
    public int currentHealth;
    public Slider slider;
    public GameObject healthBar;

    private Animator anim;
    private PlayerFollower playerFollower;
    private bool isAlive = true;

    private void Start() {
        collector = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemCollector>();
        anim = GetComponent<Animator>();
        playerFollower = GetComponent<PlayerFollower>();
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        healthBar.SetActive(true);
    }

    float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            handleDeath();
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        slider.value = currentHealth;
    }

    public void handleDeath() {
        deathSound.Play();
        isAlive = false;
        playerFollower.CancelInvokeAttack();
        playerFollower.enabled = false;
        anim.SetTrigger("death");
    }

    public void DestroyVirus() {
        Destroy(gameObject);
        float rand = Random.Range(0f, 1f);
        float total = 0f;
        for (int i = 0; i < items.Length; ++i) {
            total += items[i].chance;
            if (rand <= total) {
                Instantiate(items[i].item, transform.position, items[i].item.rotation);
                break;
            }
        }

        collector.IncreaseScore(points);
    }
}

[System.Serializable]
public class ItemInfo
{
    public Transform item;
    public float chance;
}
