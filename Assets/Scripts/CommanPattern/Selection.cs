using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Selection : ICommand
{

    //// Need to find a way of either passing these to the function when calling it or finding a way to get references to them! Then should work...
    private Tilemap surroundingGrid = null;
    private Tilemap selectionGrid = null;
    private Tile selectionTile = null;
    private Tile surroundingTile = null;
    private Vector3Int tileLocalPos = Vector3Int.zero;
    private Vector3Int prevTileLocalPos = Vector3Int.zero;

    public Selection(Vector3Int tileLocalPos, Vector3Int prevTileLocalPos, Tilemap surroundingGrid, Tilemap selectionGrid, Tile selectionTile, Tile surroundingTile)
    {
     this.tileLocalPos = tileLocalPos;
     this.prevTileLocalPos = prevTileLocalPos;
     this.surroundingGrid = surroundingGrid;
     this.selectionGrid = selectionGrid;
     this.selectionTile = selectionTile;
     this.surroundingTile = surroundingTile;
    }


    public void Execute()
    {
        Debug.Log($"mousPos: {tileLocalPos}, prevPos: {prevTileLocalPos}");
        surroundingGrid.SetTile(prevTileLocalPos - new Vector3Int(0,1,0), null);
        surroundingGrid.SetTile(prevTileLocalPos + new Vector3Int(0,1,0), null);
        surroundingGrid.SetTile(prevTileLocalPos - new Vector3Int(1,0,0), null);
        surroundingGrid.SetTile(prevTileLocalPos - new Vector3Int(0,1,0), null);
        selectionGrid.SetTile(tileLocalPos, selectionTile);
        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(1,0,0), surroundingTile);
        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(0,1,0), surroundingTile);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(1,0,0), surroundingTile);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(0,1,0), surroundingTile);
    }

    public void Undo()
    {
        selectionGrid.SetTile(tileLocalPos, null);
        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(1,0,0), null);
        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(0,1,0), null);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(1,0,0), null);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(0,1,0), null);
    }



}
