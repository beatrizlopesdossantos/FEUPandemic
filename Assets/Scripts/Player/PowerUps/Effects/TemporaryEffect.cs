using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TemporaryEffect : PowerupEffect
{
    public float duration = 5f;

    public abstract void Remove(GameObject target);
}
