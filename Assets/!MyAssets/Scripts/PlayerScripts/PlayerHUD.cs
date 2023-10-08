using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text potionsText;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider oilSlider;

    PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponentInParent<PlayerInventory>();
    }

    private void Start()
    {
        healthSlider.maxValue = inventory.MaxHealth;
        oilSlider.maxValue = inventory.MaxLampOil;
    }

    private void Update()
    {
        goldText.text = "Gold: " + inventory.CurGold;
        potionsText.text = "Potions: " + inventory.CurPotions;

        healthSlider.value = inventory.CurHealth;
        oilSlider.value = inventory.CurLampOil;
    }
}
