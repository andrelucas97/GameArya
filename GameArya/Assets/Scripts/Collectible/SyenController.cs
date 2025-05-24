using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SyenController : MonoBehaviour
{

    [SerializeField] private AudioSource audioSourceCollect;
    [SerializeField] private AudioClip collectAudio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSourceCollect.PlayOneShot(collectAudio);
            SyenCanvas.instance.AddSyen(1);

            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;


            Destroy(gameObject, collectAudio.length);
        }
    }
}
