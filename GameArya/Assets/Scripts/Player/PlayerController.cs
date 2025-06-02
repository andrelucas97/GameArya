using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    // VAR PRIVADAS
    private Rigidbody2D rb;
    private float moveX;
    private int jumpCount;
    private bool facingRight;

    private Animator animator;
    private CapsuleCollider2D colliderPlayer;

    private float lastEnergyUse = -Mathf.Infinity;    

    // VAR PUBLICAS
    [Header("Atributes Player")]
    public int life;
    public float energy = 10;
    public float energPerShot = 2;
    private float speed = 4;
    private bool isDead = false;


    [SerializeField] private float jumpForce = 10;
    private int maxJump = 2;
    private float groundCheckRadius = 0.2f;

    [Header("Atributes Arrow")]
    public float timeLastShot;
    public float velocityArrow;
    [SerializeField] private float cooldownDuration = 60f;

    [Header("Atributes Regen")] // Publicas para melhorias no arco futuramente.
    public float maxEnergy;
    public float regenRate;
    public float regenDelay;

    [Header("Atributes Collections")]
    public int syenCollection = 0;
        
    [Header("Bool")]
    private bool isGrounded;
    private bool firePower = true;

    [Header("Audios")]
    [SerializeField] private AudioSource audioSourceArrow;
    [SerializeField] private AudioSource audioSourceDamage;
    [SerializeField] private AudioClip shotArrowSound;
    [SerializeField] private AudioClip damageSound;

    [SerializeField] private AudioSource audioSourceWalk;
    [SerializeField] private AudioClip walkSound;

    [Header("Images")]
    [SerializeField] private Image fireCDImage;

    [Header("Others")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform pointShot;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowFirePrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject screenDead;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("WAS_LOADED") == 1)
        {
            life = PlayerPrefs.GetInt("KEY_LIFE", 0);
            syenCollection = PlayerPrefs.GetInt("KEY_SYEN", 0);
            Debug.Log("Game loaded!"); // Adicionar imagem de mensagem
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliderPlayer = GetComponent<CapsuleCollider2D>();

        // declarando se o Player está virado para DIR ou ESQ
        facingRight = transform.eulerAngles.y == 0f;

        if (PlayerPrefs.HasKey("WAS_LOADED") && PlayerPrefs.GetInt("WAS_LOADED") == 1)
        {
            float x = PlayerPrefs.GetFloat("KEY_POS_X", transform.position.x);
            float y = PlayerPrefs.GetFloat("KEY_POS_Y", transform.position.y);
            transform.position = new Vector3(x, y, transform.position.z);
        }
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

    public void SaveGame() // Salvando somente pelo Menu!
    {
        string activeScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LEVEL_SAVED", activeScene);
        PlayerPrefs.SetInt("KEY_LIFE", life);
        PlayerPrefs.SetInt("KEY_SYEN", SyenCanvas.Instance.syenCount);

        PlayerPrefs.SetFloat("KEY_POS_X", transform.position.x);
        PlayerPrefs.SetFloat("KEY_POS_Y", transform.position.y);

        Debug.Log("Game saved!"); // adicionar mensagem na tela
    }

    // MORTE QUEDA
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone"))
        {
            Die();
        }
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
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (UseEnergy(energPerShot))
            {
                animator.Play(("Attack"), -1);
                timeLastShot = 0.7f;
            }                    
        } else if (Input.GetButtonDown("Fire2"))
        {
            if (!firePower) return;

            animator.Play(("Attack2"), -1);
        }

        timeLastShot -= Time.deltaTime;
             
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

        ShootArrow(arrowPrefab);
        energy -= energPerShot;

    }
    public void FireArrowPower()
    {

        ShootArrow(arrowFirePrefab);
        StartCoroutine(FireArrowCD());

        // Colocar cooldown
    }

    private IEnumerator FireArrowCD()
    {
        firePower = false;
        fireCDImage.fillAmount = 1f;

        float elapsed = 0f;

        while (elapsed < cooldownDuration)
        {
            elapsed += Time.deltaTime;
            fireCDImage.fillAmount = 1f - (elapsed / cooldownDuration);
            yield return null;
        }

        fireCDImage.fillAmount = 0f;
        firePower = true;
    }

    private void ShootArrow(GameObject arrowShoot)
    {
        GameObject arrow = Instantiate(arrowShoot, pointShot.position, transform.rotation);

        if (facingRight)
        {
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityArrow, 0);
        }
        else
        {
            arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-velocityArrow, 0);

        }
        audioSourceArrow.PlayOneShot(shotArrowSound);
    }
    #endregion

    // DANO PLAYER
    #region Damage Player
    public void TakeDamage(int damage)
    {
        life -= damage;
        //animator.SetTrigger("Hurt");

        animator.Play("Hurt", -1);
        audioSourceDamage.PlayOneShot(damageSound);

        if (life <= 0)
        {
            Die();
        }
    }
    #endregion

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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
    
    public void PlayFootstep()
    {
        if (!isGrounded) return;

        audioSourceWalk.PlayOneShot(walkSound);
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
        if (isDead) return;

        isDead = true;
        life = Mathf.Max(life, 0);
        animator.Play("Dead");
        screenDead.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead && collision.gameObject.CompareTag("Ground"))
        {
            GetComponent<PlayerController>().enabled = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }
}
