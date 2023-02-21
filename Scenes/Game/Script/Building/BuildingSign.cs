using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSign : MonoBehaviour
{

    [SerializeField] Animator BuildInfoMenuAnimator;

    public string TitleText;
    public Text TitleObject;

    [TextArea] public string ContentText;
    public Text ContentObject;

    public Sprite Sprite;
    public Image Icon;

    public Text GainField;
    public Text WorkCostField;

    public BuildManager BuildManager;
    public PlayerManager PlayerManager;

    public string FormatedGain;
    public string FormatedWorkCost;

    void Start()
    {

        FormatedGain = "";
        if (BuildManager.MaterialsGain.Length > 0)
        {
            foreach (int index in Enumerable.Range(0, BuildManager.MaterialsGain.Length))
            {
                print(Item.GetItem(BuildManager.MaterialsGain[index]).Name);
                FormatedGain += Item.GetItem(BuildManager.MaterialsGain[index]).Name + " x " + BuildManager.QuantitiesGain[index];
            }
        } else
        {
            FormatedGain += "Rien cheh.";
        }

        FormatedWorkCost = "";
        if (BuildManager.WorkEnergyCost > 0)
        {
            FormatedWorkCost += "Energie " + BuildManager.WorkEnergyCost + "\n";
        }
        if ( BuildManager.WorkMaterialsCost.Length > 0 )
        {
            foreach (int index in Enumerable.Range(0, BuildManager.WorkMaterialsCost.Length))
            {
                FormatedWorkCost += Item.GetItem(BuildManager.WorkMaterialsCost[index]).Name + " x " + BuildManager.QuantitiesMaterialsCost[index] + "\n";
            }
        } else
        {
            FormatedWorkCost += "\nPas de materiaux";
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        BuildInfoMenuAnimator.SetBool("open", true);
        PlayerManager.CurrentBuilding = gameObject.transform.parent.gameObject;

        TitleObject.text = TitleText;
        ContentObject.text = ContentText;
        GainField.text = FormatedGain;
        WorkCostField.text = FormatedWorkCost;
        Icon.sprite = Sprite;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        BuildInfoMenuAnimator.SetBool("open", false);
        PlayerManager.CurrentBuilding = null;

    }

}
