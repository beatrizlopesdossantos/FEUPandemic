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
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private int maxLife = 100;
    public int currentLife;
    public HealthBar healthBar;
    private bool isCollidingWithEnemy = false;
    public bool isAlive = true;
    private int damage;
    private PlayerFollower playerFollower;

    private void Start()
    {
        healthBar.SetMaxHealth(maxLife);
        currentLife = maxLife;
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        healthBar.transform.rotation = Quaternion.Inverse(transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Virus") && isAlive) {
            playerFollower = collision.gameObject.GetComponent<PlayerFollower>();
            if (!playerFollower.enabled) return;

            isCollidingWithEnemy = true;
            damage = collision.gameObject.GetComponent<PlayerFollower>().damage;
            InvokeRepeating("HurtPlayer", 0f, 0.4f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Virus") && isAlive) {
            isCollidingWithEnemy = false;
            CancelInvokeHurt();
        }
    }

    private void KillPlayer() {
        rb.bodyType = RigidbodyType2D.Static;
        movement.enabled = false;
        isAlive = false;
        anim.SetTrigger("death");
        deathSound.Play();
        CancelInvokeHurt();
        if (playerFollower != null) playerFollower.CancelInvokeAttack();
    }

    private void HurtPlayer() {
        anim.SetTrigger("hurt");
        if (!isCollidingWithEnemy) return;
        currentLife -= damage;
        healthBar.SetHealth(currentLife);
        if (currentLife <= 0) {
            KillPlayer();
        } else {
            hurtSound.Play();
        }
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CancelInvokeHurt() {
        CancelInvoke("HurtPlayer");
    }

    public void Heal(int healAmount) {
        currentLife += healAmount;
        if (currentLife > maxLife) currentLife = maxLife;
        healthBar.SetHealth(currentLife);
    }
}
