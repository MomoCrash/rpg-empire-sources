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

    IRON_ORE,
    IRON,
    IRON_BAR,
    IRON_BLOCK,

    COPPER_ORE,
    COPPER,
    COPPER_BAR,
    COPPER_BLOCK,

    // Mob Collectibles

    HONEY_COMB,
    APPLE,
    NETTLE,
    SAGE,
    WHEAT,
    COTTON,
    MUSHROOM,
    BEEF,
    FISH,
    PORKCHOP,

    COW_LEATHER,
    WOLF_FUR,


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
        { Material.DIRT, new Item { UniqueId = 10, Name = "Terre", TextureIndex = 5 } },
        { Material.WOOD, new Item { UniqueId = 11, Name = "Bois", TextureIndex = 0 } },
        { Material.ROCK, new Item { UniqueId = 12, Name = "Pierre", TextureIndex = 29 } },
        { Material.CHARCOAL, new Item { UniqueId = 14, Name = "Charbon", TextureIndex = 4 } },

        // Ore
        { Material.COPPER_ORE, new Item { UniqueId = 100, Name = "Cuivre", TextureIndex = 30 } },
        { Material.IRON_ORE, new Item { UniqueId = 101, Name = "Fer", TextureIndex = 36 } },

        // Refined
        { Material.STONE, new Item { UniqueId = 1000, Name = "Pierre taillé", TextureIndex = 11 } },
        { Material.COPPER, new Item { UniqueId = 1001, Name = "Cuivre traité", TextureIndex = 40 } },
        { Material.IRON, new Item { UniqueId = 1002, Name = "Fer traité", TextureIndex = 46 } },
        { Material.PLANK, new Item { UniqueId = 1003, Name = "Planches", TextureIndex = 1 } },

        // Perfect
        { Material.REFINED_STONE, new Item { UniqueId = 2000, Name = "Pierre raffiner", TextureIndex = 18 } },
        { Material.COPPER_BAR, new Item { UniqueId = 2001, Name = "Barre de Cuivre", TextureIndex = 50 } },
        { Material.IRON_BAR, new Item { UniqueId = 2003, Name = "Barre de Fer", TextureIndex = 56 } },

        // Forged
        { Material.COPPER_BLOCK, new Item { UniqueId = 2001, Name = "Copper Bar", TextureIndex = 15 } },
        { Material.IRON_BLOCK, new Item { UniqueId = 2002, Name = "Copper Bar", TextureIndex = 12 } },

        // Vegetable
        { Material.HONEY_COMB, new Item { UniqueId = 10000, Name = "Rayon de Miel", TextureIndex = 69 } },
        { Material.APPLE, new Item { UniqueId = 10001, Name = "Pomme", TextureIndex = 60  } },
        { Material.NETTLE, new Item { UniqueId = 10002, Name = "Ortie", TextureIndex = 88 } },
        { Material.SAGE, new Item { UniqueId = 10003, Name = "Sauge", TextureIndex = 89 } },
        { Material.MUSHROOM, new Item { UniqueId = 10004, Name = "Champignon", TextureIndex = 58 } },


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