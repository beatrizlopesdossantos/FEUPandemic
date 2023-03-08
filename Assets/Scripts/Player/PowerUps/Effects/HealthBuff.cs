using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    [SerializeField] private int healthBuff;

    public override void Apply(GameObject target)
    {
        PlayerLife playerLife = target.GetComponent<PlayerLife>();
        playerLife.Heal(healthBuff);
    }
}
