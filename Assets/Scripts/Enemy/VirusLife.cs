using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusLife : MonoBehaviour
{
    [SerializeField] private ItemInfo[] items;
    [SerializeField] public List<GameObject> enemies;
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
        AudioSource.PlayClipAtPoint(deathSound.clip, transform.position);
        isAlive = false;
        playerFollower.CancelInvokeAttack();
        playerFollower.enabled = false;
        anim.SetTrigger("death");
        GetComponent<PolygonCollider2D>().enabled = false;
    }

    public void DestroyVirus() {
        Destroy(gameObject);
        if (enemies.Count >= 1)
        {
            GameObject newEnemy = Instantiate(enemies[0], transform.position, enemies[0].transform.rotation);
            newEnemy.GetComponent<VirusLife>().enemies = new List<GameObject>(enemies);
            newEnemy.GetComponent<VirusLife>().enemies.RemoveAt(0);
        }
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
