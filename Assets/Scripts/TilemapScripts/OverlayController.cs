using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlayController : MonoBehaviour
{
    [SerializeField] private Tilemap riskMap = null;

    // Update is called once per frame
    void Start()
    {
        SetSelectedColour();
    }

    void SetSelectedColour()
    {
        for (int x = riskMap.cellBounds.xMin; x < riskMap.cellBounds.xMax; x++)
            {for (int y = riskMap.cellBounds.yMin; y < riskMap.cellBounds.yMax; y++)
                {
                    Debug.Log($"x: {x} y: {y}");
                    // Assign local position as index
                    Vector3Int localPlace = (new Vector3Int(x, y, 0));
                    Color currentColour = riskMap.GetColor(localPlace);
                    Color newColour = new Color (currentColour.r, currentColour.g, currentColour.b, 0.8f);
                    riskMap.SetTileFlags(localPlace, TileFlags.None);
                    riskMap.SetColor(localPlace, newColour);
                }
            }



    }

}
