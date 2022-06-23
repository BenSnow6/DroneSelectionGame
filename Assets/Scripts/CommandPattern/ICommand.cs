using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public Vector3Int clickedLocation
    {
        /// To be changed to List<Vector3Int> selectedLocations which will hold all locations

        get;
        set;
    }


    void Execute(); // Inherited members must have execute function

    void Undo(); // Inherited members must have execute function

}
