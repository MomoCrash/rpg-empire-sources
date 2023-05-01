using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerInventory
{

    public List<ItemStack> Content;

    public float ActionProgress = 0;

    public int MaxHealth;
    public int Health;
    public int MaxEnergy;
    public int Energy;

    public PlayerInventory()
    {

        Content = new();

        MaxHealth = 100;
        Health = 100;
        MaxEnergy = 200;
        Energy = 200;

    }

    public void RemoveItem(ItemStack item)
    {

        var index = Content.FindIndex(it => it.Item.Equals(item.Item));
        if (index > -1)
        {
            Content[index].Amount -= item.Amount;
            if (Content[index].Amount <= 0)
            {
                Content.RemoveAt(index);
            }
        }


    }

    public void AddItem(ItemStack item)
    {

        var index = Content.FindIndex(it => it.Item.Equals(item.Item));
        if (index > -1)
        {
            Content[index].Amount += item.Amount;
        } else
        {
            Content.Add(item);
        }

        
    }

    public bool HasEnoughItem(ItemStack item)
    {
        var index = Content.FindIndex(it => it.Item.Equals(item.Item));
        if (index > -1)
        {
            if (Content[index].Amount >= item.Amount)
            {
                return true;
            }
        }
        return false;
    }

    public string[] HasEnoughItem(ItemStack[] items)
    {
        foreach ( var item in items )
        {
            if (!HasEnoughItem(item))
            {
                return new string[1] { item.Item.Name };
            }
        }
        return new string[0];
    }

    public bool HasMaxEnergy()
    {
        return Energy >= MaxEnergy;
    }

    public bool HasEnoughEnergy(int actionCost) 
    {
        return Energy >= actionCost;
    }

    public bool Damage(int damage)
    {
        if (Health > damage)
        {
            Health -= damage;
            return true;
        }
        return false;
    }

    public bool UseEnergy(int amount)
    {
        if (HasEnoughEnergy(amount)) 
        {
            Energy -= amount;
            return true;
        }
        return false;
    }

}
