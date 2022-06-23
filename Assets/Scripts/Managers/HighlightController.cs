using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Linq;


public class HighlightController : MonoBehaviour
{
private Grid grid;
private GridInformation gridInfo;
[SerializeField] private Tilemap selectionGrid = null;
[SerializeField] private Tilemap backgroundGrid = null;
[SerializeField] private Tilemap surroundingGrid = null;
[SerializeField] private Tile selectionTile = null;
[SerializeField] private Tile surroundingTile = null;
private Vector3Int previousMousePos = new Vector3Int();
private SelectionManager _selectionManager = null; // Instance of the selectionManager



    void Start()
    {
        /// <summary>
        /// Initialise the grid and game objects
        /// </summary>

        grid = gameObject.GetComponent<Grid>();
        gridInfo = backgroundGrid.GetComponent<GridInformation>();
        _selectionManager = gameObject.GetComponent<SelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {

        /// <summary>
        /// Update the current mouse position and convert to grid coordinates
        /// Check if the mouse is in the gridBounds
        /// Check if left mouse has been clicked
        /// </summary>

        // Get the mouse position
        Vector3Int mousePos = GetMousePosition();
        Vector3Int tileLocalPos = new Vector3Int((int) Mathf.Floor(mousePos.x), (int) Mathf.Floor(mousePos.y), 0);
        if(inGridBounds(mousePos)){
            selectTile(tileLocalPos);
            removeTile(tileLocalPos);
        }
    }

    Vector3Int GetMousePosition(){
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    void addTile(Vector3Int mousePosition)
    {
     if (Input.GetMouseButtonDown(0))
        {
            ICommand select = new Selection(mousePosition, previousMousePos, surroundingGrid, selectionGrid, selectionTile, surroundingTile);
            _selectionManager.commandHandler.AddCommand(select as Selection);
            //select.clickedLocation = mousePosition;
            previousMousePos = mousePosition;
            // selectionGrid.SetTile(mousePosition, selectionTile);
        }
    }
    
    void removeTile(Vector3Int mousePosition)
    {
     if (Input.GetMouseButtonDown(1))
        {
            _selectionManager.commandHandler.UndoCommand();
            var lastSelectedPosition = _selectionManager.commandHandler.commandList.LastOrDefault();
            Debug.Log($"Clicked location is undone {lastSelectedPosition.clickedLocation}");
            selectionGrid.SetTile(mousePosition, null);

        }
    }


    bool inGridBounds(Vector3Int mousePosition){
        return 0 <= mousePosition.x && mousePosition.x <= 9 && 0 <= mousePosition.y && mousePosition.y <= 7;
    }

void selectTile(Vector3Int mousePosition)
    {
     if (Input.GetMouseButtonDown(0))
        {
            /* Create a new instance of the selection command called select and call with all required variables
            Use the selection manager to access the commandHandler and add the new command, select, to the list of commands
            

            Need to check if the mousePosition is one of the surrounding tiles
            To do this, need to know the location of the last selected tile
            To access this, need the commandList.Last().clickedPosition


            /// needs to be "Can't click on tiles that have already been selected.
            Check against the list of clickedLocations and see if the current mousePosition is in that list anywhere.
            Then need to check if the clicked location is one of the surrounding tiles.
            E.g. the mousePosition is one of the surrounding tiles of the last in the list of clickedLocations


            /// Need to get the list of the last location of the tiles



            

            */

            if(_selectionManager.commandHandler.commandList.Count.Equals(0))
            {
            
                ICommand select = new Selection(mousePosition, previousMousePos, surroundingGrid, selectionGrid, selectionTile, surroundingTile);

                select.clickedLocation = mousePosition;
                
                _selectionManager.commandHandler.AddCommand(select as Selection);
            
            }

            if(_selectionManager.commandHandler.commandList.Count > 0){
                var lastSelectedPosition = _selectionManager.commandHandler.commandList.LastOrDefault();
                if(lastSelectedPosition.clickedLocation.Equals(mousePosition))
                {
                    Debug.Log("Can't click here");
                }
                else
                {
                    ICommand select = new Selection(mousePosition, previousMousePos, surroundingGrid, selectionGrid, selectionTile, surroundingTile);

                    select.clickedLocation = mousePosition;
                    
                    _selectionManager.commandHandler.AddCommand(select as Selection);
                }
            }

            
        }
    }

}