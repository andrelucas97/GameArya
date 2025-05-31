using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public static LoadGame Instance;
    public bool wasLoaded;
    [SerializeField] private GameObject notSave;
   
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }    
    public void Loading()
    {
        notSave = GameObject.Find("NotSaved");


        if (PlayerPrefs.HasKey("LEVEL_SAVED"))
        {
            wasLoaded = true;
            PlayerPrefs.SetInt("WAS_LOADED", 1);

            string levelToload = PlayerPrefs.GetString("LEVEL_SAVED");
            SceneManager.LoadScene(levelToload);
        }
        else
        {
            // chamar função dentro do Menu
             // Adicionar imagem de mensagem
        }
    }
}
