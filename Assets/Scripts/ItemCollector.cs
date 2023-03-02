using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Text itemText;
    [SerializeField] private AudioSource collectSound;

    private int items = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Collectible")) {
            collectSound.Play();
            Destroy(collision.gameObject);
            items++;
            itemText.text = $"x{items}";
        }
    }
}
