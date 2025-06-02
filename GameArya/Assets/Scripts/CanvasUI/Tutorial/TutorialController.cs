using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;

    private bool tutorialActive = false;
    private TutorialType currentTutorial;

    public enum TutorialType { Walk, Jump, Attack }

    void Start()
    {
        if (PlayerPrefs.HasKey("WAS_LOADED")) return;

        ShowTutorial(TutorialType.Walk);
    }
    void Update()
    {
        if (!tutorialActive) return;

        switch (currentTutorial)
        {
            case TutorialType.Walk:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    HideTutorial();
                break;
            case TutorialType.Jump:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
                    HideTutorial();
                break;
            case TutorialType.Attack:
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
                    HideTutorial();
                break;
        }
    }

    private void HideTutorial()
    {
        tutorialPanel.SetActive(false);
        tutorialActive = false;
    }

    public void ShowTutorial(TutorialType tutorial)
    {
        Debug.Log("Tutorial iniciado!");
        currentTutorial = tutorial;
        tutorialActive = true;

        switch (tutorial)
        {
            case TutorialType.Walk:
                tutorialText.text = "Press A or D\nfor <u>WALK</u> !";
                break;
            case TutorialType.Jump:
                tutorialText.text = "Press W or Space\nfor <u>DOUBLE</u> <u>JUMP</u> !";
                break;
            case TutorialType.Attack:
                tutorialText.text = "Press Z or X\nfor <u>Attack</u> !";
                break;
        }

        tutorialPanel.SetActive(true);
    }

    
}
