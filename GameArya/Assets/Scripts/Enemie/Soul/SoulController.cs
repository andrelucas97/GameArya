using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoulController : MonoBehaviour, IDamageable
{
    // VAR PUBLICAS
    public Transform a;
    public Transform b;
    public float speed;
    public int damage;
    public int life;

    public AudioClip soulDeathSound;

    // VAR PRIVADAS
    private CapsuleCollider2D colliderSoul;
    private Animator animator;
    private bool goRight;
    private AudioSource audioSource;


    void Start()
    {
        colliderSoul = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {       

        if (goRight)
        {
            MovimentacaoSoul(false, b, 180f);
        }
        else
        {
            MovimentacaoSoul(true, a, 0f);

        }

        if (life <= 0)
        {
            life = 0;

            float direction = goRight ? 0f : 180f;
            transform.eulerAngles = new Vector3(0f, direction, 0f);
            
            this.enabled = false;
            colliderSoul.enabled = false;
            audioSource.PlayOneShot(soulDeathSound);
            animator.Play("Dead", -1);
        }
    }

    public void DestroyAfterDeath()
    {
        Destroy(gameObject);
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

    void MovimentacaoSoul(bool b, Transform x, float pos)
    {
        if (Vector2.Distance(transform.position, x.position) < 0.1f)
        {
            goRight = b;
        }

        transform.eulerAngles = new Vector3(0f, pos, 0f);
        transform.position = Vector2.MoveTowards(transform.position, x.position, speed * Time.deltaTime);

    }

    public void TakeDamage(int damage)
    {
        life -= damage;
    }
}
