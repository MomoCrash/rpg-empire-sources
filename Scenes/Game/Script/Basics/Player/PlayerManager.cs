using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public string pseudo = "MomoCrash";

    public PlayerInventory Inventory = new();
    public GameObject CurrentBuilding;
    public int CurrentRessource;

    private TextValue fpsText;

    [SerializeField] FastTabManager FastTab;

    public int EnergyGainAmount;
    public float EnergyGainInterval;

    public float pickupRange = 1f;
    private readonly List<GameObject> followingItems = new();

    // Start is called before the first frame update
    void Start()
    {

        fpsText = GameObject.Find("FPS").GetComponent<TextValue>();
        GameObject.Find("Pseudo").GetComponent<TextValue>().textBase = pseudo;

        StartCoroutine(EnergyGain());

    }

    private void Update()
    {

        fpsText.value = ((int)(1.0f / Time.smoothDeltaTime));

    }

    void FixedUpdate()
    {

        PickupItemInRange(gameObject.transform.position, pickupRange);
        foreach (var followingItem in followingItems)
        {

            followingItem.transform.localPosition = Vector2.Lerp(followingItem.transform.position, transform.position, 4f * Time.fixedDeltaTime);

            if (followingItem.GetComponent<Collider2D>().Distance(gameObject.GetComponent<Collider2D>()).distance < 0.2f)
            {

                var itemId = int.Parse(followingItem.name);
                Inventory.AddItem(new ItemStack(Item.GetItem(itemId), 1));
                GameObject.Destroy(followingItem.gameObject);
                ItemManager.inTransition.Remove(followingItem);
                followingItems.Remove(followingItem);
                FastTab.SendFastTabMessage(new FastTab(new ItemStack(Item.GetItem(itemId), 1), 3, DateTime.Now));
                break;

            }
        }
    }

    void PickupItemInRange(Vector3 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.gameObject.CompareTag("Player"))
            {
                continue;
            }

            if (hitCollider.gameObject.CompareTag("Item"))
            {

                followingItems.Add(hitCollider.gameObject);
                hitCollider.gameObject.tag = "Pickuped";

            }
        }
    }

    IEnumerator EnergyGain()
    {

        while (true)
        {
            print("Energy");
            if (Inventory.HasMaxEnergy())
            {
                yield return new WaitForSeconds(EnergyGainInterval);
            } else
            {

                print("Energy Gain");
                Inventory.Energy += EnergyGainAmount;

                yield return new WaitForSeconds(EnergyGainInterval);

            }
        }
    }

    public void SetValues(PlayerData playerData)
    {
        this.pseudo = playerData.Pseudo;
        this.EnergyGainAmount = playerData.EnergyGainAmount;
        this.EnergyGainInterval = playerData.EnergyGainInterval;
        this.pickupRange = playerData.PickupRange;
        this.Inventory = JsonUtility.FromJson<PlayerInventory>(playerData.InventoryJson);
        gameObject.transform.position = new Vector3(playerData.Position[0], playerData.Position[1], playerData.Position[2]);
    }

}

[System.Serializable]
public class PlayerData
{

    public string Pseudo;
    public int EnergyGainAmount;
    public float EnergyGainInterval;
    public float PickupRange;
    public string InventoryJson;
    public float[] Position = new float[3];

    public PlayerData(PlayerManager player)
    {

        this.Pseudo = player.pseudo;
        this.EnergyGainAmount = player.EnergyGainAmount;
        this.EnergyGainInterval = player.EnergyGainInterval;
        this.PickupRange = player.pickupRange;
        this.InventoryJson = JsonUtility.ToJson(player.Inventory);
        this.Position[0] = player.gameObject.transform.position.x;
        this.Position[1] = player.gameObject.transform.position.y;
        this.Position[2] = player.gameObject.transform.position.z;

    }

}