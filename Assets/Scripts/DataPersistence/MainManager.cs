using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    /// <summary>
    /// Allows for data persistence to be used in the game.
    /// </summary>

    public static MainManager Instance;
    public List<Vector3Int> clickedLocations;  // Save the locations of the clicked tiles to pass to flyover scene.

    private void Awake()
    {
        // Only allow one instance of the MainManger to exist.
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object.
        // Do not allow the MainManager to be destroyed.
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}


/// Used with help from: https://learn.unity.com/tutorial/implement-data-persistence-between-scenes#

/// This method is called a Singleton pattern. Only one instance of the MainManager can exist at a time.