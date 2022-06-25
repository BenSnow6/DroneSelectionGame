using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface ICommand
{
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


    void Execute(); // Inherited members must have execute function

    void Undo(); // Inherited members must have execute function

}
