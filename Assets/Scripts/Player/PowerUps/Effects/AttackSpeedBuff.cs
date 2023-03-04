using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/AttackSpeedBuff")]
public class AttackSpeedBuff : TemporaryEffect
{
    [SerializeField] private float speedMultiplier;

    public override void Apply(GameObject target)
    {
        Shooting movementController = target.GetComponent<Shooting>();
        movementController.timeBetweenShots /= speedMultiplier;
    }

    public override void Remove(GameObject target)
    {
        Shooting movementController = target.GetComponent<Shooting>();
        movementController.timeBetweenShots *= speedMultiplier;
    }
}
