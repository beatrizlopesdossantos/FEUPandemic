using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusLife : MonoBehaviour
{
    [SerializeField] private ItemInfo[] items;

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
    }
}

[System.Serializable]
public class ItemInfo
{
    public Transform item;
    public float chance;
}
