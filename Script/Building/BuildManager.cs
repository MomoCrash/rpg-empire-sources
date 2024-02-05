using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BuildManager : MonoBehaviour
{

    public int BuildingID;

    public Material[] BuildMaterialsCost;
    public int[] BuildAmountCost;

    public int WorkDuration;
    public int GainAfter;
    public Material[] MaterialsGain;
    public int[] QuantitiesGain;

    public int WorkEnergyCost;
    public Material[] WorkMaterialsCost;
    public int[] QuantitiesMaterialsCost;

    public Light2D[] Lights;

    [TextArea(3, 5)]
    public string BuildTitle;
    [TextArea(3, 5)]
    public string BuildDescription;

    public Sprite BuildIcon;

    private bool _isActivated = false;

    IEnumerator BuildingWork()
    {

        int iteration = WorkDuration/GainAfter;

        foreach (var itNumber in Enumerable.Range(0, iteration))
        {
            int index = UnityEngine.Random.Range(0, MaterialsGain.Length);
            var itemStack = new ItemStack(Item.GetItem(MaterialsGain[index]), UnityEngine.Random.Range(1, QuantitiesGain[index] + 1));
            ItemManager.DropItemStackInRange(gameObject.transform.position, itemStack, -1, 1, -2, 1);
            yield return new WaitForSeconds(GainAfter);
        }

        _isActivated = false;
        foreach (var item in Lights)
        {
            item.enabled = false;
        }
        _isActivated = false;
        yield return null;

    }

    public void EnableBuilding()
    {
        if (!_isActivated)
        {
            _isActivated = HasEnoughtRessource();
            StartCoroutine(BuildingWork());
        }
    }

    public bool HasEnoughtRessource()
    {
        PlayerManager playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

        ItemStack[] stacks = new ItemStack[WorkMaterialsCost.Length];
        if (WorkMaterialsCost.Length > 0)
        {
            foreach (var index in Enumerable.Range(0, WorkMaterialsCost.Length))
            {
                stacks[index] = new ItemStack(Item.GetItem(WorkMaterialsCost[index]), QuantitiesMaterialsCost[index]);
            }
        }

        string[] missingItem = playerManager.Inventory.HasEnoughItem(stacks);
        if (missingItem.Length > 0)
        {
            playerManager.FastTab.SendFastTabMessageDirect("Manque de " + missingItem[0], 2);
            return false;
        }

        if (WorkEnergyCost > 0)
        {
            if (!playerManager.Inventory.UseEnergy(WorkEnergyCost))
            {
                playerManager.FastTab.SendFastTabMessageDirect("Pas assez d'energie", 2);
                return false;
            }
        }

        foreach (var itemStack in stacks)
        {
            playerManager.Inventory.RemoveItem(itemStack);
        }

        foreach (var item in Lights)
        {
            item.enabled = true;
        }
        return true;
    }

}
