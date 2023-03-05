using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusHealthBar : MonoBehaviour
{
    public Transform enemyTransform; // reference to the enemy object's transform

    void Update()
    {
        transform.position = enemyTransform.position; // keep the position of the empty game object the same as the enemy's position
        transform.rotation = Quaternion.identity; // set the rotation of the empty game object to be fixed and not rotate
    }
}


