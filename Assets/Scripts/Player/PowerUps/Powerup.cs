using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerupEffect effect;
    [SerializeField] private AudioSource audioSource;
    private Image powerUpFill = null;
    private Image currentImage = null;
    private float timer = 0;
    private bool start = false;
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

        TemporaryEffect effect = this.effect as TemporaryEffect;
        start = true;
        int i = 0;
        GameObject[] powerUpContainers = GameObject.FindGameObjectsWithTag("Container");
        foreach (GameObject powerUpContainer in powerUpContainers) {
            currentImage = powerUpContainer.GetComponent<Image>();
            if (currentImage.sprite == null) {
                Color imageColor = currentImage.color;
                imageColor.a = 1f;
                currentImage.color = imageColor;
                currentImage.sprite = (this.effect as TemporaryEffect).powerUpImage;
                break;
            }
            currentImage = null;
            i++;
        }
        GameObject[] powerUpTimers = GameObject.FindGameObjectsWithTag("PowerUpTimer");
        if (i < 3) powerUpFill = powerUpTimers[i].GetComponent<Image>();
        float maxFill = 0;

        for (int j = 0; j < powerUpTimers.Length; j++) {
            if (currentImage == null && i == 3) {
                if (powerUpTimers[j].GetComponent<Image>().fillAmount > maxFill) {
                    maxFill = powerUpTimers[j].GetComponent<Image>().fillAmount;
                    powerUpFill = powerUpTimers[j].GetComponent<Image>();
                    powerUpFill.fillAmount = 1;
                    currentImage = powerUpContainers[j].GetComponent<Image>();
                }
            }
        }
        if (i == 3) {
            Color imageColor = currentImage.color;
            imageColor.a = 1f;
            currentImage.color = imageColor;
            currentImage.sprite = (this.effect as TemporaryEffect).powerUpImage;
        }

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
                if (currentImage == null) {
                    return;
                }
                Color imageColor = currentImage.color;
                imageColor.a = 0f;
                currentImage.color = imageColor;
                currentImage.sprite = null;
                timer = effect.duration;
            }

            if (powerUpFill != null) powerUpFill.fillAmount = 1 - timer / effect.duration;
        }
    }
}
