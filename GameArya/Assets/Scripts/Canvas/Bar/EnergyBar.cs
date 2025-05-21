using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image barEnergy;
    [SerializeField] private PlayerController playerController;
    private float maxEnergy;

    void Start()
    {
        maxEnergy = playerController.energy;
    }

    void Update()
    {
        barEnergy.fillAmount = playerController.energy / maxEnergy;
    }
}
