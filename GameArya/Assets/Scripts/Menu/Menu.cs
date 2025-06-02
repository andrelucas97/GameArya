using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public string nameScene;
    public GameObject notSaved;
    public GameObject gameSaved;
    public GameObject settingsMenu;

    public LevelLoader levelLoader;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    public void PlayGame()
    {
        LoadGame.Instance.wasLoaded = false;
        DeleteKeys();
        levelLoader.Transition("Level1");
    }

    public void PlayAgain()
    {
        DeleteKeys();        
        levelLoader.Transition("Level1");
    }

    private void DeleteKeys()
    {
        string[] keysToDelete = { "LEVEL_SAVED", "KEY_LIFE", "KEY_SYEN", "KEY_POS_X", "KEY_POS_Y" };
        foreach (string key in keysToDelete)
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();

    }

    public void LoadScene(string nameScene)
    {
        Time.timeScale =1.0f;
        levelLoader.Transition(nameScene);
    }

    public void SaveGame()
    {

        var player = PlayerController.Instance;
        var syen = SyenCanvas.Instance;

        string activeScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LEVEL_SAVED", activeScene);
        PlayerPrefs.SetInt("KEY_LIFE", player.life);
        PlayerPrefs.SetInt("KEY_SYEN", syen.syenCount);

        PlayerPrefs.SetFloat("KEY_POS_X", player.transform.position.x);
        PlayerPrefs.SetFloat("KEY_POS_Y", player.transform.position.y);

        gameSaved.SetActive(true);
        StartCoroutine(HideMessage(gameSaved));
        
    }

    public void LoadGameMenu()
    {
        LoadGame.Instance.Loading();

        if (!PlayerPrefs.HasKey("LEVEL_SAVED")){
            notSaved.SetActive(true);

            StartCoroutine(HideMessage(notSaved));
        }
    }

    private IEnumerator HideMessage(GameObject messageDesactive)
    {
        yield return new WaitForSecondsRealtime(3f);
        messageDesactive.SetActive(false);
    }

    void TogglePause()
    {
        if (pauseMenu.activeSelf || settingsMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);

            Time.timeScale = 1f;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void SettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void BacktoPause()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
