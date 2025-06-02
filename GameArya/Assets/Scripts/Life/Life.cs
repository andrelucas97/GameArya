using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public string itemID;
    [SerializeField] private AudioSource audioSourceHealth;
    [SerializeField] private AudioClip audioHealth;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (collision.GetComponent<PlayerController>().life <10)
            {
                collision.GetComponent<PlayerController>().life++;
                audioSourceHealth.PlayOneShot(audioHealth);

                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

                Destroy(gameObject, audioHealth.length);
            }
            
        }
    }
}
