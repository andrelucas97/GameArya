using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public enum TutorialType { Walk, Jump, Attack }
    public TutorialType tutorialType;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            TutorialController tutorialController = FindObjectOfType<TutorialController>();
            if (tutorialController != null)
            {
                switch (tutorialType)
                {
                    case TutorialType.Walk:
                        tutorialController.ShowTutorial(TutorialController.TutorialType.Walk);
                        break;
                    case TutorialType.Jump:
                        tutorialController.ShowTutorial(TutorialController.TutorialType.Jump);
                        break;
                    case TutorialType.Attack:
                        tutorialController.ShowTutorial(TutorialController.TutorialType.Attack);
                        break;
                }
            }
        }
    }
}
