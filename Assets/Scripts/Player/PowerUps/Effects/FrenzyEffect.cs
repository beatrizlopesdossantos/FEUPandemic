using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Frenzy")]
public class FrenzyEffect : TemporaryEffect
{
    public override void Apply(GameObject target)
    {
        Shooting shootingController = target.GetComponent<Shooting>();
        shootingController.ActivateFrenzyFire();
    }

    public override void Remove(GameObject target)
    {
        Shooting shootingController = target.GetComponent<Shooting>();
        shootingController.DeactivateFrenzyFire();
    }
}
