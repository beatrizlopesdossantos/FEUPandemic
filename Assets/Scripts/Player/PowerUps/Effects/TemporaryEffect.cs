using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TemporaryEffect : PowerupEffect
{
    public float duration = 5f;
    [SerializeField] public Sprite powerUpImage;

    public abstract void Remove(GameObject target);
}
