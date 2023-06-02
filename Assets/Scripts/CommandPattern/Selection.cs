using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Selection : ICommand
{
    MainManager mainManager = MainManager.Instance;
    public Vector3Int clickedLocation
    {
        /// <summary>
        /// Holds the current selected tile location
        /// </summary>
        get;
        set;
    }
    public GridInformation gridInfo
    {
        /// <summary>
        /// Holds the grid information contatining the risk of each tile
        /// </summary>
        get;
        set;
    }

    private Tilemap surroundingGrid = null;
    private Tilemap selectionGrid = null;
    private Tile selectionTile = null;
    private Tile surroundingTile = null;
    public Vector3Int tileLocalPos = Vector3Int.zero;
    private Vector3Int prevTileLocalPos = Vector3Int.zero;
    public SelectionManager _selectionManager = null;

    public Selection(Vector3Int tileLocalPos, Vector3Int prevTileLocalPos, Tilemap surroundingGrid, Tilemap selectionGrid, Tile selectionTile, Tile surroundingTile, SelectionManager _selectionManager, GridInformation gridInformation)
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
        gridInfo = gridInformation;
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
        Vector3Int[] surroundingLocations = new Vector3Int[] {new Vector3Int(1,0,0), new Vector3Int(0,1,0), new Vector3Int(-1,0,0), new Vector3Int(0,-1,0)};
        foreach (Vector3Int location in surroundingLocations)
        {
            if (inGridBounds(tileLocalPos + location))
            {
                surroundingGrid.SetTile(tileLocalPos + location, surroundingTile);
            }
        }
    }

    private void removeSurroundingTiles(Vector3Int tileLocalPos)
    {
        /// <summary>
        /// This function removes the surrounding tiles around the selected tile.
        /// </summary>
        /// <param name="tileLocalPos">The position of the tile around which the surrounding tiles lie </param>
        Vector3Int[] surroundingLocations = new Vector3Int[] {new Vector3Int(1,0,0), new Vector3Int(0,1,0), new Vector3Int(-1,0,0), new Vector3Int(0,-1,0)};
        foreach (Vector3Int location in surroundingLocations)
        {
                surroundingGrid.SetTile(tileLocalPos + location, null);
        }
    }
    private bool inGridBounds(Vector3Int mousePos)
    {
        /// <summary>
        /// Input: mousePosition
        /// Checks if mousePosition is between the height and width of the grid
        /// Outputs true/false
        /// Hardcoded grid bounds need to be changed for different sized grid
        /// </summary>
        return 0 <= mousePos.x && mousePos.x <= mainManager.x_grid_width && 0 <= mousePos.y && mousePos.y <= mainManager.y_grid_width;
    }

}
