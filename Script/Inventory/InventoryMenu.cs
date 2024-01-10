using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{

    [SerializeField] PlayerManager PlayerManager;
    [SerializeField] GameObject SlotModel;

    public Dictionary<int, GameObject> slots = new();

    public bool IsOpen;
    private PlayerInventory _inventory;


    private void Start()
    {

        _inventory = PlayerManager.Inventory;

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && !IsOpen)
        {

            UpdateInventory();
            gameObject.GetComponent<Animator>().SetBool("open", true);
            IsOpen = true;

        } else if (Input.GetKeyDown(KeyCode.E) && IsOpen)
        {

            gameObject.GetComponent<Animator>().SetBool("open", false);
            IsOpen = false;

        }
    }

    /**
     * <summary>
     * Update all inventory slot / images / amount / content
     * </summary>
     */
    private void UpdateInventory()
    {

        var itemNumber = _inventory.Content.Count;
        var nextX = 83;
        var nextY = 305;

        foreach (ItemStack item in _inventory.Content)
        {



            if (slots.ContainsKey(item.Item.UniqueId))
            {

                UpdateSlot(slots.GetValueOrDefault(item.Item.UniqueId), item, nextX, nextY);

            } else
            {
                //print(item.Item.Name);
                var itemSlot = GameObject.Instantiate(SlotModel, gameObject.transform);
                slots.Add(item.Item.UniqueId, itemSlot);
                UpdateSlot(itemSlot, item, nextX, nextY);
            }

            nextX += 149;
            if (nextX > 600)
            {
                nextX = 83;
                nextY -= 156;
            }
            

        }
    }

    /**
     * <summary>
     * Update slot in inventory
     * </summary>
     */
    private void UpdateSlot(GameObject slot, ItemStack stack, float x, float y)
    {

        slot.transform.localPosition = new Vector3(x, y, 0);

        slot.transform.Find("Case - Name").GetComponent<Text>().text = stack.Item.Name + " x " + stack.Amount;
        slot.transform.Find("Case - Icon").GetComponent<Image>().sprite = ItemManager.GetMaterialSprite(stack.Item.TextureIndex);

    }

}
