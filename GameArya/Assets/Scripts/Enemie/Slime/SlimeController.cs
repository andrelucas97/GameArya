using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour, IDamageable
{
    // VAR PUBLICAS
    public float jumpForce;
    public float jumpInterval;
    public float life;
    public float maxLife;
    public int damage;

    public float CurrentLife => life;
    public float MaxLife => maxLife;

    // VAR PRIVADAS
    private float jumpTimer;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpTimer = jumpInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            life = 0;
            Destroy(gameObject);
        }

        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0)
        {
            Jump();
            jumpTimer = jumpInterval;
        }
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    public void TakeDamage(int damage)
    {
        life -= damage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);

        }
    }
}
