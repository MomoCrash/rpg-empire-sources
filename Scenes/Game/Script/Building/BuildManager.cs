using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BuildManager : MonoBehaviour
{

    public Material[] BuildMaterialsCost;
    public int[] BuildAmountCost;

    [SerializeField] int WorkDuration;
    [SerializeField] int GainAfter;
    public Material[] MaterialsGain;
    public int[] QuantitiesGain;

    public int WorkEnergyCost;
    public Material[] WorkMaterialsCost;
    public int[] QuantitiesMaterialsCost;

    [SerializeField] Light2D PrimaryLight;
    [SerializeField] Light2D SecondaryLight;

    private DateTime _lastActivate;
    private DateTime _lastGain;

    private bool _isActivated = false;
    private bool _canActivate = false;

    void Start()
    {

        _lastActivate = DateTime.Now;
        _lastGain = DateTime.Now;

    }

    void FixedUpdate()
    {

        DateTime now = DateTime.Now;

        TimeSpan timeActive = (now - _lastActivate);
        TimeSpan timeGain = (now - _lastGain);

        if (!_isActivated && _canActivate && Input.GetMouseButton(1))
        {
            PlayerManager playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
            if (WorkEnergyCost > 0)
            {

                if (!playerManager.Inventory.UseEnergy(WorkEnergyCost))
                {
                    return;
                }
            }

            if (WorkMaterialsCost.Length > 0)
            {
                ItemStack[] stacks = new ItemStack[WorkMaterialsCost.Length];
                foreach ( var index in Enumerable.Range(0, WorkMaterialsCost.Length) )
                {

                    stacks[index] = new ItemStack(Item.GetItem(WorkMaterialsCost[index]), QuantitiesMaterialsCost[index]);

                    if (!playerManager.Inventory.HasEnoughItem(stacks))
                    {
                        return;
                    }
                }

                foreach ( var itemStack in stacks )
                {

                    playerManager.Inventory.RemoveItem(itemStack);

                }

            }

            PrimaryLight.enabled = true;
            SecondaryLight.enabled = true;
            _isActivated = true;
            _lastActivate = now;
            _lastGain = now;
            return;
        }

        if (!_isActivated)
        {
            return;
        }

        if (timeActive.TotalMilliseconds >= WorkDuration*1000)
        {
            _isActivated = false;
            PrimaryLight.enabled = false;
            SecondaryLight.enabled = false;
            return;
        }

        if (timeGain.TotalMilliseconds >= GainAfter*1000)
        {
            int index = UnityEngine.Random.Range(0, MaterialsGain.Length);
            var itemStack = new ItemStack(Item.GetItem(MaterialsGain[index]), UnityEngine.Random.Range(1, QuantitiesGain[index]));
            ItemManager.DropItemStackInRange(gameObject.transform.position, itemStack, -1, 1, -2, 1);
            _lastGain = now;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _canActivate = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _canActivate = false;
    }

}
