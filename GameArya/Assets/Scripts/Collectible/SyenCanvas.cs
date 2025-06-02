using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SyenCanvas : MonoBehaviour
{
    public static SyenCanvas Instance;

    public int syenCount = 0;
    public TextMeshProUGUI syenText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        syenCount = PlayerPrefs.GetInt("KEY_SYEN", 0);
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
