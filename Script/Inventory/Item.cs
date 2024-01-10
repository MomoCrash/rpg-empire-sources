using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum Material
{

    NONE,
    BLINKS,
    WOOD,
    PLANK,
    DIRT,
    ROCK,
    STONE,
    REFINED_STONE,
    CHARCOAL,
    COPPER_ORE,
    COPPER,
    COPPER_BAR,

    // Vegetable


    HONEY_COMB,
    APPLE,
    NETTLE,
    SAGE,

}

[System.Serializable]
public class Item
{

    public static Dictionary<Material, Item> materials = new()
    {
        // Random things
        { Material.NONE, new Item { UniqueId = -1, Name = "Drop that :(", TextureIndex = -1 } },
        { Material.BLINKS, new Item { UniqueId = 0, Name = "Blinks", TextureIndex = 37 } },

        // Basics ressources
        { Material.DIRT, new Item { UniqueId = 10, Name = "Dirt", TextureIndex = 5 } },
        { Material.WOOD, new Item { UniqueId = 11, Name = "Wood", TextureIndex = 0 } },
        { Material.ROCK, new Item { UniqueId = 12, Name = "Rock", TextureIndex = 29 } },
        { Material.CHARCOAL, new Item { UniqueId = 14, Name = "Charcoal", TextureIndex = 4 } },

        // Ore
        { Material.COPPER_ORE, new Item { UniqueId = 100, Name = "Copper", TextureIndex = 30 } },

        // Refined
        { Material.STONE, new Item { UniqueId = 1000, Name = "Stone", TextureIndex = 11 } },
        { Material.COPPER, new Item { UniqueId = 1001, Name = "Copper", TextureIndex = 40 } },
        { Material.PLANK, new Item { UniqueId = 1002, Name = "Plank", TextureIndex = 1 } },

        // Perfect
        { Material.REFINED_STONE, new Item { UniqueId = 2000, Name = "Refined Stone", TextureIndex = 18 } },
        { Material.COPPER_BAR, new Item { UniqueId = 2001, Name = "Copper Bar", TextureIndex = 50 } },

        // Vegetable
        { Material.HONEY_COMB, new Item { UniqueId = 10000, Name = "Honney Comb", TextureIndex = 69 } },
        { Material.APPLE, new Item { UniqueId = 10001, Name = "Apple", TextureIndex = 60  } },
        { Material.NETTLE, new Item { UniqueId = 10002, Name = "Nettle", TextureIndex = 88 } },
        { Material.SAGE, new Item { UniqueId = 10003, Name = "Sage", TextureIndex = 89 } },


    };

    public int UniqueId;
    public string Name;
    public int TextureIndex;

    public static Item GetItem(Material material)
    {
        return materials.GetValueOrDefault(material);
    }

    public static Item GetItem(int uniqueId)
    {
        foreach ( Item item in materials.Values)
        {

            if (item.UniqueId == uniqueId) {
                return item;
            }

        }
        return GetItem(Material.NONE);
    }

    public override bool Equals(object obj)
    {
        if (obj is not Item)
        {
            return false;
        }

        return ((Item)obj).UniqueId == this.UniqueId;
    }

}

[System.Serializable]
public class LootTable
{

    public Material[] Materials;
    public int[] Quantities;

    public LootTable(Material[] materials, int[] quantities) => (Materials, Quantities) = (materials, quantities);

    public ItemStack[] GetDropItemStack(int numbers)
    {
        List<ItemStack> items = new List<ItemStack>();
        for (int i = 0;i < numbers; i++)
        {
            var item = Item.GetItem(Materials[Random.Range(0, Materials.Length)]);
            var amount = Random.Range(1, Quantities[Random.Range(0, Quantities.Length)]);
            items.Add(new ItemStack(item, amount));
        }
        return items.ToArray();
    }

}

[System.Serializable]
public record ItemStack
{

    public Item Item;
    public int Amount;

    public ItemStack(Item item, int amount) => (Item, Amount) = (item, amount);

    public bool Remove(int amount)
    {
        if (this.Amount < amount)
        {
            return false;
        }
        this.Amount -= amount;
        return true;
    }

}