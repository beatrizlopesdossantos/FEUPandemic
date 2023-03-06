using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Sprite gunSprite;
    [SerializeField] private Image abilityImage;

    private bool canFire = true;
    private int frenzyFireCount = 0; // counts the number of frenzy effects applied
    private float timer = 0;
    public float timeBetweenShots = 0.5f;
    public float timeBetweenShotsFrenzy = 0.1f;

    private void Start() {
        abilityImage.fillAmount = 0;
    }

    private void Update()
    {
        if (frenzyFireCount == 0) {
            NormalFire();
        } else {
            AutomaticFire();
        }
    }

    public void ActivateFrenzyFire() {
        this.frenzyFireCount++;
        this.timer = timeBetweenShotsFrenzy;
    }

    public void DeactivateFrenzyFire() {
        this.frenzyFireCount--;
        if (this.frenzyFireCount == 0) {
            this.timer = timeBetweenShots;
        }
    }

    private void NormalFire() {
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenShots)
            {
                canFire = true;
                timer = 0;
            }

            abilityImage.fillAmount = 1 - timer / timeBetweenShots;
            return;
        }

        abilityImage.fillAmount = 0;

        if (Input.GetMouseButtonDown(0))
        {
            SetSprite();
            canFire = false;
            Instantiate(bullet, firePoint.transform.position, bullet.transform.rotation);
            timer = 0;
        }
    }

    private void AutomaticFire() {
        if (Input.GetMouseButton(1))
        {
            DeactivateFrenzyFire();
            this.frenzyFireCount = 0; // remove all buffs
            return;
        }

        abilityImage.fillAmount = 1;
        SetSprite();
        if (timer <= timeBetweenShotsFrenzy) {
            timer += Time.deltaTime;
        } else {
            timer = 0;
            Instantiate(bullet, firePoint.transform.position, bullet.transform.rotation);
        }
    }

    private void SetSprite() {
        firePoint.GetComponent<SpriteRenderer>().sprite = gunSprite;
    }
}
