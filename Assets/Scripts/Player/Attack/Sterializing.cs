using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sterializing : MonoBehaviour
{
    [SerializeField] private GameObject gas;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Sprite gunSprite;
    [SerializeField] private Image abilityImage;

    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float fuelConsumption = 20f;
    [SerializeField] private float fuelRefill = 10f;

    private float fuel;

    private void Start() {
        fuel = maxFuel;
        gas = Instantiate(gas, firePoint.transform.position, gas.transform.rotation);
        gas.SetActive(false);
        abilityImage.fillAmount = 1;
    }

    private void Update() {
        if (Input.GetMouseButton(1) && fuel > 0) {
            UseGas();
        } else {
            RefillGas();
        }

        abilityImage.fillAmount = 1 - fuel / maxFuel;
    }

    private void UseGas() {
        SetSprite();
        gas.SetActive(true);
        fuel -= Time.deltaTime * fuelConsumption;
    }

    private void RefillGas() {
        gas.SetActive(false);
        float increasedFueld = fuel + Time.deltaTime * fuelRefill;
        fuel = Mathf.Min(increasedFueld, maxFuel);
    }

    private void SetSprite()
    {
        firePoint.GetComponent<SpriteRenderer>().sprite = gunSprite;
    }
}
