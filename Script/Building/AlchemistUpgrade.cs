using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlchemistUpgrade : MonoBehaviour
{
    public PlayerManager Player;

    public TextMeshProUGUI EnergyLevel;
    public TextMeshProUGUI HealthLevel;
    public TextMeshProUGUI StrenghtLevel;

    public TextMeshProUGUI BlinksField;
    public TextMeshProUGUI UpgradeCost;

    private int EnergyAdd; 
    private int HealthAdd; 
    private int StrenghtAdd;

    public void ToggleMenu()
    {

        gameObject.GetComponent<Animator>().SetTrigger("toggle");
        UpdateMenu();

    }

    public void CloseMenu()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Close");
    }

    public void UpdateMenu()
    {

        var inventory = Player.Inventory;

        BlinksField.text = "" + inventory.GetItemStack(Item.GetItem(0)).Amount;

        EnergyLevel.text = "" + inventory.Energy + EnergyAdd;
        HealthLevel.text = "" + inventory.HealthLevel + HealthAdd;
        StrenghtLevel.text = "" + Player.Damage + StrenghtAdd;

        UpgradeCost.text = "" + EnergyAdd + HealthAdd + StrenghtAdd;

    }

    public void ValidateUpgrade()
    {
        var inventory = Player.Inventory;
        var blinks = new ItemStack(Item.GetItem(0), UpgradePrice());
        if (inventory.HasEnoughItem(blinks))
        {
            inventory.RemoveItem(blinks);
            inventory.MaxEnergy += EnergyAdd;
            inventory.MaxHealth += HealthAdd;
            Player.Damage += StrenghtAdd;
        }

    } 

    public int UpgradePrice()
    {
        return EnergyAdd + HealthAdd + StrenghtAdd;
    }

    public void AddEnergy(int amount)
    {

        var inventory = Player.Inventory;

        if (inventory.MaxEnergy + amount < inventory.MaxEnergy)
        {
            EnergyAdd += amount;
        }
        UpdateMenu();

    } 

}
