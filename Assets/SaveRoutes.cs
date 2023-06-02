using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveRoutes : MonoBehaviour
{
    [SerializeField] SelectionManager selectionManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the key press "N"
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Save the current route
            // Debug.Log("Saving route");
            SaveRoute();
        }
    }

    void SaveRoute()
    {
        // Get the accumulatedRisk amount from the SelectionManager
        // float risk = selectionManager.accumulatedRisk;
        // Debug.Log($"Risk is {risk}");

    }
}
