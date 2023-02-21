using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    [SerializeField] RectTransform barStart;
    [SerializeField] RectTransform[] barEnd;

    [SerializeField] float maxBarSize;
    [SerializeField] bool isHealth = false;
    [SerializeField] bool isEnergy = false;
    [SerializeField] bool isActionBar = false;

    private float baseX;
    private float baseY;

    private PlayerInventory playerInventory;

    private void Start()
    {

        playerInventory = GameObject.Find("Player").GetComponent<PlayerManager>().Inventory;

        if (barEnd.Length > 0)
        {
            baseX = barEnd[0].localPosition.x;
            baseY = barEnd[0].localPosition.y;
        }


    }

    private void FixedUpdate()
    {

        if (isHealth)
        {
            TransformUtils.SetWidth(barStart, playerInventory.Health * (10 * maxBarSize / playerInventory.MaxHealth) / 10);
            foreach (RectTransform transform in barEnd)
            {
                transform.localPosition = new Vector3(baseX, baseY, 0) + new Vector3(playerInventory.Health * (10 * maxBarSize / playerInventory.MaxHealth) / 10, 0, 0);
            }
        } else if (isEnergy)
        {
            TransformUtils.SetWidth(barStart, playerInventory.Energy * (10 * maxBarSize / playerInventory.MaxEnergy) / 10);
            foreach (RectTransform transform in barEnd)
            {
                transform.localPosition = new Vector3(baseX-1, baseY, 0) + new Vector3(playerInventory.Energy * (10 * maxBarSize / playerInventory.MaxEnergy) / 10, 0, 0);
            }
        } else if (isActionBar)
        {
            barStart.localScale = new Vector3(playerInventory.ActionProgress * 0.5f / 10, 0.5f, 1);
        }

    }

}
