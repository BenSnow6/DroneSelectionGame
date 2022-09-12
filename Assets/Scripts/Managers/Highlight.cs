using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
public class Highlight : MonoBehaviour
{
    /// <summary>
    /// Takes the current mouse position and shows a highlight tile
    /// at the grid location it is hovering over.
    /// Shows a tooltip with the risk value for the highlighted tile at the same location.

    public Vector2 movementInput;
    public Vector3 mousePos;
    public Vector3 mouseLocation;
    public Vector3Int tileLocalPos;


    private Grid grid;
    private GridInformation gridInfo;
    private float maxRisk; // Max risk, used to normalise the risk map
    private Vector3Int previousMousePos = new Vector3Int();
    [SerializeField] private Tilemap interactiveGrid = null;
    [SerializeField] private Tilemap backgroundGrid = null;
    [SerializeField] private Tile hoverTile = null;




    // Start is called before the first frame update
    void Start()
    {
        /// <summary>
        /// Initialise the grid and gridInformation objects
        /// </summary>

        grid = gameObject.GetComponent<Grid>();
        gridInfo = backgroundGrid.GetComponent<GridInformation>();
        maxRisk = 0.000377f; // To be changed to the risk associated with driving a car this distance
    }

    // Update is called once per frame
    void Update()
    {

        if (inGridBounds(TilePosition(mouseLocation)))
        {
            tileLocalPos = TilePosition(mouseLocation);
            showHighlight(tileLocalPos, previousMousePos);
            showToolTip(tileLocalPos);
        }
        else
        {
            TooltipManager._instance.HideToolTip();
        }

        // Vector3 mousePos = GetMousePosition();
        // Vector3Int tileLocalPos = TilePosition(mousePos);
        
        // if(inGridBounds(mousePos))
        // {
        //     showHighlight(tileLocalPos, previousMousePos);
        //     showToolTip(tileLocalPos);
        // }
        // else
        // {
        //     TooltipManager._instance.HideToolTip(); // Remove tooltip
        // }
    }

    /// <summary>
    /// Functions used to show highlight and tooltip
    /// </summary>

    // Vector3 GetMousePosition()
    // {
    //     Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     return grid.WorldToCell(mouseWorldPos);
    // }

    Vector3Int TilePosition(Vector3 mousePos)
    {
        return new Vector3Int((int) Mathf.Floor(mousePos.x), (int) Mathf.Floor(mousePos.y), 0);
    }

    bool inGridBounds(Vector3 mousePos)
    {
        /// <summary>
        /// Input: mousePosition
        /// Checks if mousePosition is between the height and width of the grid
        /// Outputs true/false
        /// Hardcoded grid bounds need to be changed for different sized grid
        /// </summary>
        return 0 <= mousePos.x && mousePos.x <= 9 && 0 <= mousePos.y && mousePos.y <= 7;
    }

    void showHighlight(Vector3Int mousePosition, Vector3Int previousMousePosition)
    {
        if (!mousePosition.Equals(previousMousePos))
        {
            interactiveGrid.SetTile(previousMousePos,null); // remove old highlight tile
            interactiveGrid.SetTile(mousePosition, hoverTile); // place highlight tile at current mouse position
            previousMousePos = mousePosition;
        }
    }
    void showToolTip(Vector3Int tileLocalPos)
    {
        float riskVar = gridInfo.GetPositionProperty(tileLocalPos, "Risk", 1.0f);
        float riskNorm = riskVar/maxRisk;
        float tuningFactor =  riskNorm; // we want to map the colour space more evenly. It goes straight to red too early
        TooltipManager._instance.SetAndShowToolTip("Risk rating", riskNorm.ToString("F2"), new Color(255, 1-tuningFactor, 0,255));
    }

    public void HoverLocation(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        mousePos = new Vector3(movementInput.x, movementInput.y, 0);
        mouseLocation = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
