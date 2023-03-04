using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private GameObject objectToFollow;
    private Animator anim;
    [SerializeField] public int damage;
    [SerializeField] private float velocity = 1f;
    [SerializeField] public int maxLife;
    // public HealthBar healthBar;    

    private void Start()
    {
        // healthBar.SetMaxHealth(100);
        objectToFollow = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, objectToFollow.transform.position, Time.deltaTime * velocity);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            anim.SetTrigger("attack");
        }
    }
}
