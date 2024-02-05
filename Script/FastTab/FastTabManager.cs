using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Collections;

public class FastTabManager : MonoBehaviour
{

    [SerializeField] GameObject FastTabModel;
    [SerializeField] GameObject FastTabModelLarge;

    private readonly List<FastTab> FastTabs = new();

    private bool _hasNewTab = false;

    [SerializeField] int baseX, baseY;

    void FixedUpdate()
    {

        var i = 0;
        foreach (var fastTabObject in FastTabs)
        {
            TimeSpan timeActive = DateTime.Now - fastTabObject.Start;
            if (timeActive.TotalMilliseconds > fastTabObject.Duration*1000)
            {
                fastTabObject.FastTabObject.GetComponent<Animator>().SetBool("out", true);

                GameObject.Destroy(fastTabObject.FastTabObject, 2f);
                FastTabs.RemoveAt(i);
                break;
            }
            i++;
        }

        if (_hasNewTab)
        {
            Dictionary<int, FastTab> fastTabDict = new();
            foreach (int index in Enumerable.Range(0, FastTabs.Count))
            {
                var tab = FastTabs[index];
                if ( tab.ItemStack == null) { continue; }
                if ( fastTabDict.ContainsKey(tab.ItemStack.Item.UniqueId) )
                {
                    FastTab fastTabDictValue = fastTabDict.GetValueOrDefault(tab.ItemStack.Item.UniqueId);
                    fastTabDictValue.ItemStack.Amount += tab.ItemStack.Amount;
                    fastTabDictValue.FastTabObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text 
                        = fastTabDictValue.ItemStack.Item.Name + " x" + fastTabDictValue.ItemStack.Amount.ToString();
                    fastTabDictValue.Start = DateTime.Now;

                    GameObject.Destroy(tab.FastTabObject);
                    FastTabs.RemoveAt(index);

                } else
                {

                    fastTabDict.Add(tab.ItemStack.Item.UniqueId, tab);

                }
            }

            i = 0;
            foreach (var fastTabObject in FastTabs)
            {
                fastTabObject.FastTabObject.transform.localPosition = new Vector3(baseX, baseY + (i * 100), 0);
                i++;
            }
            _hasNewTab = false;
        }

    }

    public void SendFastTabMessage(FastTab tab)
    {

        var fastTabObject = GameObject.Instantiate(FastTabModel, gameObject.transform);
        fastTabObject.SetActive(true);
        fastTabObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tab.ItemStack.Item.Name + " x" + tab.ItemStack.Amount.ToString();
        fastTabObject.transform.GetChild(1).GetComponent<Image>().sprite
            = ItemManager.GetMaterialSprite(tab.ItemStack.Item.TextureIndex);
        fastTabObject.transform.localPosition = new Vector3(baseX, baseY, 0);
        tab.FastTabObject = fastTabObject;
        FastTabs.Add(tab);
        if (FastTabs.Count > 0)
        {
            _hasNewTab = true;
        }

    }

    public void SendFastTabMessageDirect(String message, int duration)
    {

        var fastTabObject = GameObject.Instantiate(FastTabModelLarge, gameObject.transform);
        fastTabObject.SetActive(true);
        fastTabObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        fastTabObject.transform.localPosition = new Vector3(baseX, baseY, 0);
        FastTab messageTab = new(duration, DateTime.Now) { FastTabObject = fastTabObject };
        FastTabs.Add(messageTab);
        if (FastTabs.Count > 0)
        {
            _hasNewTab = true;
        }

    }

}

public class FastTab
{

    public GameObject FastTabObject;
    public ItemStack ItemStack;
    public int Duration;
    public DateTime Start;

    public FastTab(ItemStack stack, int duration, DateTime start)
    {
        this.ItemStack = stack;
        this.Duration = duration;
        this.Start = start;
    }

    public FastTab(int duration, DateTime start)
    {
        this.ItemStack = null;
        this.Duration = duration;
        this.Start = start;
    }

}
