using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitRouteButton : MonoBehaviour
{
    public bool buttonPressed;
    Grid grid;
    ClickController clickController;
    [SerializeField] GameManager gameManager;

    void Start()
        {
            Debug.Log("SubmitRouteButton Start");
            grid = GameObject.FindGameObjectWithTag("TagGrid").GetComponent<Grid>();
            clickController = grid.GetComponent<ClickController>();
        }
    void Update()
    {
        if (buttonPressed)
        {
            // Debug.Log("Undo Button Pressed");
            clickController.setInputTrue();
            gameManager.StartFlyoverButton();
            buttonPressed = false;
        }
    }
    public void setPressed(bool pressed)
    {
        buttonPressed = pressed;
        // Debug.Log($"Button pressed is {buttonPressed}");
    }
    public void printIt()
    {
        Debug.Log("Route Button pressed is " + buttonPressed);
    }
}
