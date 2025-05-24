using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryController : MonoBehaviour, IDamageable
{
    // VAR PUBLICAS
    private BoxCollider2D colliderMemory;
    private Rigidbody2D rb;
    private Animator animator;
    private float sideSign;
    private string side;

    [Header("Atributes CheckGround")]
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    // VAR PRIVADAS
    //public GameObject range;

    [Header("Atributes")]

    public float life;
    public float maxLife;
    public float speed;
    public int damage;
    public Transform player;

    public float CurrentLife => life;
    public float MaxLife => maxLife;

    [Header("Aduios")]
    public AudioSource audioSourceDamage;
    public AudioSource audioSourceDeath;
    public AudioClip audioDamage;
    public AudioClip audioDeath;

    void Start()
    {
        colliderMemory = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (life <= 0)
        {
            life = 0;
            audioSourceDeath.PlayOneShot(audioDeath);
            animator.Play("Die", -1);
        }
    }


    void FixedUpdate()
    {
        movimentacaoMemory();

    }
    void movimentacaoMemory()
    {
        sideSign = Mathf.Sign(transform.position.x - player.position.x);

        if (Mathf.Abs(sideSign) == 1.0f)
        {
            side = sideSign == 1.0f ? "right" : "left";
        }

        switch (sideSign)
        {
            case -1: // right
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;

            case 1: //left
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                break;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < 10f && distanceToPlayer > 1.5f && IsGroundAhead())
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), speed * Time.deltaTime);
            rb.MovePosition(targetPosition);

        } else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private bool IsGroundAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
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
            animator.Play("Die", -1);

        }
    }
    public void TakeDamage(int damage)
    {
        life -= damage;

        if(life > 0)
        {
            audioSourceDamage.PlayOneShot(audioDamage);
        }
    }

    public void DestroyAfterDeath()
    {
        Destroy(gameObject);
    }
}
