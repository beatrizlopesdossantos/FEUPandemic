using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementState { IDLE, RUNNING }

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private float dirX, dirY;
    private bool facingLeft = false;

    [SerializeField] private float velocity = 10;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        dirX = 0f;
        dirY = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateVelocity();
        UpdateAnimation();
    }

    private void UpdateVelocity()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(dirX * velocity, dirY * velocity);
    }

    private void UpdateAnimation()
    {
        MovementState state;
        if (dirX != 0 || dirY != 0) state = MovementState.RUNNING;
        else state = MovementState.IDLE;

        facingLeft = dirX < 0 || (facingLeft && dirX == 0);
        sprite.flipX = facingLeft;

        anim.SetInteger("state", (int)state);
    }
}
