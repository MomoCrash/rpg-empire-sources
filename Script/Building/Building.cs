using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;

public class Building : MonoBehaviour
{

    [SerializeField] GameObject Layer;
    [SerializeField] Tilemap TileMap;

    [SerializeField] TilemapControler Controler;
    [SerializeField] BuildingMenu BuildMenu;
    [SerializeField] Tile[] buildableTiles;

    public SaveSystem SaveSystem;
    public BuildSave BuildSave;

    private GameObject _preview;

    void Start()
    {
        
        if (SaveSystem.IsFirstLoad)
        {
            BuildSave = new BuildSave();
        } else
        {

            foreach (BuildData data in BuildSave.GetBuildsData())
            {

                GameObject build = BuildMenu.Buildings[data.AgeIndex].Builds[data.BuildIndex];
                BuildMenu.SelectedBuildIndex = data.BuildIndex;
                BuildGenerationConstruct(new Vector3Int(data.Position[0], data.Position[1], data.Position[2]), build);
            }

        }

    }

    void FixedUpdate()
    {
        if (BuildMenu.HasChoose)
        {
            PreviewConstruct(BuildMenu.SelectedBuild);
        }
        else
        {
            DestroyImmediate(_preview);
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && BuildMenu.HasChoose)
        {
            var mousseCell = Controler.GetMouseCellPosition(TileMap);
            Debug.Log(mousseCell);
            TileBase tile = Controler.GetTileAtCellPostion(TileMap, mousseCell);
            Debug.Log(tile);

            foreach (Tile buildable in buildableTiles)
            {
                Debug.Log(tile == buildable);
                if (tile == buildable)
                {

                    Collider2D buildCollider = _preview.transform.GetChild(2).GetComponent<Collider2D>();
                    Vector3Int start = Controler.GetCellAtWorldPosition(TileMap, buildCollider.bounds.min);
                    Vector3Int end = Controler.GetCellAtWorldPosition(TileMap, buildCollider.bounds.max);

                    if (!Controler.IsUsedTileArea(start, end))
                    {
                        BuildConstruct(mousseCell, BuildMenu.SelectedBuild);
                    } else
                    {
                        BuildMenu.Player.FastTab.SendFastTabMessageDirect("Déjà utiliser !", 2);
                        return;
                    }
                }
            }
        }
    }

    void PreviewConstruct(GameObject building)
    {

        if (_preview == null)
        {
            _preview = Instantiate(building, Controler.GetMouseCellPosition(TileMap), Quaternion.identity);
            _preview.transform.SetParent(Layer.transform);
            _preview.transform.localPosition = new Vector3(0, 0, 0);
            _preview.name = "build preview";
            _preview.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
            _preview.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        } else
        {
            Collider2D previewCollider = building.transform.GetChild(1).GetComponent<Collider2D>();

            _preview.transform.localPosition = Controler.GetMouseCellPosition(TileMap) - new Vector3(-(previewCollider.bounds.size.x / 2), -previewCollider.bounds.size.y, 0);
        }


    }

    void BuildConstruct(Vector3Int buildCell, GameObject building)
    {

        var itemStacks = new ItemStack[BuildMenu.MaterialsCost.Length];
        foreach ( int index in Enumerable.Range(0, BuildMenu.MaterialsCost.Length))
        {
            itemStacks[index] = new ItemStack(Item.GetItem(BuildMenu.MaterialsCost[index]), BuildMenu.MaterialCostAmount[index]);
            print(BuildMenu.Player.Inventory.HasEnoughItem((new ItemStack(Item.GetItem(BuildMenu.MaterialsCost[index]), BuildMenu.MaterialCostAmount[index]))));
        }

        string[] missingItem = BuildMenu.Player.Inventory.HasEnoughItem(itemStacks);
        if (missingItem.Length > 0)
        {
            BuildMenu.Player.FastTab.SendFastTabMessageDirect("Manque de " + missingItem[0], 2);
            return;
        } else
        {
            foreach ( var item in itemStacks )
            {
                BuildMenu.Player.Inventory.RemoveItem(item);
            }
        }

        GameObject newObject = Instantiate(building, buildCell, Quaternion.identity);
        newObject.transform.SetParent(Layer.transform);
        newObject.name = BuildMenu.SelectedBuildIndex.ToString() + ";" + BuildMenu.CurrentPage.ToString();

        newObject.transform.localPosition = Controler.GetMouseCellPosition(TileMap);
        BuildSave.AddBuild(buildCell, BuildMenu.SelectedBuildIndex, BuildMenu.CurrentPage);

        BuildMenu.HasChoose = false;
        BuildMenu.SelectedBuild = null;

        var buildCollider = newObject.transform.GetChild(2).GetComponent<Collider2D>();

        Vector3Int start = Controler.GetCellAtWorldPosition(TileMap, buildCollider.bounds.min);
        Vector3Int end = Controler.GetCellAtWorldPosition(TileMap, buildCollider.bounds.max);

        Controler.HightlightTilesInRange(start, end);
        Controler.SetBuildMapState(false);

        BuildMenu.Player.gameObject.GetComponent<QuestManager>().BuildBuilding(newObject.transform.GetChild(0).GetComponent<BuildManager>().BuildingID);

    }

    void BuildGenerationConstruct(Vector3Int buildCell, GameObject building)
    {

        GameObject newObject = Instantiate(building, buildCell, Quaternion.identity);
        newObject.transform.SetParent(Layer.transform);
        newObject.name = BuildMenu.SelectedBuildIndex.ToString() + ";" + BuildMenu.CurrentPage.ToString();

        newObject.transform.localPosition = buildCell;

        BuildMenu.SelectedBuild = null;

        var buildCollider = newObject.transform.GetChild(2).GetComponent<Collider2D>();

        Vector3Int start = Controler.GetCellAtWorldPosition(TileMap, buildCollider.bounds.min);
        Vector3Int end = Controler.GetCellAtWorldPosition(TileMap, buildCollider.bounds.max);

        Controler.HightlightTilesInRange(start, end);
        Controler.SetBuildMapState(false);
    }
}

[System.Serializable]
public class BuildSave
{

    public List<BuildData> BuildsData;

    public BuildSave()
    {
        BuildsData = new List<BuildData>();
    }

    public void AddBuild(Vector3 position, int id, int ageIndex)
    {
        BuildsData.Add(new BuildData(position, id, ageIndex)) ;
    }

    public void RemoveBuild(BuildData ressource)
    {
        var index = BuildsData.FindIndex(re => re.Equals(ressource));
        if (index < 0) return;
        BuildsData.RemoveAt(index);
    }

    public BuildData[] GetBuildsData()
    {
        return BuildsData.ToArray<BuildData>();
    }
}

[System.Serializable]
public class BuildData
{

    public int[] Position = new int[3];
    public int BuildIndex;
    public int AgeIndex;

    public BuildData(Vector3 vector, int index, int ageIndex)
    {
        this.Position[0] = (int)vector.x;
        this.Position[1] = (int)vector.y;
        this.Position[2] = (int)vector.z;
        this.BuildIndex = index;
        this.AgeIndex = ageIndex;
    }

    public override bool Equals(object obj)
    {
        if (obj is not RessourceData)
        {
            return false;
        }

        BuildData ressource = (BuildData)obj;
        Debug.Log(ressource.Position[0] == this.Position[0]);

        return (ressource.Position[0] == this.Position[0]
            && ressource.Position[1] == this.Position[1]
            && ressource.Position[2] == this.Position[2]);
    }

}
