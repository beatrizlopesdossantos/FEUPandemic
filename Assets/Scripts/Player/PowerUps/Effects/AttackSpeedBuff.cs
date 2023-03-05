using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/AttackSpeedBuff")]
public class AttackSpeedBuff : TemporaryEffect
{
    [SerializeField] private float speedMultiplier;

    public override void Apply(GameObject target)
    {
        Shooting shootingController = target.GetComponent<Shooting>();
        shootingController.timeBetweenShots /= speedMultiplier;
    }

    public override void Remove(GameObject target)
    {
        Shooting shootingController = target.GetComponent<Shooting>();
        shootingController.timeBetweenShots *= speedMultiplier;
    }
}
