using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    public LevelLoader levelLoader;


    [SerializeField] private string levelToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();

        if (collision.CompareTag("Player"))
        {

            PlayerPrefs.SetInt("WAS_LOADED", 1);
            PlayerPrefs.SetInt("KEY_LIFE", player.life);
            PlayerPrefs.SetInt("KEY_SYEN", SyenCanvas.Instance.syenCount);
            PlayerPrefs.SetString("LEVEL_SAVED", levelToLoad);

            levelLoader.Transition(levelToLoad);
            //SceneManager.LoadScene(levelToLoad);
        }
    }
}
