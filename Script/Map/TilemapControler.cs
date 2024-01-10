using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapControler : MonoBehaviour
{
    private static readonly System.Random random = new();
    public GameObject BuildMap;
    public Tilemap BuildBoxController;

    public List<Tile> MapTiles = new();
    public Tilemap MapBoxController;

    /**
    In chunks of 16 
    */
    readonly int _map_X = 16;
    readonly int _map_Y = 16;
    
    public Tile HightLightTile;
    public Tile UnLightTile;
    public Building Building;

    void Start() {

        foreach (Tile tile in Resources.LoadAll<Tile>("Tiles"))
        {
            MapTiles.Add(tile);
        }

        for (int x = -(_map_X*16/2); x < _map_X*16; x++)
        {
            for (int y = -(_map_Y*16/2); y < _map_Y*16; y++) {

                int tile = random.Next(20);

                SetMapTileAtCellPosition(x, y, MapTiles[tile]);
                UnlightTileAtCellPostion(new Vector3Int(x, y));

            }
        }

    }
    
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

    public void SetMapTileAtCellPosition(int x, int y, Tile tile) {

        MapBoxController.SetTile(GetCellAtWorldPosition(MapBoxController, new Vector3(x, y)), tile); ;

    }

    public void HightlightTileAtWorldPosition(Vector3 vec)
    {
        BuildBoxController.SetTile(GetCellAtWorldPosition(BuildBoxController, vec), HightLightTile); ;
    }

    public void HightlightTileAtCellPostion(Vector3Int vec)
    {
        BuildBoxController.SetTile(vec, HightLightTile);
    }

    public void UnlightTileAtCellPostion(Vector3Int vec)
    {
        BuildBoxController.SetTile(vec, UnLightTile);
    }

    public TileBase GetTileAtCellPostion(Tilemap tilemap, Vector3Int cellPosition)
    {
        return tilemap.GetTile(cellPosition);
    }

    public Vector3Int GetCellAtWorldPosition(Tilemap map, Vector3 vector)
    {
        return map.WorldToCell(vector);
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
        Debug.Log("Le tile ici est : " + GetTileAtCellPostion(BuildBoxController, cell));
        if (GetTileAtCellPostion(BuildBoxController, cell) == HightLightTile)
        {
            return true;
        }
        return false;

    }

}
