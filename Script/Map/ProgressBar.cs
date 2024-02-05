using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public RectTransform BarStart;
    public RectTransform[] BarEnd;
    public Slider SliderBar;

    public int MaxSize;

    private float BaseX;
    private float BaseY;

    private void Start()
    {

        if (BarEnd.Length > 0)
        {
            BaseX = BarEnd[0].localPosition.x;
            BaseY = BarEnd[0].localPosition.y;
        }


    }

    public void SetWidth(float max, float current)
    {
        TransformUtils.SetWidth(BarStart, current * MaxSize / max);
        foreach (RectTransform transform in BarEnd)
        {
            transform.localPosition = new Vector3(BaseX, BaseY, 0) + new Vector3(current * MaxSize / max - 1, 0, 0);
        }
    }

    public void SetScale(float max, float current)
    {
        BarStart.localScale = new Vector3(current * 0.5f / max, 0.5f, 1); ;
    }

    public void SetValue(float max, float current)
    {
        SliderBar.value = current / max;
    }

}
