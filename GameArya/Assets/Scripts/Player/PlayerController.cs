using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController : MonoBehaviour
{
    // VAR PRIVADAS
    private Rigidbody2D rb;
    private float moveX;
    private int jumpCount;
    private bool facingRight;

    private Animator animator;
    private CapsuleCollider2D colliderPlayer;

    private float lastEnergyUse = -Mathf.Infinity;
    private AudioSource audioSource;

    // VAR PUBLICAS
    [Header("Atributes")]
    public int life = 10;
    public float energy = 10;
    public float energPerSHot = 2;
    public float speed = 4;

    public float jumpForce = 10;
    public int maxJump = 2;
    public float groundCheckRadius = 0.2f;

    [Header("Atributes Arrow")]
    public float timeLastShot;
    public float velocityArrow;
    public bool isFire;

    [Header("Atributes Regen")] // Publicas para melhorias no arco futuramente.
    public float maxEnergy;
    public float regenRate;
    public float regenDelay;

    [Header("Atributes Collections")]
    public int syenCollection = 0;
        
    [Header("Bool")]
    [SerializeField]
    public bool isGrounded;

    [Header("Audios")]
    [SerializeField]
    public AudioClip shotArrowSound;

    [Header("Others")]
    [SerializeField]
    public Transform groundCheck;
    public Transform pointShot;
    public GameObject arrowPrefab;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliderPlayer = GetComponent<CapsuleCollider2D>();

        // declarando se o Player está virado para DIR ou ESQ
        facingRight = transform.eulerAngles.y == 0f;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimentacao e Pulo
        Move();
        Jump();

        // Attack
        Attack();

        // Regeneracao Energia
        RegenerateEnergy();
    }

    // REGENARACAO ENERGY
    void RegenerateEnergy()
    {
        if ((Time.time - lastEnergyUse) >= regenDelay && energy < maxEnergy)
        {
            energy += regenRate * Time.deltaTime;
            energy = Mathf.Min(energy, maxEnergy);
        }
    }

    // ATAQUE PLAYER
    #region Attack Player
    void Attack()
    {
        
        if (Input.GetButtonDown("Fire2"))
        {
            if (UseEnergy(energPerSHot))
            {
                animator.Play(("Attack"), -1);
                isFire = true;
                timeLastShot = 0.7f;
            }                    
        }

        timeLastShot -= Time.deltaTime;
        if (timeLastShot <= 0)
        {
            isFire = false;
        }        
    }

    bool UseEnergy(float energ)
    {
        if (energy >= energ)
        {
            lastEnergyUse = Time.time;
            return true;
        }

        return false;
    }

    public void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, pointShot.position, transform.rotation);

        if (facingRight)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityArrow, 0);
        }
        else
        {
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-velocityArrow, 0);

        }
        audioSource.PlayOneShot(shotArrowSound);

        energy -= energPerSHot;

    }
    #endregion

    // DANO PLAYER
    #region Damage Player
    public void TakeDamage(int damage)
    {
        life -= damage;
        animator.SetTrigger("Hurt");

        if (life <= 0)
        {
            Die();
        }
    }
    #endregion

    // MOVIMENTACAO PLAYER
    #region Move Player
    void Jump()
    {
        if (isGrounded)
        {
            jumpCount = 1;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
            // animator.SetBool("isJump", true);
        }
    } 

    void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        if (moveX != 0)
        {

            facingRight = moveX > 0;
            float direction = moveX > 0 ? 0f : 180f;
            transform.eulerAngles = new Vector3(0f, direction, 0f);
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }
    #endregion

    // MORTE PLAYER
    private void Die()
    {
        animator.Play("Dead");
    }
}
