using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class RiskToggle : MonoBehaviour

{
    [SerializeField] private Tilemap RiskGrid = null;
    public bool buttonOn = false; // toggle to show/hide schools
    public bool alreadyShown = false; // toggle to show/hide schools
    TilemapRenderer renderer;

    void Start()
    {
        renderer = gameObject.GetComponent<TilemapRenderer>();
        buttonOn = false;
        alreadyShown = false;
    }
    void Update()
    {
        // Debug.Log($"buttonOn: {buttonOn} and alreadyShown: {alreadyShown}");
        if(buttonOn == true)
        {
            if(alreadyShown == false)
            {
                alreadyShown = true;
                showRisk();
            }
        }
        else
        {
            hideRisk();
            alreadyShown = false;
        }
    }



    void showRisk()
    {
        // Set sort order of the tilemap
        renderer.sortingOrder = 10;
    }

    void hideRisk()
    {
        renderer.sortingOrder = 0;
    }

    public void setButtonOn(bool on)
    {
        buttonOn = !buttonOn;
    }

}


