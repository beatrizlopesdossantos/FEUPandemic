using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupEffect effect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (effect is TemporaryEffect) {
                StartCoroutine(StartEffect(other.gameObject));
            } else {
                effect.Apply(other.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator StartEffect(GameObject target)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        TemporaryEffect effect = this.effect as TemporaryEffect;
        effect.Apply(target);
        yield return new WaitForSeconds(effect.duration);
        effect.Remove(target);

        Destroy(gameObject);
    }
}
