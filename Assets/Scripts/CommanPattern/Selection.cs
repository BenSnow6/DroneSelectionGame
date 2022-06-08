using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Selection : ICommand
{
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
