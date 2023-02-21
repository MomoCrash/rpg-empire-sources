using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class FastTabManager : MonoBehaviour
{

    [SerializeField] GameObject FastTabModel;
    private List<FastTab> FastTabs = new List<FastTab>();

    private bool _hasNewTab = false;

    [SerializeField] int baseX, baseY;

    void FixedUpdate()
    {

        var i = 0;
        foreach (var fastTabObject in FastTabs)
        {
            TimeSpan timeActive = (DateTime.Now - fastTabObject.Start);
            if (timeActive.TotalMilliseconds > fastTabObject.Duration*1000)
            {
                GameObject.Destroy(fastTabObject.FastTabObject);
                FastTabs.RemoveAt(i);
                break;
            }
            i++;
        }

        if (_hasNewTab)
        {
            Dictionary<int, FastTab> fastTabDict = new Dictionary<int, FastTab>();
            foreach (int index in Enumerable.Range(0, FastTabs.Count))
            {
                var tab = FastTabs[index];
                if ( fastTabDict.ContainsKey(tab.ItemStack.Item.UniqueId) )
                {
                    FastTab fastTabDictValue = fastTabDict.GetValueOrDefault(tab.ItemStack.Item.UniqueId);
                    fastTabDictValue.ItemStack.Amount += tab.ItemStack.Amount;
                    fastTabDictValue.FastTabObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text 
                        = fastTabDictValue.ItemStack.Item.Name + "x" + fastTabDictValue.ItemStack.Amount;
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
        fastTabObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tab.ItemStack.Item.Name + " x " + tab.ItemStack.Amount;
        fastTabObject.transform.localPosition = new Vector3(baseX, baseY, 0);
        tab.FastTabObject = fastTabObject;
        FastTabs.Add(tab);
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

}
