using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoulController : MonoBehaviour, IDamageable
{
    // VAR PUBLICAS
    [SerializeField] private Transform a;
    [SerializeField] private Transform b;
    [SerializeField] private float speed;
    public int damage;

    [Header("Atributes Health")]
    public float life;
    public float maxLife;
    public float CurrentLife => life;
    public float MaxLife => maxLife;

    [Header("Audios")]
    [SerializeField] private AudioSource audioSourceDeath;
    [SerializeField] private AudioSource audioSourceDamage;
    [SerializeField] private AudioClip soulDeathSound;
    [SerializeField] private AudioClip soulDamageSound;

    // VAR PRIVADAS
    private CapsuleCollider2D colliderSoul;
    private Animator animator;

    private bool goRight;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private SoulController soulController;

    void Start()
    {
        colliderSoul = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        //life = maxLife;
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
            audioSourceDeath.PlayOneShot(soulDeathSound);
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
        if (life > 0)
        {
            audioSourceDamage.PlayOneShot(soulDamageSound);
        }

    }
}
