using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(Tilemap))]


public class TileMapManager : MonoBehaviour
{
    Tilemap _tilemap;
    private int numCols, numRows;
    GridInformation GridInfo;
    public TextAsset RiskMapData;
    private string[] riskData;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        // get ref to spriterenderer
        sprite = GetComponent<SpriteRenderer>();
        // Assign the Tilemap component
        _tilemap = GetComponent<Tilemap>();
        // Instantiate a new GridInformation component
        GridInfo = GetComponent<GridInformation>();
        // Populate the GridInfo
        setTileData();
    }

    // Update is called once per frame
    void Update()
    {
        clickTile();
    }

    void setTileData()
    {
        // Read in the risk map data from CSV
        riskData = ReadCSV();

        // loop over all tiles from min x to max x for each y from min y to max y
        for (int x = _tilemap.cellBounds.xMin; x < _tilemap.cellBounds.xMax; x++){
            for (int y = _tilemap.cellBounds.yMin; y < _tilemap.cellBounds.yMax; y++){
                // Assign local position as index
                Vector3Int localPlace = (new Vector3Int(x, y, 0));

                if (_tilemap.HasTile(localPlace)){
                    // Set properties to data dictionary
                    GridInfo.SetPositionProperty(localPlace,"x", x);
                    GridInfo.SetPositionProperty(localPlace,"y", y);
                    int riskIndex = numCols*(numRows-1-y)+x; // transformation from x,y index to corresponding position in csv file
                    GridInfo.SetPositionProperty(localPlace, "Risk", float.Parse(riskData[riskIndex])); // save risk value to grid
                    if(x==3 && y==4){
                        // SetSelectedColour(localPlace);
                        // Debug.Log("set colour");
                    }
                
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }

    void clickTile()
    {
     if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tileLocalPos = new Vector3Int((int) Mathf.Floor(pos.x), (int) Mathf.Floor(pos.y), 0);;
            // Debug.Log(string.Format($"Co-ords of mouse are [X: {(int) pos.x} Y: {(int) pos.y} Risk: {GridInfo.GetPositionProperty(tileLocalPos, "Risk", 0.0f)}]"));

        }
    }

    string[] ReadCSV()
    {
        string[] data = RiskMapData.text.Split(new string[] {",", "\n"}, System.StringSplitOptions.None);
        string[] data_rows = RiskMapData.text.Split(new string[] {"\n"}, System.StringSplitOptions.None);
        numRows = data_rows.Length-1; // length of dataRows is given as the actual number of rows+1 but the last row is empty
        numCols = (data.Length-1)/(numRows);
        return data;
    }

    void SetSelectedColour(Vector3Int tilePosition)
    {
        Color currentColour = _tilemap.GetColor(tilePosition);
        Color newColour = new Color (currentColour.r, currentColour.g, currentColour.b, 0.2f);
        Debug.Log(newColour);
        _tilemap.SetTileFlags(tilePosition, TileFlags.None);
        _tilemap.SetColor(tilePosition, Color.black);



    }
}

/* Helper:


How to get a grid position property:

    GridInfo.GetPositionProperty(new Vector3Int(0,0,0), "pos", 0);

where the Vector3Int is the local tile position, "pos" is the name of the property stored, and 0 is the default to be returned if there is no value stored. (0 for int, "No value" for string etc...)


*/
