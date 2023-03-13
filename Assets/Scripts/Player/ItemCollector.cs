using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Text itemText;
    [SerializeField] private Text scoreText;
    [SerializeField] private AudioSource collectSound;

    private int items = 0;
    private int score = 0;

    private void Start() {
        itemText.text = $"x{items}";
        scoreText.text = $"{score}";
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Collectible")) {
            collectSound.Play();
            Destroy(collision.gameObject);
            items++;
            itemText.text = $"x{items}";
        }
    }

    public void IncreaseScore(int points) {
        score += points;
        scoreText.text = $"{score}";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Items", items);
    }
}
