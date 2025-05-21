using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArrow : MonoBehaviour
{
    // VAR PUBLICAS
    public int damage;
    public float arrowTime;
    public float distance;
    public LayerMask layerEnemie;

    void Start()
    {
        Destroy(gameObject, arrowTime);
    }

    void Update()
    {
        RaycastHit2D hitInf = Physics2D.Raycast(transform.position, transform.forward, distance, layerEnemie);

        if (hitInf.collider != null && hitInf.collider.CompareTag("Enemie"))
        {
            IDamageable damageable = hitInf.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Debug.Log("Destruída por invisibilidade");
        Destroy(gameObject);
    }
}
