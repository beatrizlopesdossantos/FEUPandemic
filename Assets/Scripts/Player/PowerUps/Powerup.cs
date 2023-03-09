using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupEffect effect;
    [SerializeField] private AudioSource audioSource;
    private Image powerUpImage;
    private float timer = 0;
    private bool start = false;

    private void Start() {
        powerUpImage = GameObject.FindGameObjectWithTag("PowerUpTimer").GetComponent<Image>();
        powerUpImage.fillAmount = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null) {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            }

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
        powerUpImage.fillAmount = 1;

        TemporaryEffect effect = this.effect as TemporaryEffect;
        start = true;
        effect.Apply(target);
        yield return new WaitForSeconds(effect.duration);
        effect.Remove(target);

        Destroy(gameObject);
    }

    private void Update()
    {
        if (this.effect is TemporaryEffect && this.start) {
            timer += Time.deltaTime;
            TemporaryEffect effect = this.effect as TemporaryEffect;
            if (timer > effect.duration)
            {
                timer = effect.duration;
            }
            powerUpImage.fillAmount = 1 - timer / effect.duration;
        }
    }
}
