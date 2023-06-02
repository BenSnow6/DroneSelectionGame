using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.InputSystem;
using System.IO;
using System;
using System.Globalization;


public class ClickController : MonoBehaviour
{


public Vector2 movementInput;
public Vector3 mousePos;
public Vector3 mouseLocation;
public Vector3Int tileLocalPos;
public bool clickSelect = false;
public bool clickUndo = false;
public int undoCounter = 0;
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
MainManager mainManager = MainManager.Instance;



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
        // For making the grid cells smaller, set these to be the bounds of one of the tilemaps (Check how it's done in the TileMapManager)
        // Need to cheange the Environment->Grid->Cell Size to be 0.5, 0.5 or smaller
        // Need to then re-import the tiles and have their size per pixles to be smaller
        // Each of the tilemaps will have to be shrunk too
        // Risk overlay can stay the same size
        // The background image can stay the same
        // The selection image (high res) needs to be split into more squares, resolution can be the same (or increased if you wish)
        // Check the tileLocalPos calculation because I think it will change and not give the correct position
        // Reading the risk data will also need to be changed
        //     - The risk map needs to be recalculated from the .ipynb file and saved as a new csv file 
        //     - The gridinformation then needs to be updated in the TileMapManager
        // The other tiles need to be re-imported with a different scale per pixel (hoverTile, selectionTile, surroundingTile, startTile, endTile)
        return 0 <= mousePosition.x && mousePosition.x <= mainManager.x_grid_width && 0 <= mousePosition.y && mousePosition.y <= mainManager.y_grid_width;
    }
    public void selectTile(Vector3Int mousePosition)
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
                        if(compareSurrounding(mousePosition) && compareAlreadySelected()) 
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
                Debug.Log($"index is {_selectionManager.commandHandler.index}");
                _selectionManager.commandHandler.UndoCommand();
                undoCounter += 1;
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

    public void submitRoute()
    {
        if (clickedNewInput)
        {
            Debug.Log("Route submitted");
            MainManager.Instance.clickedLocations = _selectionManager.commandHandler.selectedLocations;
            MainManager.Instance.accumulatedRisk = _selectionManager.commandHandler.accumulatedRisk;
            MainManager.Instance.BatteryLeft = _selectionManager.commandHandler.batteryLevel;
            
            clickedNewInput = false;
            SaveRoutes();
        }
    }


    public void setInputTrue()
    {
        clickedNewInput = true;
        Debug.Log("Clicked new input");
    }

    public Vector3Int TilePosition(Vector3 mousePos)
    {
        // Debug.Log($"tile position: {new Vector3Int((int) Mathf.Floor(mousePos.x), (int) Mathf.Floor(mousePos.y), 0)}");
        return new Vector3Int((int) Mathf.Floor(mousePos.x), (int) Mathf.Floor(mousePos.y), 0);
    }

    public void HoverLocation(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        mousePos = new Vector3(movementInput.x, movementInput.y, 0);
        mouseLocation = Camera.main.ScreenToWorldPoint(mousePos);
        // Debug.Log($"mouse loc is {mouseLocation}");
        mouseLocation *= 2;
        tileLocalPos = TilePosition(mouseLocation);
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
    public void TapSelect(InputAction.CallbackContext context)
    {
        Debug.Log($"Tap select is {clickSelect}");
        clickSelect = !clickSelect;
    }

    public void SaveRoutes()
    {
        // Get accumulatedRisk from selectionManager
        float riskLevel = _selectionManager.commandHandler.accumulatedRisk;
        Debug.Log($"Risk level is {riskLevel}");
        // Get the current battery level from the selectionManager
        float batteryLevel = _selectionManager.commandHandler.batteryLevel;
        Debug.Log($"Battery level is {batteryLevel}");
        // Save the route stats to a csv file
        DateTime now = DateTime.Now;
        string timestamp = now.ToString("yyMMdd_HHmmss");
        string path = Application.persistentDataPath + "/RouteStats/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path += timestamp + ".csv"; //"Assets/Resources/RouteStats.csv";

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine($"{riskLevel}, {batteryLevel}, {undoCounter}");
            CultureInfo culture = CultureInfo.InvariantCulture;
            List<Vector3Int> Route = _selectionManager.commandHandler.selectedLocations;
            foreach (Vector3Int vector in Route)
            {
                writer.WriteLine($"{vector.x.ToString(culture)}, {vector.y.ToString(culture)}");
            }
        }

    }

    public bool compareAlreadySelected()
    {
        /// <summary>
        /// Compare the current tile with the list of already selected tiles
        /// </summary>
        var alreadySelected = _selectionManager.commandHandler.selectedLocations;
        if(alreadySelected.Contains(tileLocalPos))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}