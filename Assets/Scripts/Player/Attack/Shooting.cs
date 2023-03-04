using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    private bool canFire = true;
    private int frenzyFireCount = 0; // counts the number of frenzy effects applied
    private float timer = 0;
    public float timeBetweenShots = 0.5f;
    public float timeBetweenShotsFrenzy = 0.1f;

    void Update()
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
        this.timer = 0;
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
        }

        if (Input.GetMouseButtonDown(0) && canFire)
        {
            canFire = false;
            Instantiate(bullet, firePoint.transform.position, bullet.transform.rotation);
            timer = 0;
        }
    }

    private void AutomaticFire() {
        if (timer <= timeBetweenShotsFrenzy) {
            timer += Time.deltaTime;
        } else {
            timer = 0;
            Instantiate(bullet, firePoint.transform.position, bullet.transform.rotation);
        }
    }
}
