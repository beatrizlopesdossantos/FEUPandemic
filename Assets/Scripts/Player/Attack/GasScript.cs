using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasScript : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float cooldownTime = 0.1f;

    private List<GameObject> damagedViruses = new List<GameObject>();
    private float lastDamageTime = 0f;

    private void OnParticleCollision(GameObject collision)
    {
        if (collision.gameObject.CompareTag("Virus") && !damagedViruses.Contains(collision.gameObject))
        {
            if (collision.gameObject.GetComponent<VirusLife>() != null)
            {
                collision.gameObject.GetComponent<VirusLife>().TakeDamage(damage);
                damagedViruses.Add(collision.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= cooldownTime)
        {
            damagedViruses.Clear();
            lastDamageTime = Time.time;
        }
    }
}
