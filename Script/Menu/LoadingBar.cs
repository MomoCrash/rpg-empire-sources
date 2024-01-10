using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class LoadingBar : MonoBehaviour
{

    public Slider Bar;
    public TextMeshProUGUI BarText;

    public void ActivateBar()
    {
        Bar.gameObject.SetActive(true);
    }

    public void UpdateBar(float progress)
    {
        BarText.text = "Loading - " + (progress * 100) + "%";
        Bar.value = progress;
    }

}
