using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedPowerup")]
public class SpeedPowerup : Powerup
{
    [SerializeField] private float speedMultiplier = 2f;

    public override void Apply(GameObject target) {
        PlayerMovement movementController = target.GetComponent<PlayerMovement>();
        movementController.velocity *= speedMultiplier;
    }
}
