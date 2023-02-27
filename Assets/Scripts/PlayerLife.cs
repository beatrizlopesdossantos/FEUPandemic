using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement movement;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource deathSound;

    private bool isAlive = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Virus") && isAlive) {
            KillPlayer();
        }
    }

    private void KillPlayer() {
        rb.bodyType = RigidbodyType2D.Static;
        movement.enabled = false;
        isAlive = false;
        anim.SetTrigger("death");
        deathSound.Play();
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
