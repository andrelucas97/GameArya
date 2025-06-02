using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SyenController : MonoBehaviour
{

    [SerializeField] private AudioSource audioSourceCollect;
    [SerializeField] private AudioClip collectAudio;
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
        if (collision.CompareTag("Player"))
        {
            //Salvando ITEM
            PlayerPrefs.SetInt("Collected_" + itemID, 1);
            PlayerPrefs.Save();

            audioSourceCollect.PlayOneShot(collectAudio);
            SyenCanvas.Instance.AddSyen(1);

            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;


            Destroy(gameObject, collectAudio.length);
        }
    }
}
