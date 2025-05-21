using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SyenCanvas : MonoBehaviour
{
    public static SyenCanvas instance;

    public int syenCount = 0;
    public TextMeshProUGUI syenText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        syenText.text = syenCount.ToString();
    }

    public void AddSyen(int amount)
    {
        syenCount += amount;
        UpdateUI();
    }
}
