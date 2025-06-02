using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceHealth;
    [SerializeField] private AudioClip audioHealth;

    [Header("ID:")]
    [Header("ID:")]
    public string itemID;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Collected_" + itemID, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<PlayerController>().life <10)
            {
                //Salvando ITEM
                PlayerPrefs.SetInt("Collected_" + itemID, 1);
                PlayerPrefs.Save();

                collision.GetComponent<PlayerController>().life++;
                audioSourceHealth.PlayOneShot(audioHealth);

                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

                Destroy(gameObject, audioHealth.length);
            }
            
        }
    }
}
