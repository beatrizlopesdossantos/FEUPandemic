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
    private bool isCollidingWithPlayer = false;

    private void Start()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, objectToFollow.transform.position, Time.deltaTime * velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            isCollidingWithPlayer = true;
            InvokeRepeating("attackPlayer", 0.4f, 0.4f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            isCollidingWithPlayer = false;
            CancelInvoke("attackPlayer");
        }
    }

    private void attackPlayer() {
        if (isCollidingWithPlayer) anim.SetTrigger("attack");
    }
}
