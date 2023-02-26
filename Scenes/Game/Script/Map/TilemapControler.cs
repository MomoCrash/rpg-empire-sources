using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapControler : MonoBehaviour
{

    public GameObject BuildMap;
    public Tilemap BuildBoxController;
    public Tilemap MapBoxController;
    
    public Tile HightLightTile;
    public Building Building;

    
    public void HightlightTilesInRange(Vector3Int start, Vector3Int end)
    {

        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                HightlightTileAtCellPostion(new Vector3Int(x, y));
            }

        }

    }

    public void HightlightTileAtWorldPosition(Vector3 vec)
    {
        BuildBoxController.SetTile(GetCellAtWorldPosition(BuildBoxController, vec), HightLightTile); ;
    }

    public void HightlightTileAtCellPostion(Vector3Int vec)
    {
        BuildBoxController.SetTile(vec, HightLightTile);
    }

    public TileBase GetTileAtCellPostion(Tilemap tilemap, Vector3Int cellPosition)
    {
        return tilemap.GetTile(cellPosition);
    }

    public Vector3Int GetCellAtWorldPosition(Tilemap map, Vector3 vector)
    {
        return BuildBoxController.WorldToCell(vector);
    }

    public Vector3Int GetMouseCellPosition(Tilemap tilemap)
    {

        Vector3Int cellPosition = tilemap.WorldToCell(GetMouseWorldPosition());
        if (cellPosition.z != 0)
        {
            cellPosition -= new Vector3Int(0, 0, -tilemap.WorldToCell(GetMouseWorldPosition()).z);
        }
        return cellPosition;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;
        return vec;
    }

    public void SetBuildMapState(bool state)
    {
        BuildMap.SetActive(state);
    }

    public bool IsUsedTileArea(Vector3Int start, Vector3Int end)
    {

        for (int x = start.x; x < end.x; x++)
        {
            for (int y = start.y; y < end.y; y++)
            {
                if (IsUsedTile(new Vector3Int(x, y)))
                {
                    return true;
                }
            }

        }

        return false;

    }

    public bool IsUsedTile(Vector3Int cell)
    {
        Debug.Log(GetTileAtCellPostion(BuildBoxController, cell));
        if (GetTileAtCellPostion(BuildBoxController, cell) == HightLightTile)
        {
            return true;
        }
        return false;

    }

}
