using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;

    private bool hasEnded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasEnded) return;

        if (collision.CompareTag("Player"))
        {
            hasEnded = true;
            Time.timeScale = 0f; 
            endScreen.SetActive(true);
        }
    }
}
