using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.InputSystem;


public class ClickController : MonoBehaviour
{


public Vector2 movementInput;
public Vector3 mousePos;
public Vector3 mouseLocation;
public Vector3Int tileLocalPos;
public bool clickSelect = false;
public bool clickUndo = false;
private bool startingCondition = true;
private Grid grid;
private GridInformation gridInfo;
[SerializeField] private Tilemap selectionGrid = null;
[SerializeField] private Tilemap backgroundGrid = null;
[SerializeField] private Tilemap surroundingGrid = null;
[SerializeField] private Tile selectionTile = null;
[SerializeField] private Tile surroundingTile = null;
private Vector3Int previousMousePos = new Vector3Int();
public bool clickedNewInput = false;
private SelectionManager _selectionManager = null; // Instance of the selectionManager



    void Start()
    {
        /// <summary>
        /// Initialise the grid and component references.
        /// </summary>

        grid = gameObject.GetComponent<Grid>();
        gridInfo = backgroundGrid.GetComponent<GridInformation>();
        _selectionManager = gameObject.GetComponent<SelectionManager>();
        selectTile(new Vector3Int(1,1,0));
        startingCondition = false;
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
        
        // Vector3Int mousePos = GetMousePosition();
        // Vector3Int tileLocalPos = TilePosition(mousePos);

        // Mouse position is mouseLocation
        // tileLocalPos is the tile position on the grid
        

        if(inGridBounds(tileLocalPos)){
            selectTile(tileLocalPos);
            removeTile(tileLocalPos, false);
        }
        submitRoute();
    }

    bool inGridBounds(Vector3Int mousePosition)
    {
        return 0 <= mousePosition.x && mousePosition.x <= 9 && 0 <= mousePosition.y && mousePosition.y <= 7;
    }
    void selectTile(Vector3Int mousePosition)
    {
     if (clickSelect || startingCondition)
        {
            /// <summary>
            /// Create a new instance of the selection command called select and call with all required variables
            /// Use the selection manager to access the commandHandler and add the new command, select, to the list of commands
            

            /// Need to check if the mousePosition is one of the surrounding tiles
            /// To do this, need to know the location of the last selected tile
            /// To access this, need the commandList.Last().clickedPosition


            /// needs to be "Can't click on tiles that have already been selected.
            /// Check against the list of clickedLocations and see if the current mousePosition is in that list anywhere.
            /// Then need to check if the clicked location is one of the surrounding tiles.
            /// E.g. the mousePosition is one of the surrounding tiles of the last in the list of clickedLocations


            /// Need to get the list of the last location of the tiles
            /// </summary>


            

            
            if(_selectionManager.commandHandler.commandList.Count.Equals(0))
            {
            
                addSelectionCommand(mousePosition, previousMousePos, surroundingGrid, selectionGrid, selectionTile, surroundingTile, _selectionManager);
            
            }

            if(_selectionManager.commandHandler.commandList.Count > 0){
                if (_selectionManager.commandHandler.index > 0)
                {
                    if(compareLastSelected(mousePosition))
                    {
                        Debug.Log("Can't click here, that's the same as the last selected");
                    }
                    else
                    {
                        if(compareSurrounding(mousePosition))
                        {
                            addSelectionCommand(mousePosition, previousMousePos, surroundingGrid, selectionGrid, selectionTile, surroundingTile, _selectionManager);
                        }
                        else
                        {
                            Debug.Log("Can't click here, it's not a surrounding");
                        }
                    }
                }
                else
                {
                    addSelectionCommand(mousePosition, previousMousePos, surroundingGrid, selectionGrid, selectionTile, surroundingTile, _selectionManager);
                }
            }

            
        }
        clickSelect = false;
    }
    public void removeTile(Vector3Int mousePosition, bool buttonPressed)
        {
        if ((clickUndo || buttonPressed) && _selectionManager.commandHandler.commandList.Count > 1)
            {
                _selectionManager.commandHandler.UndoCommand();
                var lastSelectedPosition = _selectionManager.commandHandler.commandList.LastOrDefault();
                Debug.Log($"Clicked location is undone {lastSelectedPosition.clickedLocation}");
                selectionGrid.SetTile(mousePosition, null);

            }
            clickUndo = false;
        }
    void addSelectionCommand(Vector3Int mousePosition, Vector3Int previousMousePos, Tilemap surroundingGrid, Tilemap selectionGrid, Tile selectionTile, Tile surroundingTile, SelectionManager _selectionManager)
    { 
        ICommand select = new Selection(mousePosition, previousMousePos, surroundingGrid, selectionGrid, selectionTile, surroundingTile, _selectionManager, gridInfo);

        select.clickedLocation = mousePosition;

        _selectionManager.commandHandler.AddCommand(select as Selection);
    }
    bool compareLastSelected(Vector3Int tileLocalPos)
    {
        /// <summary>
        /// Compare the last selected tile with the current tile
        /// </summary>

        var lastSelectedPosition = _selectionManager.commandHandler.commandList.LastOrDefault();
        if(lastSelectedPosition.clickedLocation.Equals(tileLocalPos))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool compareSurrounding(Vector3Int tileLocalPos)
    {
        /// <summary>
        /// Compare the surrounding tiles with the current tile
        /// Only return true if the current tile is one of the surrounding tiles
        /// </summary>

        Vector3Int previousClickedLocation = _selectionManager.commandHandler.selectedLocations[_selectionManager.commandHandler.index - 1];
        bool up = tileLocalPos.Equals(previousClickedLocation + new Vector3Int(1,0,0));
        bool down = tileLocalPos.Equals(previousClickedLocation + new Vector3Int(-1,0,0));
        bool left = tileLocalPos.Equals(previousClickedLocation + new Vector3Int(0,-1,0));
        bool right = tileLocalPos.Equals(previousClickedLocation + new Vector3Int(0,1,0));
        return up || down || left || right;

    }

    void submitRoute()
    {
        if (clickedNewInput)
        {
            Debug.Log("Route submitted");
            MainManager.Instance.clickedLocations = _selectionManager.commandHandler.selectedLocations;
            clickedNewInput = false;
        }
    }


    public void setInputTrue()
    {
        clickedNewInput = true;
        Debug.Log("Clicked new input");
    }

    Vector3Int TilePosition(Vector3 mousePos)
    {
        return new Vector3Int((int) Mathf.Floor(mousePos.x), (int) Mathf.Floor(mousePos.y), 0);
    }

    public void HoverLocation(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        mousePos = new Vector3(movementInput.x, movementInput.y, 0);
        mouseLocation = Camera.main.ScreenToWorldPoint(mousePos);
        tileLocalPos = TilePosition(mouseLocation);
        // Debug.Log($"Mouse position is {mouseLocation}");
    }

    public void SelectClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // clickSelect = !clickSelect;
        }
        else if (context.canceled)
            {
                clickSelect = !clickSelect;
            }
    }
    public void UndoClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // clickUndo = !clickUndo;
        }
        else if (context.canceled)
            {
                clickUndo = !clickUndo;
            }
    }
    
}