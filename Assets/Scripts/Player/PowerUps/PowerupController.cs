using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    private Shooting shootingController;
    private PlayerMovement movementController;
    void Start() {
        shootingController = GetComponent<Shooting>();
        movementController = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        GameObject powerup = other.gameObject;
        if (powerup.CompareTag("SpeedPowerUp")) {
            movementController.velocity *= 2; // TODO: Make this a variable/script
        } else if (powerup.CompareTag("AttackSpeedPowerUp")) {
            shootingController.timeBetweenShots /= 2; // TODO: Make this a variable/script
        }
    }
}
