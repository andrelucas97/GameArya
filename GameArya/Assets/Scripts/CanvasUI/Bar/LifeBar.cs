using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{   
    public Image barLife;
    [SerializeField] private PlayerController playerController;
    private float maxLife = 10;

    void Start()
    {
        
    }

    void Update()
    {
        barLife.fillAmount = playerController.life / maxLife;
    }
}
