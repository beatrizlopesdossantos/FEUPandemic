using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class SpeedBuff : TemporaryEffect
{
    [SerializeField] private float speedMultiplier = 2f;

    public override void Apply(GameObject target) {
        PlayerMovement movementController = target.GetComponent<PlayerMovement>();
        movementController.velocity *= speedMultiplier;
    }

    public override void Remove(GameObject target) {
        PlayerMovement movementController = target.GetComponent<PlayerMovement>();
        movementController.velocity /= speedMultiplier;
    }
}
