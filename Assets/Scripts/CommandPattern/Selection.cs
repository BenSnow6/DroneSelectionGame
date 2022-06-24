using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Selection : ICommand
{
    public Vector3Int clickedLocation
    {
        /// Should be a list of selectedLocations
        /// Stores a contiguous list of the selected tile locations
        get;
        set;
    }

    private Tilemap surroundingGrid = null;
    private Tilemap selectionGrid = null;
    private Tile selectionTile = null;
    private Tile surroundingTile = null;
    public Vector3Int tileLocalPos = Vector3Int.zero; // Depricated, this will be replaced with a list of selectedLocations
    private Vector3Int prevTileLocalPos = Vector3Int.zero; // Depricated, this will be replaced with a list of selectedLocations
    public SelectionManager _selectionManager = null;

    public Selection(Vector3Int tileLocalPos, Vector3Int prevTileLocalPos, Tilemap surroundingGrid, Tilemap selectionGrid, Tile selectionTile, Tile surroundingTile, SelectionManager _selectionManager)
    {
    /// Set the arguemnts of the call to be local variables
        this.tileLocalPos = tileLocalPos;
        this.prevTileLocalPos = prevTileLocalPos;
        this.surroundingGrid = surroundingGrid;
        this.selectionGrid = selectionGrid;
        this.selectionTile = selectionTile;
        this.surroundingTile = surroundingTile;
        clickedLocation = tileLocalPos;
        this._selectionManager = _selectionManager;
    }


    public void Execute()
    {
        /*
        Change the 'prevTileLocalPos variable to the (i-1)th value of the list of selected tile positions
        */
        /// Execute function is called when the user selects a tile from the surrounding tiles
        /// This function then removes the surrounding tiles from the last selected location,
        /// adds a selection tile to the selected location, and then adds the new
        /// surrounding tiles to the new selection tile location.

        if (_selectionManager.commandHandler.index > 0)
        {
            removeSurroundingTiles(_selectionManager.commandHandler.selectedLocations[_selectionManager.commandHandler.index - 1]);
            // Debug.Log($"Current index is {_selectionManager.commandHandler.index} and the current last selected location is {_selectionManager.commandHandler.selectedLocations[_selectionManager.commandHandler.index - 1]}");
        }
        
        
        selectionGrid.SetTile(tileLocalPos, selectionTile);
        placeSurroundingTiles(tileLocalPos);
    }

    public void Undo()
    {
        /// The undo function is called when the user performs the undo command.
        /// This function removes the selection tile from the last selected position,
        /// removes the surrounding tiles fro  the last selected position,
        /// and then adds surrounding tiles to the last - 1 selected position.

        selectionGrid.SetTile(tileLocalPos, null);
        removeSurroundingTiles(tileLocalPos);
        placeSurroundingTiles(_selectionManager.commandHandler.selectedLocations[_selectionManager.commandHandler.index - 2]);
    }




    private void placeSurroundingTiles(Vector3Int tileLocalPos)
    {
        /// <summary>
        /// This function places the surrounding tiles around the selected tile.
        /// It is called when the user selects a tile from the surrounding tiles.
        /// </summary>
        /// <param name="tileLocalPos">The position of the selected tile</param>

        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(1,0,0), surroundingTile);
        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(0,1,0), surroundingTile);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(1,0,0), surroundingTile);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(0,1,0), surroundingTile);
    }

    private void removeSurroundingTiles(Vector3Int tileLocalPos)
    {
        /// <summary>
        /// This function removes the surrounding tiles around the selected tile.
        /// </summary>
        /// <param name="tileLocalPos">The position of the tile around which the surrounding tiles lie </param>

        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(1,0,0), null);
        surroundingGrid.SetTile(tileLocalPos + new Vector3Int(0,1,0), null);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(1,0,0), null);
        surroundingGrid.SetTile(tileLocalPos - new Vector3Int(0,1,0), null);
    }
}
