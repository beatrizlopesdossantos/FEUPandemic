using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    private bool canFire = true;
    private float timer = 0;
    public float timeBetweenShots = 0.5f;

    void Update()
    {
        if (!canFire) {
            timer += Time.deltaTime;
            if (timer > timeBetweenShots)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && canFire) {
            canFire = false;
            Instantiate(bullet, firePoint.transform.position, Quaternion.identity);
            timer = 0;
        }
    }
}
