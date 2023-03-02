using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusLife : MonoBehaviour
{
    [SerializeField] private ItemInfo[] items;
    [SerializeField] private int points = 100;
    private ItemCollector collector;

    private void Start() {
        collector = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemCollector>();
    }

    public void handleHit() {
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
