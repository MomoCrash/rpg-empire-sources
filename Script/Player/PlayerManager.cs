using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{

    [Header("Player Parameters")]
    public string pseudo = "Michelus";
    public float AttackRange = 5.0f;
    public float Damage = 10.0f;
    public float AttackCooldown = 0.1f;
    public float AttackEnergyCost = 4.0f;
    public float CriticalMultiplier = 1.2f;

    public float EnergyGainAmount;
    public float EnergyGainInterval;

    [Header("Utils")]

    public PlayerInventory Inventory = new();
    public ObjectDetector ObjectDetector ;
    public ProgressBar HealthBar ;
    public ProgressBar StaminaBar;
    public ProgressBar ActionBar;
    public GameObject CurrentBuilding;
    public int CurrentRessource;

    public FastTabManager FastTab;

    private TextValue fpsText;

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

        HealthBar.SetWidth(Inventory.MaxHealth, Inventory.Health);
        StaminaBar.SetWidth(Inventory.MaxEnergy, Inventory.Energy);
        if (Inventory.ActionProgress > 0)
        {
            ActionBar.SetScale(10, Inventory.ActionProgress);
        }

        var animator = gameObject.GetComponent<Animator>();
        if (Input.GetMouseButtonDown(0))
        {

            if (ObjectDetector.InRangeMobs.Count > 0)
            {

                if (Inventory.UseEnergy(AttackEnergyCost))
                {
                    ObjectDetector.InRangeMobs[0].GetComponent<MobIA>().Damage(Damage * UnityEngine.Random.Range(1, CriticalMultiplier));
                    animator.SetBool("used", true);
                }

            }

        } else
        {
            animator.SetBool("used", false);
        }

    }

    IEnumerator EnergyGain()
    {

        while (true)
        {

            if (Inventory.HasMaxEnergy())
            {
                yield return new WaitForSeconds(EnergyGainInterval);
            } else
            {

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
        this.Inventory = JsonUtility.FromJson<PlayerInventory>(playerData.InventoryJson);
        gameObject.transform.position = new Vector3(playerData.Position[0], playerData.Position[1], playerData.Position[2]);
    }

}

[System.Serializable]
public class PlayerData
{

    public string Pseudo;
    public float EnergyGainAmount;
    public float EnergyGainInterval;
    public string InventoryJson;
    public float[] Position = new float[3];

    public PlayerData(PlayerManager player)
    {

        this.Pseudo = player.pseudo;
        this.EnergyGainAmount = player.EnergyGainAmount;
        this.EnergyGainInterval = player.EnergyGainInterval;
        this.InventoryJson = JsonUtility.ToJson(player.Inventory);
        this.Position[0] = player.gameObject.transform.position.x;
        this.Position[1] = player.gameObject.transform.position.y;
        this.Position[2] = player.gameObject.transform.position.z;

    }

}