using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private GameObject objectToFollow;
    [SerializeField] private float velocity = 1f;

    private void Start()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, objectToFollow.transform.position, Time.deltaTime * velocity);
    }
}
