using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class HighlightController : MonoBehaviour
{
    // Start is called before the first frame update
private Grid grid;
private GridInformation gridInfo;
[SerializeField] private Tilemap interactiveGrid = null;
[SerializeField] private Tilemap selectionGrid = null;
[SerializeField] private Tilemap backgroundGrid = null;
[SerializeField] private Tilemap surroundingGrid = null;
[SerializeField] private Tile hoverTile = null;
[SerializeField] private Tile selectionTile = null;
[SerializeField] private Tile surroundingTile = null;
private Vector3Int previousMousePos = new Vector3Int();
private Vector3Int[] nearestNeighbours = new Vector3Int[4];
private Vector3Int[] previousNearestNeighbours = new Vector3Int[4];
private float maxRisk;
private SelectionManager _selectionManager = null;



    void Start()
    {
        // Store the grid component
        grid = gameObject.GetComponent<Grid>();
        gridInfo = backgroundGrid.GetComponent<GridInformation>();
        maxRisk = 0.000377f;
        _selectionManager = gameObject.GetComponent<SelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {

        // Show highlight tile on the interactive map when the mouse is over it and is within the selection grid bounds

        // Get the mouse position
        Vector3Int mousePos = GetMousePosition();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tileLocalPos = new Vector3Int((int) Mathf.Floor(mousePos.x), (int) Mathf.Floor(mousePos.y), 0);
        if(inGridBounds(mousePos)){
            
            addTile(tileLocalPos);
            removeTile(tileLocalPos);
            showHighlight(tileLocalPos, previousMousePos);



            if(Input.GetMouseButton(0)){
                if(!mousePos.Equals(previousMousePos)){
                    removeNearestNeighbours(nearestNeighbours);
                }
            }
        
            float riskVar = gridInfo.GetPositionProperty(tileLocalPos, "Risk", 1.0f);
            float riskCol = maxRisk/(riskVar*255);
            float riskNorm = riskVar/maxRisk;
            float tuningFactor =  riskNorm; // we wanna map the colour space more evenly. It goes straight to red too early
            TooltipManager._instance.SetAndShowToolTip("Risk rating", riskNorm.ToString("F2"), new Color(255, 1-tuningFactor, 0,255));

        }
        else
        {
            TooltipManager._instance.HideToolTip();
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
            ICommand select = new Selection(mousePosition, surroundingGrid, selectionGrid, selectionTile, surroundingTile);
            _selectionManager.commandHandler.AddCommand(select as Selection);
            // selectionGrid.SetTile(mousePosition, selectionTile);
        }
    }
    
    void removeTile(Vector3Int mousePosition)
    {
     if (Input.GetMouseButtonDown(1))
        {
            selectionGrid.SetTile(mousePosition, null);

        }
    }

    void addNearestNeighbours(Vector3Int[] nnPositions, Vector3Int mousePosition, Vector3Int previousMousePosition){
        foreach (Vector3Int neighbourPos in nnPositions)
        {
            if(inGridBounds(neighbourPos)){
                addTile(neighbourPos);
            }
        }
    }

    void removeNearestNeighbours(Vector3Int[] nnPositions){
        foreach (Vector3Int neighbourPos in nnPositions)
        {
            if(inGridBounds(neighbourPos)){
                selectionGrid.SetTile(neighbourPos,null);
            }
        }
    }


    void showHighlight(Vector3Int mousePosition, Vector3Int previousMousePosition){
        if (!mousePosition.Equals(previousMousePos)){
            interactiveGrid.SetTile(previousMousePos,null); // remove old highlight tile
            interactiveGrid.SetTile(mousePosition, hoverTile); // place highlight tile at current mouse position
            previousMousePos = mousePosition;
        }
    }




    bool inGridBounds(Vector3Int mousePosition){
        return 0 <= mousePosition.x && mousePosition.x <= 9 && 0 <= mousePosition.y && mousePosition.y <= 7;
    }


}




// nearest neighbours

//             // set the nearest neighbour positions
//             nearestNeighbours[0] = new Vector3Int(mousePos.x-1, mousePos.y, 0);
//             nearestNeighbours[1] = new Vector3Int(mousePos.x+1, mousePos.y, 0);
//             nearestNeighbours[2] = new Vector3Int(mousePos.x, mousePos.y+1, 0);
//             nearestNeighbours[3] = new Vector3Int(mousePos.x, mousePos.y-1, 0);

//             //addNearestNeighbours(nearestNeighbours, mousePos, previousMousePos);
//             // place tiles at nearest neighbour positions

        /*  
            check if clicked click

            set all the nearest neighbours in an array
            
            place tiles at their locations (check they are in bounds)


            
            set previousNN positions







            */

