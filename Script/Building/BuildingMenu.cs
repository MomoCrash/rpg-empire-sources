using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{

    public PlayerManager Player;

    [SerializeField] TilemapControler Controler;
    private Animator _animator;
    [SerializeField] Text menuName;

    [SerializeField] public AgeBuilds[] Buildings;

    [SerializeField] GameObject CaseModel;
    [SerializeField] GameObject[] GeneratedCase;
    [SerializeField] float BaseY, BaseX, CaseSize;

    public GameObject SelectedBuild = null;
    public int SelectedBuildIndex = -1;
    public int CurrentPage = 0;

    public float EnergyCost = 0;
    public Material[] MaterialsCost;
    public int[] MaterialCostAmount;

    public bool HasChoose = false;
    public bool IsOpen = false;
 
    // Start is called before the first frame update
    void Start()
    {

        _animator = gameObject.GetComponent<Animator>();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R) && !IsOpen && !HasChoose)
        {
            ShowPage(0);
            IsOpen = true;
            _animator.SetBool("open", true);
        } else if (Input.GetKeyDown(KeyCode.R) && !IsOpen && HasChoose){
            CloseMenu();
        } else if (Input.GetKeyDown(KeyCode.R) && IsOpen && HasChoose)
        {
            CloseMenu();
        } else if (Input.GetKeyDown(KeyCode.R) && IsOpen)
        {
            CloseMenu();
        }

    }

    public void PreviousPage()
    {

        if (CurrentPage == 0) {
            return;
        }

        CurrentPage -= 1;
        ShowPage(CurrentPage);

    }

    public void NextPage()
    {

        if (CurrentPage == Buildings.Length-1) {
            return;
        }

        CurrentPage += 1;
        ShowPage(CurrentPage);

    }

    private void ShowPage(int pageIndex)
    {

        CurrentPage = pageIndex;
        menuName.text = Buildings[CurrentPage].AgeName;
        if (GeneratedCase is null)
        {
            GeneratedCase = new GameObject[Buildings[CurrentPage].Builds.Length];
        } else
        {

            foreach ( GameObject gameObject in GeneratedCase )
            {

                Destroy(gameObject);

            }
            GeneratedCase = new GameObject[Buildings[CurrentPage].Builds.Length];
        }

        for (int index = 0; index < Buildings[CurrentPage].Builds.Length; index++)
        {

            var build = Buildings[pageIndex].Builds[index].transform.GetChild(0);
            var buildInfo = build.GetComponent<BuildingSign>();
            var buildManager = build.GetComponent<BuildManager>();

            var gain = buildInfo.FormatedGain;
            var workCost = buildInfo.FormatedWorkCost;
            var icon = buildInfo.Sprite;

            string cost = "Cost :\n";
            foreach (int costIndex in Enumerable.Range(0, buildManager.BuildMaterialsCost.Length))
            {
                cost += buildManager.BuildMaterialsCost[costIndex] + "x" + buildManager.BuildAmountCost[costIndex] + "\n";
            }

            Vector3 vector = new(BaseX, (BaseY + (CaseSize * -(index))), 0);

            GameObject newCase = Instantiate(CaseModel, new Vector3(), Quaternion.identity);
            newCase.transform.SetParent(gameObject.transform);
            newCase.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            newCase.transform.localPosition = vector;
            newCase.transform.Find("Case - Name").GetComponent<Text>().text = buildInfo.TitleText;
            newCase.transform.Find("Case - Description").GetComponent<Text>().text = buildInfo.ContentText;
            newCase.transform.Find("Case - Gain").GetComponent<Text>().text = buildInfo.FormatedGain;
            newCase.transform.Find("Case - Cons").GetComponent<Text>().text = workCost;
            newCase.transform.Find("Case - Price").GetComponent<Text>().text = cost;
            newCase.transform.Find("Case - Icon").GetComponent<Image>().sprite = icon;
            var a = index;
            newCase.transform.Find("Case - Build").GetComponent<Button>().onClick.AddListener(delegate { SetBuild(a, buildManager.WorkEnergyCost, buildManager.BuildMaterialsCost, buildManager.BuildAmountCost); });

            newCase.SetActive(true);
            GeneratedCase[index] = newCase;

        }

    }

    private void CloseMenu()
    {
        SelectedBuild = null;
        HasChoose = false;
        IsOpen = false;
        _animator.SetBool("open", false);
        Controler.SetBuildMapState(false);
    }

    void SetBuild(int index, float energyCost, Material[] materialCost, int[] amountNeed)
    {

        SelectedBuildIndex = index;
        SelectedBuild = Buildings[CurrentPage].Builds[index];
        HasChoose = true;
        Controler.SetBuildMapState(true);

        this.EnergyCost = energyCost;
        this.MaterialsCost = materialCost;
        this.MaterialCostAmount = amountNeed;

        _animator.SetBool("open", false);
        IsOpen = false;

    }

}

[System.Serializable]
public class AgeBuilds
{

    public Age Age;
    public string AgeName;
    public GameObject[] Builds;

}

public enum Age
{

    AGE_I,
    AGE_II,
    AGE_III,

}
