using System.Collections.Generic;

[System.Serializable]
public class PlayerInventory
{

    public List<ItemStack> Content;

    public float ActionProgress = 0;

    public float MaxHealth;
    public float Health;
    public float HealthLevel;
    public float MaxEnergy;
    public float Energy;
    public float EnergyLevel;

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

    public ItemStack GetItemStack(Item item)
    {
        var index = Content.FindIndex(it => it.Item.Equals(item));
        if (index > -1)
        {
            return Content[index];
        }
        return new ItemStack(item, 0);
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
            if (item is null)
            {
                continue;
            }
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

    public bool HasEnoughEnergy(float actionCost) 
    {
        return Energy >= actionCost;
    }

    public bool Damage(float damage)
    {
        if (Health > damage)
        {
            Health -= damage;
            return true;
        }
        return false;
    }

    public bool UseEnergy(float amount)
    {
        if (HasEnoughEnergy(amount)) 
        {
            Energy -= amount;
            return true;
        }
        return false;
    }

}
