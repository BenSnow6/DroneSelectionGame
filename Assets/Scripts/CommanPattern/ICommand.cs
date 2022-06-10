using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public Vector3Int clickedLocation
    {
        get;
        set;
    }
    void Execute();

    void Undo();

}
