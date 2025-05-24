using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class AttackArrow : MonoBehaviour
{
    // VAR PUBLICAS
    [Header("Atributes")]
    public int damage;
    public float arrowTime;
    public float distance;

    public float timeAudio;

    [Header("Others")]
    public LayerMask layerEnemie;

    //VAR PRIVADAS
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, arrowTime);
    }

    void Update()
    {
        
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemie"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        else
        {
            AudioSource audio = collision.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.time = timeAudio;
                audio.Play();
            }

            Debug.Log("Acertou um objeto!");
            Destroy(gameObject);
        }
    }
}
