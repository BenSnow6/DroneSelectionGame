using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Selection : ICommand
{

    //// Need to find a way of either passing these to the function when calling it or finding a way to get references to them! Then should work...
    [SerializeField] private Tilemap surroundingMap = null;
    [SerializeField] private Tile selectionTile = null;
    [SerializeField] private Tile surroundingTile = null;
    private Vector3Int tileLocalPos = Vector3Int.zero;

    public Selection(Vector3Int tileLocalPos)
    {
     this.tileLocalPos = tileLocalPos;   
    }


    public void Execute()
    {
        surroundingMap.SetTile(tileLocalPos, selectionTile);
        surroundingMap.SetTile(tileLocalPos + new Vector3Int(1,0,0), surroundingTile);
    }

    public void Undo()
    {

    }



}
